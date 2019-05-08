using SQLite;

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
        public string ThresholdPercentage { get; set; }
        public string HoldDuration { get; set; }
        public string RestDuration { get; set; }
        public string Type { get; set; }

        public Workout()
        {
        }

        public Workout(string Name, string Patient, string Doctor, string NumReps, string NumSets, string ThresholdPercentage,
                        string Hold, string Rest, string Type)
        {
            this.WorkoutName = Name;
            this.PatientEmrNumber = Patient;
            this.DoctorID = Doctor;
            this.NumReps = NumReps;
            this.NumSets = NumSets;
            this.ThresholdPercentage = ThresholdPercentage;
            this.HoldDuration = Hold;
            this.RestDuration = Rest;
            this.Type = Type;
        }

        public bool CheckInformation()
        {
            if (!this.WorkoutName.Equals("") && !this.NumReps.Equals("") && !this.NumSets.Equals("") && !this.ThresholdPercentage.Equals("")
                && !this.PatientEmrNumber.Equals("") && !this.DoctorID.Equals("") && !this.HoldDuration.Equals("") && !this.RestDuration.Equals("") && !this.Type.Equals(""))
                return true;
            else
                return false;
        }
    }
}