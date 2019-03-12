using System;
using SQLite;

using Xamarin.Forms;
using CTAR_All_Star.Models;
using Syncfusion.SfChart.XForms;
using CTAR_All_Star.Database;
using System.Timers;
using System.Threading;

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

        public GraphPage()
        {            
            InitializeComponent();
            Init();
        }

        public void Init()
        {
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
                newGoal = (Convert.ToDouble(workout.ThresholdPercentage)/100) * App.currentUser.OneRepMax;
                Goal.Start = newGoal;
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
                Navigation.PushAsync(new ManageExercise());
            }
        }

        private void Start_Exercise(object sender, EventArgs e)
        {
            if (!App.currentUser.DeviceIsConnected)
            {
                CheckBTConnection();
                return;
            }
            Device.BeginInvokeOnMainThread(() => TimerLabel.Text = "APPLY PRESSURE");

            StartTimer();           
          
        }
        private void Stop_Exercise(object sender, EventArgs e)
        {
            timer.Stop();
            TimerLabel.Text = "PAUSE";
            TimeDisplay.BackgroundColor = Constants.RestColor;
        }
        private void Save_Exercise(object sender, EventArgs e)
        {
            DisplayAlert("Save", "You have saved the exercise.", "Dismiss");
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
                if (App.currentMeasurement.Pressure <= newGoal)
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
                if (App.currentMeasurement.Pressure >= newGoal)
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
            if (!App.currentUser.DeviceIsConnected)
            {
                bool loadDevice = await DisplayAlert("No Connected Device", "Please connect an exercise device ", "Ok", "Cancel");
                if (loadDevice)
                {
                    Navigation.PushAsync(new MainPage());
                }
            }
        }
    }
}