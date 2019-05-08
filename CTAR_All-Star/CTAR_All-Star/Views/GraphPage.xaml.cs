using System;

using Xamarin.Forms;
using CTAR_All_Star.Models;
using CTAR_All_Star.Database;
using System.Timers;
using System.Collections.ObjectModel;

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
        private double? oneRepMax = -1;
        private double? minimumPressure = 1024; //the pressure inside the ball when not under any load
        private bool minimumPressureIsSet = false; //bool tells whether the minimum pressure has been set
        private bool oneRepMaxIsSet = false; //bool tells whether the maximum pressure has been set
        //private bool okIsClicked = false;
        private BLEViewModel ble;
        private ObservableCollection<Measurement> allWorkoutData;
        //private MeasurementViewModel measurementVM;
        private double? currentPressure = null;
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

                        if (!minimumPressureIsSet)
                        {
                            if(currentPressure != null || currentPressure < minimumPressure)
                            {
                                minimumPressure = currentPressure;
                            }
                        }
                        else if (!oneRepMaxIsSet)
                        {
                            if (currentPressure > oneRepMax)
                            {
                                oneRepMax = currentPressure;
                            }
                        }

                        //measurementVM.InsertMeasurement(currentPressure);
                        Device.BeginInvokeOnMainThread(() => MessagingCenter.Send<GraphPage, int>(this, "pressureChange", (int)currentPressure));

                        // Get current date and time
                        DateTime d = DateTime.Now;
                        DateTime dt = DateTime.Parse(d.ToString());

                        Measurement measurement = new Measurement()
                        {
                            UserName = App.currentUser.Username,
                            DocID = String.Empty,
                            SessionNumber = App.currentUser.Session.ToString(),
                            TimeStamp = d,
                            Pressure = currentPressure,
                            DisplayTime = dt.ToString("HH:mm:ss"),
                            DisplayDate = dt.ToString("MM/dd/yy"),
                            OneRepMax = oneRepMax
                        };

                        allWorkoutData.Add(measurement);
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
            if (workout != null /*&& workout.CheckInformation()*/)
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
                //DisplayAlert("No Exercise Loaded", "Please choose an exercise to continue.", "Ok");
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
            //if(App.currentWorkout == null)
            //{
            //    LoadExercise();
            //}

            //if (!App.currentUser.DeviceIsConnected)
            if (!ble.deviceConnected)
            {
                CheckBTConnection();
                return;
            }
            if (!minimumPressureIsSet && !oneRepMaxIsSet) //need to initialize the one rep max
            {
                getMinimumPressure();
            }
            else
            {
                //Device.BeginInvokeOnMainThread(() => TimerLabel.Text = "APPLY PRESSURE");
                TimerLabel.Text = "APPLY PRESSURE";

                StartTimer();
                ble.StartUpdates();
            }
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
            timer.Stop();
            ble.StopUpdates();
            DisplayAlert("Save", "You have saved the exercise.", "Dismiss");
            DatabaseHelper dbHelper = new DatabaseHelper();
            dbHelper.addDataList(allWorkoutData);
            App.currentUser.Session++;
            App.currentWorkout = null;
        }

        private async void OK_Clicked(object sender, EventArgs e)
        {
            if (!minimumPressureIsSet)
            {
                minimumPressureIsSet = true;
                getMaximumPressure();
            }
            else if (!oneRepMaxIsSet)
            {
                oneRepMaxIsSet = true;
            }
            else //both max and minimum have been set
            {
                newGoal = ((Convert.ToDouble(workout.ThresholdPercentage) / 100) * ((Double)oneRepMax - (Double)minimumPressure)) + (Double)minimumPressure;
                Goal.Start = newGoal;
                Goal.IsVisible = true;
                yAxis.Minimum = minimumPressure - (minimumPressure * 0.10);
                yAxis.Maximum = oneRepMax + (oneRepMax * 0.10);

                btnOK.IsVisible = false;
                startBtn.IsVisible = true;
                //pauseBtn.IsVisible = true;
                doneBtn.IsVisible = true;

                //Device.BeginInvokeOnMainThread(() =>
                //{
                await DisplayAlert("Calibration", "Great. All done with calibration. Press 'OK' to begin the workout", "Ok");
                //});
                TimerLabel.Text = "APPLY PRESSURE";

                StartTimer();
                //ble.StartUpdates();
            }
            //okIsClicked = true;
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

        private async void getMinimumPressure()
        {
            //Device.BeginInvokeOnMainThread(async() =>
            //{
            await DisplayAlert("Calibration", "First we need to calibrate for the pressure in the ball", "Ok");
            //});

            ble.StartUpdates();
            btnOK.IsVisible = true;
            startBtn.IsVisible = false;
            //pauseBtn.IsVisible = false;
            doneBtn.IsVisible = false;
            TimerLabel.Text = "No squeezing... Press OK";

            ////while(currentPressure == -1)
            ////{
            ////    // wait for a pressure update
            ////}
            //minimumPressure = currentPressure; //set the minimum pressure
            //while (!okIsClicked)//loop and update the minimum pressure until user clicks OK
            //{
            //    if (currentPressure < minimumPressure)
            //    {
            //        minimumPressure = currentPressure;
            //    }
            //}
            //okIsClicked = false;


        }
        private async void getMaximumPressure()
        {
            TimerLabel.Text = "Okay, SQUEEZE!!!";
        }

        //protected override void OnAppearing()
        //{
        //    base.OnAppearing();
        //    if(App.currentWorkout == null)
        //    {
        //        LoadExercise();
        //    }
        //}
    }
}