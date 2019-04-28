using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using CTAR_All_Star.Database;
using CTAR_All_Star.Models;
using System.Collections.Generic;

namespace CTAR_All_Star.ViewModels
{
    public class HistoryGraphViewModel
    {
        public ObservableCollection<Measurement> Data { get; set; }

        public HistoryGraphViewModel()       
        {     
            Data = new ObservableCollection<Measurement>();

            //// Get current date and time
            //DateTime d = DateTime.Now;

            //Measurement measurement = new Measurement()
            //{
            //    UserName = "Tester 1",
            //    DocID = String.Empty,
            //    SessionNumber = "1",
            //    TimeStamp = d,
            //    Pressure = null,
            //    DisplayTime = String.Empty,
            //    DisplayDate = String.Empty,
            //    OneRepMax = null
            //};

            //// Initialize list
            // Data.Add(measurement);
            

            // Listen for signal to update data for graph
            MessagingCenter.Subscribe<HistoryGraph, List<Measurement>>(this, "databaseChange", (sender, newData) =>
            {
                Data.Clear();
                if (newData != null)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        foreach (var m in newData)
                        {
                            Data.Add(m);
                        }
                    });
                }
                
            });
        } 
    }
}
