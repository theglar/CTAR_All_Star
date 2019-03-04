using SQLite;
using System;
using System.Collections.Generic;

namespace CTAR_All_Star.Models
{
    public class Workout
    {
        [PrimaryKey, AutoIncrement, Unique]
        public int WorkID { get; set; }
        public string WorkoutName { get; set; }
        public string PatientEmrNumber { get; set; }
        public string DoctorID { get; set; }
        public string NumReps { get; set; }
        public string NumSets { get; set; }
        public List<double> ThresholdPercentage { get; set; }
        public double HoldDuration { get; set; }
        public double RestDuration { get; set; }

        public Workout()
        {
        }

        public Workout(string Name, string Patient, string NumReps, string NumSets, List<double> ThresholdPercentage)
        {
            this.WorkoutName = Name;
            this.PatientEmrNumber = Patient;
            this.NumReps = NumReps;
            this.NumSets = NumSets;
            this.ThresholdPercentage = ThresholdPercentage;
        }

        public bool CheckInformation()
        {
            if (!this.WorkoutName.Equals("") && !this.NumReps.Equals("") && !this.NumSets.Equals("") && !this.ThresholdPercentage.Equals(null)
                && !this.PatientEmrNumber.Equals(""))
                return true;
            else
                return false;
        }
    }
}