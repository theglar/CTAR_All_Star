using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace CTAR_All_Star.Models
{
     public class Patient
    {
        [PrimaryKey, AutoIncrement, Unique]
        public int PatientId { get; set; }
        public string PatientEmrNumber { get; set; }
        public string DoctorName { get; set; }

        public Patient()
        {

        }

        public Patient(string num, string doc)
        {
            this.PatientEmrNumber = num;
            this.DoctorName = doc;
        }
    }
}
