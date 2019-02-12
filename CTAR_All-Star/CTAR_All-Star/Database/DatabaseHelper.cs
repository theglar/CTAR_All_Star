using CTAR_All_Star.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CTAR_All_Star.Database
{
    class DatabaseHelper
    {
        public DatabaseHelper()
        {

        }

        public void initializeAllTables()
        {
            initializeMeasurementTable();
            initializePatientTable();
            initializeUsersTable();
            initializeWorkoutTable();
        }

        /*******MEASUREMENTS*********/

        public void initializeMeasurementTable()
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<Measurement>();
            }
        }

        public void addData(Measurement measurement)
        {
            // Add to database
            using (SQLiteConnection conn = new SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<Measurement>();
                conn.Insert(measurement);
            }

            //Notify ViewModel of changes
            Device.BeginInvokeOnMainThread(() => MessagingCenter.Send<DatabaseHelper,Measurement>(this, "databaseChange", measurement));
        }

        public void removeData(int measurementId)
        {
            // Delete from database
            using (SQLiteConnection conn = new SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<Measurement>();
                conn.Delete(measurementId);
            }

            // Notify ViewModel of changes
            Device.BeginInvokeOnMainThread(() => MessagingCenter.Send<DatabaseHelper>(this, "databaseChange"));
        }

        public void clearDatabase()
        {
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                conn.DeleteAll<Measurement>();
            }
        }

        /*******PATIENTS*********/

        public void initializePatientTable()
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<Patient>();
            }
        }

        public void addPatient(Patient patient)
        {
            // Add to database
            using (SQLiteConnection conn = new SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<Patient>();
                conn.Insert(patient);
            }

            //Notify ViewModel of changes
            Device.BeginInvokeOnMainThread(() => MessagingCenter.Send<DatabaseHelper>(this, "patientChange"));
        }        

        public void removePatient(Patient patient)
        {
            // Delete from database
            using (SQLiteConnection conn = new SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<Patient>();
                conn.Delete(patient);
            }

            // Notify ViewModel of changes
            Device.BeginInvokeOnMainThread(() => MessagingCenter.Send<DatabaseHelper>(this, "patientChange"));
        }

        public void clearPatients()
        {
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                conn.DeleteAll<Patient>();
            }

            // Notify ViewModel of changes
            Device.BeginInvokeOnMainThread(() => MessagingCenter.Send<DatabaseHelper>(this, "patientChange"));
        }

        /*******WORKOUTS*********/

        public void initializeWorkoutTable()
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<Workout>();
            }
        }

        public void addWorkout(Workout workout)
        {
            // Add to database
            using (SQLiteConnection conn = new SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<Workout>();
                conn.Insert(workout);
            }

            //Notify ViewModel of changes
            //Device.BeginInvokeOnMainThread(() => MessagingCenter.Send<DatabaseHelper>(this, "databaseChange"));
        }

        public void removeWorkout(int workoutId)
        {
            // Delete from database
            using (SQLiteConnection conn = new SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<Workout>();
                conn.Delete(workoutId);
            }

            // Notify ViewModel of changes
            //Device.BeginInvokeOnMainThread(() => MessagingCenter.Send<DatabaseHelper>(this, "databaseChange"));
        }

        public void clearWorkouts()
        {
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                conn.DeleteAll<Workout>();
            }
        }

        /*******USERS*********/

        public void initializeUsersTable()
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<User>();
            }
        }

        public void addUser(User user)
        {
            // Add to database
            using (SQLiteConnection conn = new SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<User>();
                conn.Insert(user);
            }

            //Notify ViewModel of changes
            //MessagingCenter.Send<DatabaseHelper>(this, "databaseChange");
        }

        public void removeUser(int userId)
        {
            // Delete from database
            using (SQLiteConnection conn = new SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<User>();
                conn.Delete(userId);
            }

            // Notify ViewModel of changes
            //MessagingCenter.Send<DatabaseHelper>(this, "databaseChange");
        }

        public void clearUsers()
        {
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                conn.DeleteAll<User>();
            }
        }

        // Might need 
        public bool verifiedUser()
        {

            return false;
        }
    }
}
