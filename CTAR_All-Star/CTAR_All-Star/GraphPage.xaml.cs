using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microcharts.Forms;
using SkiaSharp;
using Microcharts;
using CTAR_All_Star.Models;

using Syncfusion.SfChart.XForms;

namespace CTAR_All_Star
{
    public partial class GraphPage : ContentPage
    {        
        public GraphPage()
        {
            InitializeComponent();            
        }
       
        private void Start_Simulation(object sender, EventArgs e)
        {           
            // Initialize a starting point
            Double pressure = 0;

            //Loop 100 times
            for (int i = 0; i < 100; i++)
            {
                //Create and add a measurement to the database
                // Get current date and time
                DateTime d = DateTime.Now;
                DateTime dt = DateTime.Parse(d.ToString());

                //Top threshold - start going down
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

                using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
                {
                    conn.CreateTable<Measurement>();
                    conn.Insert(measurement);
                }

                //Wait 0.25 seconds
                //Thread.Sleep(250);

                //Refresh page
                Navigation.PushAsync(new GraphPage());
            }
        }

        private void Signin_Activated(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SigninPage());
        }
        private void History_Activated(object sender, EventArgs e)
        {
            Navigation.PushAsync(new HistoryPage());
        }
        private void Graph_Activated(object sender, EventArgs e)
        {
            Navigation.PushAsync(new GraphPage());
        }
        private void Setup_Activated(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SetupPage());
        }
        private void Measure_Activated(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CreatePage());
        }
        private void Remove_Activated(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RemovePage());
        }
    }
}