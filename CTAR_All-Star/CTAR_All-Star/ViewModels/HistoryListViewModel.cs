using CTAR_All_Star.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using SQLite;
using Xamarin.Forms;
using CTAR_All_Star.Database;

namespace CTAR_All_Star.ViewModels
{
    public class HistoryListViewModel : HistoryViewModel
    {
        public ObservableCollection<Measurement> Dates { get; set; }

        public HistoryListViewModel()
        {
            Dates = new ObservableCollection<Measurement>();

            // Connect to database, pull data and store it in the list
            using (SQLiteConnection conn = new SQLiteConnection(App.DB_PATH))
            {
                // Gather all history
                var table = conn.Table<Measurement>();
                table = table.OrderByDescending(x => x.DisplayTime);

                //For Patients
                if (App.currentUser.userType.Equals("Patient"))
                {
                    foreach (var w in table)
                    {
                        // Display only history of current patient
                        if (w.UserName.Equals(App.currentUser.Username))
                        {
                            Dates.Add(w);
                        }
                    }
                }
                //For Doctors
                else
                {
                    foreach (var w in table)
                    {
                        //Display all workouts created by current doctor
                        if (w.UserName.Equals(App.currentUser.Username))
                        {
                            Dates.Add(w);
                        }
                    }                                               
                }
            }

            // Listen for signal to update data for table
            MessagingCenter.Subscribe<DatabaseHelper>(this, "historyChange", (sender) =>
            {
                Dates.Clear();

                // Refresh list to display
                using (SQLiteConnection conn = new SQLiteConnection(App.DB_PATH))
                {
                    // Gather all history
                    var table = conn.Table<Measurement>();
                    table = table.OrderByDescending(x => x.DisplayTime);

                    //For Patients
                    if (App.currentUser.userType.Equals("Patient"))
                    {
                        foreach (var w in table)
                        {
                            // Display only workouts assigned to current patient
                            if (w.UserName.Equals(App.currentUser.Username))
                            {
                                Dates.Add(w);
                            }
                        }
                    }
                    //For Doctors
                    else
                    {
                        foreach (var w in table)
                        {
                            //Display all workouts created by current doctor
                            if (w.UserName.Equals(App.currentUser.Username))
                            {
                                Dates.Add(w);
                            }
                        }
                    }
                }
            });
        }

        public void ShowAllData()
        {
            Dates.Clear();

            // Refresh list to display
            using (SQLiteConnection conn = new SQLiteConnection(App.DB_PATH))
            {
                // Gather all history
                var table = conn.Table<Measurement>();
                table = table.OrderByDescending(x => x.DisplayTime);
                foreach (var w in table)
                {
                    Dates.Add(w);
                }
            }
        }

        public void ShowDataByDate(String date)
        {
            Dates.Clear();

            // Refresh list to display
            using (SQLiteConnection conn = new SQLiteConnection(App.DB_PATH))
            {
                // Gather all history
                var table = conn.Table<Measurement>();
                table = table.OrderByDescending(x => x.DisplayTime);
                foreach (var w in table)
                {
                    if(w.DisplayDate == date && w.UserName.Equals(App.currentUser.Username))
                    {
                        Dates.Add(w);
                    }                    
                }
            }
        }
    }
}
