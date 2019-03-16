using CTAR_All_Star.Database;
using CTAR_All_Star.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace CTAR_All_Star.ViewModels
{
    public class PatientListViewModel : PatientViewModel
    {
        public ObservableCollection<Patient> Patients { get; set; }

        public PatientListViewModel()
        {
            Patients = new ObservableCollection<Patient>();

            // Connect to database, pull data and store it in the list
            using (SQLiteConnection conn = new SQLiteConnection(App.DB_PATH))
            {
                // Display the current doctor's patients
                var table = conn.Table<Patient>();
                table = table.OrderByDescending(x => x.PatientId);
                foreach (var m in table)
                {
                    if(m.DoctorName == App.currentUser.Username)
                    {
                        Patients.Add(m);
                    }                    
                }
            }

            // Listen for signal to update data for graph
            MessagingCenter.Subscribe<DatabaseHelper>(this, "patientChange", (sender) =>
            {
                Patients.Clear();

                // Connect to database, pull data and store it in the list
                using (SQLiteConnection conn = new SQLiteConnection(App.DB_PATH))
                {
                    // Display the most recent measurements
                    var table = conn.Table<Patient>();
                    table = table.OrderByDescending(x => x.PatientId);
                    foreach (var m in table)
                    {
                        if (m.DoctorName == App.currentUser.Username)
                        {
                            Patients.Add(m);
                        }
                    }
                }
            });
        }
    }
}
