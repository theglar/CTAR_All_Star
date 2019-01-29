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
            MessagingCenter.Send<DatabaseHelper>(this, "databaseChange");
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
            MessagingCenter.Send<DatabaseHelper>(this, "databaseChange");
        }

        public void clearDatabase()
        {
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                conn.DeleteAll<Measurement>();              
            }
        }
    }
}
