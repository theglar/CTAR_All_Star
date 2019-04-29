using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using CTAR_All_Star.Database;
using CTAR_All_Star.Models;

namespace CTAR_All_Star.ViewModels
{
    public class MeasurementViewModel
    {
        public ObservableCollection<GraphMeasurement> Data { get; private set; }

        public MeasurementViewModel()       
        {     
            Data = new ObservableCollection<GraphMeasurement>();

            // Get current date and time
            DateTime d = DateTime.Now;

            GraphMeasurement measurement = new GraphMeasurement()
            {
                Pressure = null,
                Time = String.Empty
            };

            // Initialize list
            for (int i=0; i<500; i++)
            {
                Data.Add(measurement);
            }

            //// Listen for signal to update data for graph
            MessagingCenter.Subscribe<GraphPage, int>(this, "pressureChange", (sender, newPressure) =>
            {
                InsertMeasurement(newPressure);
                //Device.BeginInvokeOnMainThread(() =>
                //{
                //    Data.RemoveAt(0);
                //    Data.Insert(400, newMeasurement);
                //    App.currentMeasurement = newMeasurement;
                //});
            });
        } 

        public void InsertMeasurement(int newMeasurementVal)
        {
            GraphMeasurement newMeasurement = new GraphMeasurement();
            newMeasurement.Pressure = newMeasurementVal;
            newMeasurement.Time = DateTime.Now.ToString("HH:mm:ss");

            Data.RemoveAt(0);
            Data.Insert(400, newMeasurement);
        }
    }
}
