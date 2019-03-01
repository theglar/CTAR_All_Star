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
        private int countdown = 10;
        System.Timers.Timer timer;

        public GraphPage()
        {            
            InitializeComponent();
        }

        private void Start_Exercise(object sender, EventArgs e)
        {
            StartTimer();
            
            DatabaseHelper dbHelper = new DatabaseHelper();

            // Initialize a starting point
            Double pressure = 0;

            //Loop 100 times - REMOVED THE LOOP FOR TESTING
            for (int i = 0; i < 1; i++)
            {
                // Get current date and time
                DateTime d = DateTime.Now;
                DateTime dt = DateTime.Parse(d.ToString());
                
                pressure = Math.Sin(Convert.ToDouble(d.Millisecond)/10)*100+500;

                Measurement measurement = new Measurement()
                {
                    UserName = "Tester 1",
                    SessionNumber = "1",
                    TimeStamp = d,
                    Pressure = pressure,
                    Duration = "1",
                    DisplayTime = dt.ToString("HH:mm:ss")
                };

                dbHelper.addData(measurement);
            }
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
            TimerLabel.Text = "APPLY PRESSURE";
            TimeDisplay.BackgroundColor = Constants.BackgroundColor;

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
                countdown--;
                Device.BeginInvokeOnMainThread(() => TimeDisplay.Text = Convert.ToString(countdown)); 

            }

            else if (countdown == 0)
            {
                countdown = 10;
                Device.BeginInvokeOnMainThread(() => TimeDisplay.Text = Convert.ToString(countdown));
                timer.Stop();
                TimerLabel.Text = "REST";
                TimeDisplay.BackgroundColor = Constants.RestColor;
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