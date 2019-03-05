using CTAR_All_Star.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using SQLite;

namespace CTAR_All_Star.ViewModels
{
    public class WorkoutListViewModel : WorkoutViewModel
    {
        public ObservableCollection<Workout> Workouts { get; set; }

        public WorkoutListViewModel()
        {
            Workouts = new ObservableCollection<Workout>();

            // Connect to database, pull data and store it in the list
            using (SQLiteConnection conn = new SQLiteConnection(App.DB_PATH))
            {
                // Display the most recent workouts
                var table = conn.Table<Workout>();
                table = table.OrderByDescending(x => x.WorkID);
                foreach (var m in table)
                {
                    // Only workouts assigned to current user
                    if(m.PatientEmrNumber == App.currentUser.Username)
                    {
                        Workouts.Add(m);
                    }                    
                }
            }
        }
    }
}
