using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using CTAR_All_Star;
using SQLite;
using Xamarin.Forms.Xaml;

namespace CTAR_All_Star.Model
{
    public class ViewModel  
    { 
      public List<Measurement> Data { get; set; }

      public ViewModel()       
      {
            Data = new List<Measurement>();

            // Connect to database, pull data and store it in the list
            using (SQLiteConnection conn = new SQLiteConnection(App.DB_PATH))
            {
                //To display all data in the table
                var table = conn.Table<Measurement>();
                foreach (var m in table)
                {
                    Data.Add(m);               
                }
            }
        }
    }
}
