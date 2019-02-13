using System;
using SQLite;

using Xamarin.Forms;
using CTAR_All_Star.Models;
using System.Threading;
using Syncfusion.SfChart.XForms;
using CTAR_All_Star.Database;

namespace CTAR_All_Star
{
    public partial class GraphPage : ContentPage
    {

        public GraphPage()
        {            
            InitializeComponent();
        }

        private async void Start_ExerciseAsync(object sender, EventArgs e)
        {
            await DisplayAlert("Start", "Start exercise?", "Yes");

            //Not working...
            //Device.StartTimer(TimeSpan.FromSeconds(5), () =>
            //{
            //    // Start accepting bluetooth data?
            //    return false; //True repeats, False stops
            //});
            await DisplayAlert("Complete", "Congratulation! You finished the exercise!", "Dismiss");
        }
        private void Stop_Exercise(object sender, EventArgs e)
        {
            DisplayAlert("Stop", "You have stopped the exercise.", "Dismiss");
        }
        private void Save_Exercise(object sender, EventArgs e)
        {
            DisplayAlert("Save", "You have saved the exercise.", "Dismiss");
        }

        //For testing
        private void Simulation(object sender, EventArgs e)
        {
            DatabaseHelper dbHelper = new DatabaseHelper();

            // Initialize a starting point
            Double pressure = 0;

            //Loop 100 times - REMOVED THE LOOP FOR TESTING
            for (int i = 0; i < 1; i++)
            {
                // Get current date and time
                DateTime d = DateTime.Now;
                DateTime dt = DateTime.Parse(d.ToString());

                pressure = Math.Sin(Convert.ToDouble(d.Millisecond) / 10) * 100 + 500;

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
    }
}