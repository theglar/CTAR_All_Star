using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;
using CTAR_All_Star;
using SQLite;
using Xamarin.Forms.Xaml;
using System.ComponentModel;
using CTAR_All_Star.Navigation;
using CTAR_All_Star.Database;
using System.Diagnostics;

namespace CTAR_All_Star.Models
{
    public class ViewModel
    {
        public ObservableCollection<Measurement> Data { get; set; }

        public ViewModel()       
        {     
            Data = new ObservableCollection<Measurement>();

            // Get current date and time
            DateTime d = DateTime.Now;
            DateTime dt = DateTime.Parse(d.ToString());

            Measurement measurement = new Measurement()
            {
                UserName = "Tester 1",
                SessionNumber = "1",
                TimeStamp = d,
                Pressure = 500,
                Duration = "1",
                DisplayTime = dt.ToString("HH:mm:ss")
            };

            // Initialize list with zeros
            for (int i=0; i<10; i++)
            {
                Data.Add(measurement);
            }
            // Connect to database, pull data and store it in the list
            //using (SQLiteConnection conn = new SQLiteConnection(App.DB_PATH))
            //{
            //    // Display the most recent measurements
            //    var table = conn.Table<Measurement>();
            //    table = table.OrderByDescending(x => x.Id).Take(10);
            //    table = table.OrderBy(x => x.Id);
            //    foreach (var m in table)
            //    {
            //        Data.Add(m);
            //    }
            //}

            // Listen for signal to update data for graph
            MessagingCenter.Subscribe<DatabaseHelper, Measurement>(this, "databaseChange", (sender, newMeasurement) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Data.RemoveAt(0);
                    Data.Add(newMeasurement);
                });
                
                foreach (Measurement data in Data)
                {
                    Debug.Print(data.Pressure.ToString());
                }
                

                //Data.Clear();

                //// Connect to database, pull data and store it in the list
                //using (SQLiteConnection conn = new SQLiteConnection(App.DB_PATH))
                //{
                //    // Display the most recent measurements
                //    var table = conn.Table<Measurement>();
                //    table = table.OrderByDescending(x => x.Id).Take(10);
                //    table = table.OrderBy(x => x.Id);
                //    foreach (var m in table)
                //    {
                //        Data.Add(m);
                //    }
                //}
            });
        } 
    }
}
