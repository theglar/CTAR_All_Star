using System;
using SQLite;

using Xamarin.Forms;
using CTAR_All_Star.Models;
using System.Threading;
using Syncfusion.SfChart.XForms;

namespace CTAR_All_Star
{
    public partial class GraphPage : ContentPage
    {

        public GraphPage()
        {            
            InitializeComponent();
        }

        private void Start_Exercise(object sender, EventArgs e)
        {           
            // Initialize a starting point
            Double pressure = 0;

            //Loop 100 times - REMOVED THE LOOP FOR TESTING
            for (int i = 0; i < 1; i++)
            {
                //Create and add a measurement to the database
                // Get current date and time
                DateTime d = DateTime.Now;
                DateTime dt = DateTime.Parse(d.ToString());
                
                pressure = Math.Sin(Convert.ToDouble(d.Millisecond)/10);

                Measurement measurement = new Measurement()
                {
                    UserName = "Tester 1",
                    SessionNumber = "1",
                    TimeStamp = d,
                    Pressure = pressure,
                    Duration = "1",
                    DisplayTime = dt.ToString("HH:mm:ss")
                };

                using (SQLiteConnection conn = new SQLiteConnection(App.DB_PATH))
                {
                    conn.CreateTable<Measurement>();
                    conn.Insert(measurement);
                }                               

                // Notify ViewModel of changes
                MessagingCenter.Send<GraphPage>(this, "newMeasurement");                

                //Wait 0.25 seconds
                //Thread.Sleep(250);

                //Refresh page
                //Navigation.PushAsync(new GraphPage());
            }
        }
        private void Stop_Exercise(object sender, EventArgs e)
        {
            DisplayAlert("Stop", "You have stopped the exercise.", "Dismiss");
        }
        private void Save_Exercise(object sender, EventArgs e)
        {
            DisplayAlert("Save", "You have saved the exercise.", "Dismiss");
        }
    }
}