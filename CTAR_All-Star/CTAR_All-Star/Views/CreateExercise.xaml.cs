using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTAR_All_Star.Database;
using CTAR_All_Star.Models;
using CTAR_All_Star.Views;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CTAR_All_Star
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CreateExercise : ContentPage
	{
		public CreateExercise()
		{
			InitializeComponent();
            Init();
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

            Exercise.SelectedIndexChanged += this.myPickerSelectedIndexChanged;
            UserID.SelectedIndexChanged += this.myPatientPickerSelectedIndexChanged;

        
        }

        void SaveWorkoutProcedure(object sender, EventArgs e)
        {
            Workout workout = new Workout();
            if(UserID.SelectedItem.ToString().Equals("New"))
            {
                workout = new Workout(Entry_WorkoutName.Text, Entry_NewPatientID.Text, App.currentUser.Username, Entry_NumReps.Text, Entry_NumSets.Text, Entry_Threshold.Text,
                Entry_HoldDuration.Text, Entry_RestDuration.Text);
            }
            else
            {
                workout = new Workout(Entry_WorkoutName.Text, UserID.SelectedItem.ToString(), App.currentUser.Username, Entry_NumReps.Text, Entry_NumSets.Text, Entry_Threshold.Text,
                Entry_HoldDuration.Text, Entry_RestDuration.Text);
            }            

            if (workout.CheckInformation())
            {
                //For testing
                //App.currentWorkout = workout;
                DatabaseHelper dbHelper = new DatabaseHelper();

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
            if(UserID.SelectedIndex == 5) //Will need to fix this when we change the hardcoding
            {
                Lbl_NewPatientID.IsVisible = true;
                Entry_NewPatientID.IsVisible = true;
            }
        }
    }
}