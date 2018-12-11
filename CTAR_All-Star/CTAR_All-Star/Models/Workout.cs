using System;
namespace CTAR_All_Star.Models
{
    public class Workout
    {
        public int work { get; set; }
        public string NumReps { get; set; }
        public string NumSets { get; set; }
        public string ThresholdPercentage { get; set; }
        public Workout()
        {
        }

        public Workout(string NumReps, string NumSets, string ThresholdPercentage)
        {
            this.NumReps = NumReps;
            this.NumSets = NumSets;
            this.ThresholdPercentage = ThresholdPercentage;
        }

        public bool CheckInformation()
        {
            if (!this.NumReps.Equals("") && !this.NumSets.Equals("") && !this.ThresholdPercentage.Equals(""))
                return true;
            else
                return false;
        }
    }
}