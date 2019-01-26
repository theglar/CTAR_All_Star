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

namespace CTAR_All_Star.Models
{
    public class ViewModel
    {
        public ObservableCollection<Measurement> Data { get; set; }

        public ViewModel()       
        {     
            Data = new ObservableCollection<Measurement>();

            // Connect to database, pull data and store it in the list
            using (SQLiteConnection conn = new SQLiteConnection(App.DB_PATH))
            {
                // Display the most recent measurements
                var table = conn.Table<Measurement>();
                table = table.OrderByDescending(x => x.Id).Take(10);
                table = table.OrderBy(x => x.Id);
                foreach (var m in table)
                {
                    Data.Add(m);
                }
            }

            // Listen for signal to update data for graph
            MessagingCenter.Subscribe<GraphPage>(this, "newMeasurement", (sender) =>
            {
                Data.Clear();

                // Connect to database, pull data and store it in the list
                using (SQLiteConnection conn = new SQLiteConnection(App.DB_PATH))
                {
                    // Display the most recent measurements
                    var table = conn.Table<Measurement>();
                    table = table.OrderByDescending(x => x.Id).Take(10);
                    table = table.OrderBy(x => x.Id);
                    foreach (var m in table)
                    {
                        Data.Add(m);
                    }
                }
            });
        }        
    }
}
