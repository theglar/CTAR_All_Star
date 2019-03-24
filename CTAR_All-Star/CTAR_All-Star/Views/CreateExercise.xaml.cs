using System;
using CTAR_All_Star.Database;
using CTAR_All_Star.Models;
using CTAR_All_Star.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CTAR_All_Star
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CreateExercise : ContentPage
	{
        PatientListViewModel patientListViewModel;
        Patient newPatient = new Patient();
        Patient patient = new Patient();
        DatabaseHelper dbHelper = new DatabaseHelper();
        Boolean isNew = true;

        public CreateExercise()
		{
            isNew = true;
            newPatient.PatientEmrNumber = "New";
            InitializeComponent();
            patientListViewModel = new PatientListViewModel();
            patientListViewModel.Patients.Add(newPatient);
            BindingContext = patientListViewModel;
            Init();
		}

        //For workout duplication functionality
        public CreateExercise(Workout workout)
        {
            isNew = false;
            newPatient.PatientEmrNumber = "New";
            InitializeComponent();
            patientListViewModel = new PatientListViewModel();
            patientListViewModel.Patients.Add(newPatient);
            BindingContext = patientListViewModel;
            Init();
            SetWorkout(workout);
        }

        void Init()
        {
            BackgroundColor = Constants.BackgroundColor;
            Lbl_NumReps.TextColor = Constants.MainTextColor;
            Lbl_NumSets.TextColor = Constants.MainTextColor;
            Lbl_Threshold.TextColor = Constants.MainTextColor;
            Lbl_WorkoutName.TextColor = Constants.MainTextColor;
            Lbl_HoldDuration.TextColor = Constants.MainTextColor;
            Lbl_RestDuration.TextColor = Constants.MainTextColor;
            Lbl_NewPatientID.TextColor = Constants.MainTextColor;

            UserID.BackgroundColor = Constants.MainTextColor;
            Exercise.BackgroundColor = Constants.MainTextColor;

            Entry_NumReps.Completed += (s, e) => Entry_NumSets.Focus();
            Entry_NumSets.Completed += (s, e) => Entry_Threshold.Focus();
            Entry_Threshold.Completed += (s, e) => SaveWorkoutProcedure(s, e);
            Entry_WorkoutName.Completed += (s, e) => Entry_WorkoutName.Focus();
            Entry_HoldDuration.Completed += (s, e) => Entry_HoldDuration.Focus();
            Entry_RestDuration.Completed += (s, e) => Entry_RestDuration.Focus();
            Entry_NewPatientID.Completed += (s, e) => Entry_RestDuration.Focus();


            UserID.SelectedIndexChanged += this.myPatientPickerSelectedIndexChanged;

            // Only enable if it's a brand new exercise
            if (isNew)
            {
                Exercise.SelectedIndexChanged += this.myPickerSelectedIndexChanged;
            }           
                    
        }

        void SaveWorkoutProcedure(object sender, EventArgs e)
        {
            Workout workout;
            string type;
            if (Exercise.SelectedIndex == 0) { type = "Isometric"; } else { type = "Isotonic"; }

            if (Entry_NewPatientID.Text != "")
            {
                patient.PatientEmrNumber = Entry_NewPatientID.Text;
                patient.DoctorName = App.currentUser.Username;
                dbHelper.addPatient(patient);
                DisplayAlert("New Patient", "You have also added a new patient.", "Ok");
                
                workout = new Workout(Entry_WorkoutName.Text, Entry_NewPatientID.Text, App.currentUser.Username, Entry_NumReps.Text, Entry_NumSets.Text, Entry_Threshold.Text,
                Entry_HoldDuration.Text, Entry_RestDuration.Text, type);
            }
            else
            {
                patient = (Patient)UserID.SelectedItem;
                workout = new Workout(Entry_WorkoutName.Text, patient.PatientEmrNumber, App.currentUser.Username, Entry_NumReps.Text, Entry_NumSets.Text, Entry_Threshold.Text,
                Entry_HoldDuration.Text, Entry_RestDuration.Text, type);
            }            

            if (workout.CheckInformation())
            {
                dbHelper.addWorkout(workout);
                DisplayAlert("Success", "You have added an exercise!", "Dismiss");
                Navigation.PopAsync();                
            }

            else
            {
                DisplayAlert("Workout Failed", "Please complete all fields", "Ok");
            }
        }
        
        public void myPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            if(Exercise.SelectedIndex == 0) //Isometric
            {
                Entry_NumReps.Text = "1";
                Entry_NumSets.Text = "3";
                Entry_HoldDuration.Text = "30";
                Entry_RestDuration.Text = "30";

            }
            else //Isotonic
            {
                Entry_NumReps.Text = "10";
                Entry_NumSets.Text = "3";
                Entry_HoldDuration.Text = "1";
                Entry_RestDuration.Text = "1";
            }
        }

        public void myPatientPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            if(UserID.SelectedIndex == patientListViewModel.Patients.Count - 1) 
            {
                Lbl_NewPatientID.IsVisible = true;
                Entry_NewPatientID.IsVisible = true;
            }
            else
            {
                Lbl_NewPatientID.IsVisible = false;
                Entry_NewPatientID.IsVisible = false;
            }
        }

        void SetWorkout(Workout workout)
        {
            Entry_WorkoutName.Text = workout.WorkoutName;
            Entry_NumReps.Text = workout.NumReps;
            Entry_NumSets.Text = workout.NumSets;
            Entry_Threshold.Text = workout.ThresholdPercentage;
            Entry_HoldDuration.Text = workout.HoldDuration;
            Entry_RestDuration.Text = workout.RestDuration;
            if(workout.Type.Equals("Isometric"))
            {
                Exercise.SelectedIndex = 0;
            }
            else { Exercise.SelectedIndex = 1; }
        }
    }
}