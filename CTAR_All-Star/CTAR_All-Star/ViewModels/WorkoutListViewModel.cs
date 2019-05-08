using CTAR_All_Star.Models;
using System.Collections.ObjectModel;
using SQLite;
using Xamarin.Forms;
using CTAR_All_Star.Database;

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
                // Gather all the workouts
                var table = conn.Table<Workout>();
                table = table.OrderByDescending(x => x.WorkID);

                //For Patients
                if (App.currentUser.userType.Equals("Patient"))
                {
                    foreach (var w in table)
                    {
                        // Display only workouts assigned to current patient
                        if (w.PatientEmrNumber.Equals(App.currentUser.Username))
                        {
                            Workouts.Add(w);
                        }
                    }
                }
                //For Doctors
                else
                {
                    foreach (var w in table)
                    {
                        //Display all workouts created by current doctor
                        if (w.DoctorID.Equals(App.currentUser.Username))
                        {
                            Workouts.Add(w);
                        }
                    }                                               
                }
            }

            // Listen for signal to update data for table
            MessagingCenter.Subscribe<DatabaseHelper>(this, "workoutChange", (sender) =>
            {
                Workouts.Clear();

                // Refresh list to display
                using (SQLiteConnection conn = new SQLiteConnection(App.DB_PATH))
                {
                    // Gather all the workouts
                    var table = conn.Table<Workout>();
                    table = table.OrderByDescending(x => x.WorkID);

                    //For Patients
                    if (App.currentUser.userType.Equals("Patient"))
                    {
                        foreach (var w in table)
                        {
                            // Display only workouts assigned to current patient
                            if (w.PatientEmrNumber.Equals(App.currentUser.Username))
                            {
                                Workouts.Add(w);
                            }
                        }
                    }
                    //For Doctors
                    else
                    {
                        foreach (var w in table)
                        {
                            //Display all workouts created by current doctor
                            if (w.DoctorID.Equals(App.currentUser.Username))
                            {
                                Workouts.Add(w);
                            }
                        }
                    }
                }
            });
        }

        public void ShowAllData()
        {
            Workouts.Clear();

            // Refresh list to display
            using (SQLiteConnection conn = new SQLiteConnection(App.DB_PATH))
            {
                // Gather all the workouts
                var table = conn.Table<Workout>();
                table = table.OrderByDescending(x => x.WorkID);
                foreach (var w in table)
                {
                    Workouts.Add(w);
                }
            }
        }
    }
}
