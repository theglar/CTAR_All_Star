using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using CTAR_All_Star.Database;
using CTAR_All_Star.Models;

namespace CTAR_All_Star.ViewModels
{
    public class MeasurementViewModel
    {
        public ObservableCollection<Measurement> Data { get; set; }

        public MeasurementViewModel()       
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
                Pressure = null,
                Duration = "1",
                DisplayTime = ""
            };

            // Initialize list with zeros and nulls for spacing
            for (int i=0; i<500; i++)
            {
                Data.Add(measurement);
            }

            // Listen for signal to update data for graph
            MessagingCenter.Subscribe<DatabaseHelper, Measurement>(this, "databaseChange", (sender, newMeasurement) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Data.RemoveAt(0);
                    Data.Insert(400, newMeasurement);
                });
            });
        } 
    }
}
