using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            UserID.BackgroundColor = Constants.MainTextColor;
            Exercise.BackgroundColor = Constants.MainTextColor;

            Entry_NumReps.Completed += (s, e) => Entry_NumSets.Focus();
            Entry_NumSets.Completed += (s, e) => Entry_Threshold.Focus();
            Entry_Threshold.Completed += (s, e) => SaveWorkoutProcedure(s, e);
            Entry_WorkoutName.Completed += (s, e) => Entry_WorkoutName.Focus();
        }

        void SaveWorkoutProcedure(object sender, EventArgs e)
        {
            Workout workout = new Workout(Entry_WorkoutName.Text, UserID.SelectedItem.ToString(), Entry_NumReps.Text, Entry_NumSets.Text, null); //Add list here later
            if (workout.CheckInformation())
            {
                DisplayAlert("Workout Saved", "You've successfully saved a workout.", "Ok");
            }

            else
            {
                DisplayAlert("Workout Failed", "Please complete all fields", "Ok");
            }
        }       
    }
}