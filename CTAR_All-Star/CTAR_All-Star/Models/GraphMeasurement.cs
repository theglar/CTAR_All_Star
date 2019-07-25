using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace CTAR_All_Star.Models
{
    public class GraphMeasurement
    {
        //[PrimaryKey, AutoIncrement]

        public string Time { get; set; }

        public double? Pressure { get; set; }

        public GraphMeasurement()
        {

        }

        public GraphMeasurement(string t, double? p)
        {
            Time = t;
            Pressure = p;
        }
    }
}
