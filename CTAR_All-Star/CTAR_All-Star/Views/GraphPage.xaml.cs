﻿using System;
using SQLite;

using Xamarin.Forms;
using CTAR_All_Star.Models;
using Syncfusion.SfChart.XForms;
using CTAR_All_Star.Database;
using System.Timers;
using System.Threading;
using System.Collections.ObjectModel;
using CTAR_All_Star.ViewModels;

namespace CTAR_All_Star
{
    public partial class GraphPage : ContentPage
    {
        private int minute;
        private int second;
        private int repCount = 0;
        private int setCount = 1;
        private int totalReps;
        private int totalSets;
        private System.Timers.Timer timer;
        private Workout workout = new Workout();
        private bool isAtRest = true;
        private double newGoal;
        private double oneRepMax = -1;
        private double minimumPressure = -1; //the pressure inside the ball when not under any load
        //private bool minimumPressureIsSet = false; //bool tells whether the minimum pressure has been set
        //private bool maximumPressureIsSet = false; //bool tells whether the maximum pressure has been set
        private bool okIsClicked = false;
        private BLEViewModel ble;
        private ObservableCollection<Measurement> allWorkoutData;
        //private MeasurementViewModel measurementVM;
        private int currentPressure = -1;
        //public ReadOnlyObservableCollection<int> displayedWorkoutData;

        public GraphPage()
        {            
            InitializeComponent();
            Init();
        }

        public void Init()
        {
            allWorkoutData = new ObservableCollection<Measurement>();
            //measurementVM = new MeasurementViewModel();
            ble = App.ble;
            ble.PropertyChanged += (s, e) =>
            {
                switch (e.PropertyName)
                {
                    case "state":
                        if (ble.isOn)
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                DisplayAlert("Notice", "Bluetooth is on", "OK");
                            });
                        }
                        else
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                DisplayAlert("Notice", "Bluetooth is off", "OK");
                            });
                        }
                        break;
                    case "deviceConnected":
                        if (ble.deviceConnected)
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                DisplayAlert("Notice", "Device Connected!", "OK");
                            });
                        }
                        else
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                DisplayAlert("Notice", "Device Disconnected!", "OK");
                            });
                        }
                        break;
                    case "pressure":
                        currentPressure = ble.pressureVal;
                        //measurementVM.InsertMeasurement(currentPressure);
                        Device.BeginInvokeOnMainThread(() => MessagingCenter.Send<GraphPage, int>(this, "pressureChange", currentPressure));
                        break;
                    default:
                        break;
                }
            };
            // Get current information
            if (App.currentWorkout != null)
            {
                workout = App.currentWorkout;
            }

            //Set up current workout
            if(workout != null /*&& workout.CheckInformation()*/)
            {
                NumSets.Text = setCount.ToString();
                TotalSets.Text = "of " + workout.NumSets;
                NumReps.Text = repCount.ToString();
                TotalReps.Text = "of " + workout.NumReps;
                TimeDisplay.Text = "";
                //newGoal = (Convert.ToDouble(workout.ThresholdPercentage)/100) * App.currentUser.OneRepMax;
                //Goal.Start = newGoal;
                totalReps = Convert.ToInt32(workout.NumReps);
                totalSets = Convert.ToInt32(workout.NumSets);
            }
            else
            {
                DisplayAlert("No Exercise Loaded", "Please choose an exercise to continue.", "Ok");
                //LoadExercise();
            }
        }        

        private async void LoadExercise()
        {
            bool loadExercise = await DisplayAlert("No Exercise Loaded", "Please choose an exercise", "Ok", "Cancel");
            if (loadExercise)
            {
                await Navigation.PushAsync(new ManageExercise());
            }
        }

        private void Start_Exercise(object sender, EventArgs e)
        {
            //if (!App.currentUser.DeviceIsConnected)
            if(!ble.deviceConnected)
            {
                CheckBTConnection();
                return;
            }
            if(oneRepMax == -1) //need to initialize the one rep max
            {
                CalibratePressure();
            }
            //Device.BeginInvokeOnMainThread(() => TimerLabel.Text = "APPLY PRESSURE");
            TimerLabel.Text = "APPLY PRESSURE";

            StartTimer();
            ble.StartUpdates();
        }
        private void Stop_Exercise(object sender, EventArgs e)
        {
            timer.Stop();
            ble.StopUpdates();
            TimerLabel.Text = "PAUSE";
            TimeDisplay.BackgroundColor = Constants.RestColor;
        }
        private void Save_Exercise(object sender, EventArgs e)
        {
            DisplayAlert("Save", "You have saved the exercise.", "Dismiss");
        }

        private void OK_Clicked(object sender, EventArgs e)
        {
            okIsClicked = true;
        }

        private void StartTimer()
        {
            //base.onResume;
            timer = new System.Timers.Timer
            {
                Interval = 1000
            };
            timer.Elapsed += Time_Elapsed;
            timer.Start();

        }

        public void Time_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (TimerLabel.Text.Equals("REST"))
            {
                if (currentPressure <= newGoal)
                {
                    if (second > 10)
                    {

                        second--;
                        Device.BeginInvokeOnMainThread(() => TimeDisplay.Text = Convert.ToString(minute + ":" + second));
                    }

                    else if (second > 0 && second <= 10)
                    {
                        second--;
                        Device.BeginInvokeOnMainThread(() => TimeDisplay.Text = Convert.ToString(minute + ":0" + second));
                    }

                    else if (second == 0)
                    {
                        if (minute > 0)
                        {
                            minute--;
                            second = 59;
                            Device.BeginInvokeOnMainThread(() => TimeDisplay.Text = Convert.ToString(minute + ":" + second));
                        }

                        else
                        {
                            if (setCount <= totalSets)
                            {
                                if (repCount <= totalReps)
                                {
                                    Device.BeginInvokeOnMainThread(() => NumReps.Text = repCount.ToString());
                                    Device.BeginInvokeOnMainThread(() => NumSets.Text = setCount.ToString());

                                    if (isAtRest)
                                    {
                                        Device.BeginInvokeOnMainThread(() => TimerLabel.Text = "APPLY PRESSURE");
                                        Device.BeginInvokeOnMainThread(() => TimeDisplay.BackgroundColor = Constants.BackgroundColor);
                                        //countdown = Convert.ToInt32(workout.HoldDuration);
                                        second = Convert.ToInt32(workout.HoldDuration);
                                        if (second >= 10)
                                        {
                                            Device.BeginInvokeOnMainThread(() => TimeDisplay.Text = Convert.ToString(minute + ":" + second));
                                        }
                                        else
                                        {
                                            Device.BeginInvokeOnMainThread(() => TimeDisplay.Text = Convert.ToString(minute + ":0" + second));
                                        }
                                        repCount++;
                                        isAtRest = false;
                                    }

                                    else
                                    {
                                        Device.BeginInvokeOnMainThread(() => TimerLabel.Text = "REST");
                                        Device.BeginInvokeOnMainThread(() => TimeDisplay.BackgroundColor = Constants.RestColor);
                                        //countdown = Convert.ToInt32(workout.RestDuration);
                                        second = Convert.ToInt32(workout.RestDuration);
                                        if (second >= 10)
                                        {
                                            Device.BeginInvokeOnMainThread(() => TimeDisplay.Text = Convert.ToString(minute + ":" + second));
                                        }
                                        else
                                        {
                                            Device.BeginInvokeOnMainThread(() => TimeDisplay.Text = Convert.ToString(minute + ":0" + second));
                                        }
                                        isAtRest = true;
                                    }
                                }

                                else
                                {
                                    setCount++;
                                    repCount = 0;
                                }
                            }

                            else
                            {
                                Device.BeginInvokeOnMainThread(() => TimerLabel.Text = "COMPLETE");
                                Device.BeginInvokeOnMainThread(() => TimeDisplay.Text = Convert.ToString("0" + minute + ":0" + second));
                                TimeDisplay.BackgroundColor = Constants.CompleteColor;
                                timer.Stop();
                            }
                        }
                    }
                }
            }
            else
            {
                if (currentPressure >= newGoal)
                {
                    if (second > 10)
                    {
                        second--;
                        Device.BeginInvokeOnMainThread(() => TimeDisplay.Text = Convert.ToString(minute + ":" + second));
                    }

                    else if (second > 0 && second <= 10)
                    {
                        second--;
                        Device.BeginInvokeOnMainThread(() => TimeDisplay.Text = Convert.ToString(minute + ":0" + second));
                    }

                    else if (second == 0)
                    {
                        if (minute > 0)
                        {
                            minute--;
                            second = 59;
                            Device.BeginInvokeOnMainThread(() => TimeDisplay.Text = Convert.ToString(minute + ":" + second));
                        }

                        else
                        {
                            if (setCount <= totalSets)
                            {
                                if (repCount < totalReps)
                                {
                                    Device.BeginInvokeOnMainThread(() => NumReps.Text = repCount.ToString());
                                    Device.BeginInvokeOnMainThread(() => NumSets.Text = setCount.ToString());

                                    if (isAtRest)
                                    {
                                        Device.BeginInvokeOnMainThread(() => TimerLabel.Text = "APPLY PRESSURE");
                                        Device.BeginInvokeOnMainThread(() => TimeDisplay.BackgroundColor = Constants.BackgroundColor);
                                        second = Convert.ToInt32(workout.HoldDuration);
                                        if (second >= 10)
                                        {
                                            Device.BeginInvokeOnMainThread(() => TimeDisplay.Text = Convert.ToString(minute + ":" + second));
                                        }
                                        else
                                        {
                                            Device.BeginInvokeOnMainThread(() => TimeDisplay.Text = Convert.ToString(minute + ":0" + second));
                                        }
                                        repCount++;
                                        isAtRest = false;
                                    }

                                    else
                                    {
                                        Device.BeginInvokeOnMainThread(() => TimerLabel.Text = "REST");
                                        Device.BeginInvokeOnMainThread(() => TimeDisplay.BackgroundColor = Constants.RestColor);
                                        //countdown = Convert.ToInt32(workout.RestDuration);
                                        second = Convert.ToInt32(workout.RestDuration);
                                        if (second >= 10)
                                        {
                                            Device.BeginInvokeOnMainThread(() => TimeDisplay.Text = Convert.ToString(minute + ":" + second));
                                        }
                                        else
                                        {
                                            Device.BeginInvokeOnMainThread(() => TimeDisplay.Text = Convert.ToString(minute + ":0" + second));
                                        }
                                        isAtRest = true;
                                    }
                                }

                                else
                                {
                                    setCount++;
                                    repCount = 0;
                                }
                            }

                            else
                            {
                                Device.BeginInvokeOnMainThread(() => TimerLabel.Text = "COMPLETE");
                                Device.BeginInvokeOnMainThread(() => TimeDisplay.Text = Convert.ToString("0" + minute + ":0" + second));
                                TimeDisplay.BackgroundColor = Constants.CompleteColor;
                                timer.Stop();
                            }
                        }
                    }
                }
            }
        }

            //else if (countdown.Equals(0))
            //{
            //    if(setCount <= totalSets)
            //    {                    
            //        if (repCount <= totalReps)
            //        { 
            //            Device.BeginInvokeOnMainThread(() => NumReps.Text = repCount.ToString());
            //            Device.BeginInvokeOnMainThread(() => NumSets.Text = setCount.ToString());
            //            Device.BeginInvokeOnMainThread(() => TimeDisplay.Text = Convert.ToString(countdown));
                    
            //            if (isAtRest)
            //            {
            //                Device.BeginInvokeOnMainThread(() => TimerLabel.Text = "APPLY PRESSURE");
            //                Device.BeginInvokeOnMainThread(() => TimeDisplay.BackgroundColor = Constants.BackgroundColor);
            //                countdown = Convert.ToInt32(workout.HoldDuration);
            //                Device.BeginInvokeOnMainThread(() => TimeDisplay.Text = Convert.ToString(countdown));
            //                isAtRest = false;
            //            }
            //            else
            //            {
            //                Device.BeginInvokeOnMainThread(() => TimerLabel.Text = "REST");
            //                Device.BeginInvokeOnMainThread(() => TimeDisplay.BackgroundColor = Constants.RestColor);
            //                countdown = Convert.ToInt32(workout.RestDuration);
            //                Device.BeginInvokeOnMainThread(() => TimeDisplay.Text = Convert.ToString(countdown));
            //                repCount++;
            //                isAtRest = true;
            //            }
            //        }
            //        else
            //        {
            //            setCount++;
            //            repCount = 1;
            //        }
            //    }
            //    else
            //    {                    
            //        TimerLabel.Text = "COMPLETE";
            //        TimeDisplay.Text = "";
            //        TimeDisplay.BackgroundColor = Constants.CompleteColor;
            //        timer.Stop();
            //    }                
                
                
            //}

            ////If it ever decides to go negative.
            //else
            //{
            //    TimeDisplay.Text = "" + Convert.ToString(countdown);
            //    timer.Stop();
            //    TimerLabel.Text = "REST";
            //    TimeDisplay.BackgroundColor = Constants.RestColor;
            //}
        

        public async void CheckBTConnection()
        {
            //if (!App.currentUser.DeviceIsConnected)
            if (!ble.deviceConnected)
            {
                bool loadDevice = await DisplayAlert("No Connected Device", "Please connect an exercise device ", "Ok", "Cancel");
                if (loadDevice)
                {
                    await Navigation.PushAsync(new BLEView());
                }
            }
        }

        private async void CalibratePressure()
        {
            await DisplayAlert("Calibration", "First we need to calibrate for the pressure in the ball", "Ok");
            btnOK.IsVisible = true;
            ble.StartUpdates();
            TimerLabel.Text = "Do not apply any pressure to ball. Press 'OK' when ready.";

            while(currentPressure == -1)
            {
                // wait for a pressure update
            }
            minimumPressure = currentPressure; //set the minimum pressure
            while(!okIsClicked)//loop and update the minimum pressure until user clicks OK
            {
                if(currentPressure < minimumPressure)
                {
                    minimumPressure = currentPressure;
                }
            }
            okIsClicked = false;
            TimerLabel.Text = "Okay, now squeeze the ball between chin and chest as hard as you possibly can. Press 'OK' when done";
            while (!okIsClicked)
            {
                if(currentPressure > oneRepMax)
                {
                    oneRepMax = currentPressure;
                }
            }
            okIsClicked = false;
            //LineChart.yAxis.
            newGoal = (Convert.ToDouble(workout.ThresholdPercentage) / 100) * oneRepMax;
            Goal.Start = newGoal;
            Goal.IsVisible = true;
            btnOK.IsVisible = false;
            await DisplayAlert("Calibration", "Great. All done with calibration. Press 'OK' to begin the workout", "Ok");
        }


    }
}