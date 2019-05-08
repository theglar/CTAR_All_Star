using CTAR_All_Star.Models;
using SQLite;
using System.Linq;
using Xamarin.Forms;
using System.Collections.ObjectModel;

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

        // For use in development
        public void deleteAllTables()
        {
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                conn.DropTable<Workout>();
                conn.DropTable<User>();
                conn.DropTable<Patient>();
                conn.DropTable<Measurement>();
            }
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
            //Device.BeginInvokeOnMainThread(() => MessagingCenter.Send<DatabaseHelper,Measurement>(this, "databaseChange", measurement));
        }

        public void addDataList(ObservableCollection<Measurement> data)
        {
            // Add to database
            using (SQLiteConnection conn = new SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<Measurement>();
                foreach (var m in data)
                {
                    conn.Insert(m);
                }               
            }           
        }

        public void removeData(Measurement measurement)
        {
            // Delete from database
            using (SQLiteConnection conn = new SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<Measurement>();
                conn.Delete(measurement);
            }

            // Notify ViewModel of changes
            //Device.BeginInvokeOnMainThread(() => MessagingCenter.Send<DatabaseHelper>(this, "databaseChange"));
        }

        public void clearDatabase()
        {
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                conn.DeleteAll<Measurement>();
            }

            // Notify ViewModel of changes
            Device.BeginInvokeOnMainThread(() => MessagingCenter.Send<DatabaseHelper>(this, "databaseChange"));
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
            Device.BeginInvokeOnMainThread(() => MessagingCenter.Send<DatabaseHelper>(this, "workoutChange"));
        }

        public void removeWorkout(Workout workout)
        {
            // Delete from database
            using (SQLiteConnection conn = new SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<Workout>();
                conn.Delete(workout);
            }

            // Notify ViewModel of changes
            Device.BeginInvokeOnMainThread(() => MessagingCenter.Send<DatabaseHelper>(this, "workoutChange"));
        }

        public void clearWorkouts()
        {
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                conn.DeleteAll<Workout>();
            }

            // Notify ViewModel of changes
            Device.BeginInvokeOnMainThread(() => MessagingCenter.Send<DatabaseHelper>(this, "workoutChange"));
        }

        public Workout GetWorkout(int id)
        {
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                Workout thisWorkout = conn.Query<Workout>("select * from Workout where WorkID = " + id).SingleOrDefault();
                return thisWorkout;
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
                //conn.CreateTable<User>();
                conn.Insert(user);
            }

            //Notify ViewModel of changes
            MessagingCenter.Send<DatabaseHelper>(this, "userChange");
        }

        public void removeUser(User user)
        {
            // Delete from database
            using (SQLiteConnection conn = new SQLiteConnection(App.DB_PATH))
            {
                //conn.CreateTable<User>();
                conn.Delete(user);
            }

            // Notify ViewModel of changes
            MessagingCenter.Send<DatabaseHelper>(this, "userChange");
        }

        public void clearUsers()
        {
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                conn.DeleteAll<User>();
            }

            // Notify ViewModel of changes
            MessagingCenter.Send<DatabaseHelper>(this, "userChange");
        }
        
        public bool verifyUser(string username, string password)
        {
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                User thisUser = conn.Query<User>("select * from User where Username = " + "'" + username + "'").FirstOrDefault(); //SingleOfDefault?
                if(thisUser != null)
                {
                    if (thisUser.Password == password)
                    {
                        return true;
                    }
                    else
                    {
                        System.Console.WriteLine("This password does not match.");
                        return false;
                    }
                }
                else
                {
                    System.Console.WriteLine("This user does not exist.");
                    return false;
                }
            }
        }
        public User GetUser(string name)
        {
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                User thisUser = conn.Query<User>("select * from User where Username = " + "'" + name + "'").SingleOrDefault();
                return thisUser;
            }
        }
        public bool UniqueUser(string username)
        {
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                User thisUser = conn.Query<User>("Select * from User where Username = " + "'" + username + "'").SingleOrDefault(); //FirstorDefault?
                if(thisUser == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
