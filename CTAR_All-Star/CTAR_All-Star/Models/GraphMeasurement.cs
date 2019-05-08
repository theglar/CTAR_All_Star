using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace CTAR_All_Star.Models
{
    public class GraphMeasurement
    {
        //This file defines a class model representing the data needed for plotting each pressure measurement on the graph.
        //[PrimaryKey, AutoIncrement]

        public string Time { get; set; }

        public double? Pressure { get; set; }
    }
}
