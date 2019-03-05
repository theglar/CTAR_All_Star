﻿using System;
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
        private int countdown = 0;
        private int repCount = 1;
        private int setCount = 1;
        private int totalReps;
        private int totalSets;
        System.Timers.Timer timer;
        private Workout workout = new Workout();
        private bool isAtRest = true;

        public GraphPage()
        {            
            InitializeComponent();
            Init();
        }

        public void Init()
        {
            workout = App.currentWorkout;
            //Set up current workout
            if(workout != null)
            {
                NumSets.Text = setCount.ToString();
                TotalSets.Text = "of " + workout.NumSets;
                NumReps.Text = repCount.ToString();
                TotalReps.Text = "of " + workout.NumReps;
                TimeDisplay.Text = "";
                double newGoal = (Convert.ToDouble(workout.ThresholdPercentage)/100) * App.currentUser.OneRepMax;
                Goal.Start = newGoal;
                totalReps = Convert.ToInt32(workout.NumReps);
                totalSets = Convert.ToInt32(workout.NumSets);
            }
            
        }

        private void Start_Exercise(object sender, EventArgs e)
        {
            StartTimer();
            
            //DatabaseHelper dbHelper = new DatabaseHelper();

            //// Initialize a starting point
            //Double pressure = 0;

            ////Loop 100 times - REMOVED THE LOOP FOR TESTING
            //for (int i = 0; i < 1; i++)
            //{
            //    // Get current date and time
            //    DateTime d = DateTime.Now;
            //    DateTime dt = DateTime.Parse(d.ToString());
                
            //    pressure = Math.Sin(Convert.ToDouble(d.Millisecond)/10)*100+500;

            //    Measurement measurement = new Measurement()
            //    {
            //        UserName = "Tester 1",
            //        SessionNumber = "1",
            //        TimeStamp = d,
            //        Pressure = pressure,
            //        Duration = "1",
            //        DisplayTime = dt.ToString("HH:mm:ss")
            //    };

            //    dbHelper.addData(measurement);
            //}
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
            

            if (countdown > 0)
            {
                Device.BeginInvokeOnMainThread(() => TimeDisplay.Text = Convert.ToString(countdown));
                countdown--;
            }

            else if (countdown.Equals(0))
            {
                if(setCount <= totalSets)
                {                    
                    if (repCount <= totalReps)
                    { 
                        Device.BeginInvokeOnMainThread(() => NumReps.Text = repCount.ToString());
                        Device.BeginInvokeOnMainThread(() => NumSets.Text = setCount.ToString());
                        Device.BeginInvokeOnMainThread(() => TimeDisplay.Text = Convert.ToString(countdown));
                    
                        if (isAtRest)
                        {
                            Device.BeginInvokeOnMainThread(() => TimerLabel.Text = "APPLY PRESSURE");
                            Device.BeginInvokeOnMainThread(() => TimeDisplay.BackgroundColor = Constants.BackgroundColor);
                            countdown = Convert.ToInt32(workout.HoldDuration);
                            Device.BeginInvokeOnMainThread(() => TimeDisplay.Text = Convert.ToString(countdown));
                            isAtRest = false;
                        }
                        else
                        {
                            Device.BeginInvokeOnMainThread(() => TimerLabel.Text = "REST");
                            Device.BeginInvokeOnMainThread(() => TimeDisplay.BackgroundColor = Constants.RestColor);
                            countdown = Convert.ToInt32(workout.RestDuration);
                            Device.BeginInvokeOnMainThread(() => TimeDisplay.Text = Convert.ToString(countdown));
                            repCount++;
                            isAtRest = true;
                        }
                    }
                    else
                    {
                        setCount++;
                        repCount = 1;
                    }
                }
                else
                {                    
                    TimerLabel.Text = "COMPLETE";
                    TimeDisplay.Text = "";
                    TimeDisplay.BackgroundColor = Constants.CompleteColor;
                    timer.Stop();
                }                
                
                
            }

            //If it ever decides to go negative.
            else
            {
                TimeDisplay.Text = "" + Convert.ToString(countdown);
                timer.Stop();
                TimerLabel.Text = "REST";
                TimeDisplay.BackgroundColor = Constants.RestColor;
            }
        }
    }
}