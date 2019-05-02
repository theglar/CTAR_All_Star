//This will be a page for the doctos only. A similar page for the patients is called PatientManageExercise

using System;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CTAR_All_Star.ViewModels;
using CTAR_All_Star.Database;
using CTAR_All_Star.Models;

namespace CTAR_All_Star
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ManageExercise : ContentPage
	{
        WorkoutListViewModel workoutListViewModel;
        DatabaseHelper dbHelper = new DatabaseHelper();

		public ManageExercise ()
		{            
			InitializeComponent ();
            workoutListViewModel = new WorkoutListViewModel();
            BindingContext = workoutListViewModel;
            Init();
		}
        
        void Init()
        {
            if(App.currentUser.userType.Equals("Doctor"))
            {
                AssignButton.IsVisible = true;
                RemoveButton.IsVisible = true;
                Header.Text = "Manage Exercises";
            }
            else
            {
                AssignButton.IsVisible = false;
                RemoveButton.IsVisible = false;
                Header.Text = "Exercises";
            }
        }
        
        private void New_Button_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CreateExercise());
        }

        private async void Begin_Button_Clicked(object sender, EventArgs e)
        {
            if (workoutList.SelectedItem == null)
            {
                DisplayAlert("No Exercise Selected", "Please select an exercise.", "OK");
                return;
            }
            Workout workout = workoutList.SelectedItem as Workout;

            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<Workout>();
                workout = conn.Query<Workout>("select * from Workout where WorkId = " + workout.WorkID).SingleOrDefault();
                if (workout != null)
                {
                    App.currentWorkout = workout;
                    bool startExercise = await DisplayAlert("You selected " + workout.WorkoutName, "Begin workout?", "Yes", "Cancel");
                    if(startExercise)
                        Navigation.PushAsync(new GraphPage());
                }
                else
                    DisplayAlert("Failed", "workout is null", "ok");
            }
        }

        private async void Details_Button_Clicked()
        {
            if (workoutList.SelectedItem == null)
            {
                DisplayAlert("No Exercise Selected", "Please select an exercise.", "OK");
                return;
            }
            Workout workout = workoutList.SelectedItem as Workout;

            DisplayAlert(workout.WorkoutName + " Details", "Reps: " + workout.NumReps + "\nSets: " + workout.NumReps + "\nThreshold: " 
                + workout.ThresholdPercentage + "%\nHold Duration: " + workout.HoldDuration + " second(s)\nRest Duration: " + workout.RestDuration + " second(s)", "OK");

        }
        private async void Assign_Button_Clicked()
        {
            if(workoutList.SelectedItem == null)
            {
                DisplayAlert("No Exercise Selected", "Please select an exercise.", "OK");
                return;
            }
            Workout workout = workoutList.SelectedItem as Workout;

            bool getDetails = await DisplayAlert("Assign " + workout.WorkoutName, "You will be directed to the create exercise page to complete the assignment.", "OK", "Cancel");
            if (getDetails)
            {
                Navigation.PushAsync(new CreateExercise(workout));
            }
        }
        private async void Remove_Button_Clicked()
        {
            if (workoutList.SelectedItem == null)
            {
                DisplayAlert("No Exercise Selected", "Please select an exercise.", "OK");
                return;
            }
            Workout workout = workoutList.SelectedItem as Workout;

            bool removeWorkout = await DisplayAlert("Remove " + workout.WorkoutName, "Continue? This cannot be undone.", "Yes", "Cancel");
            if (removeWorkout)
            {
                using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
                {
                    conn.CreateTable<Workout>();
                    workout = conn.Query<Workout>("select * from Workout where WorkID = " + workout.WorkID).SingleOrDefault();
                    if (workout != null)
                    {
                        DatabaseHelper dbHelper = new DatabaseHelper();
                        dbHelper.removeWorkout(workout);
                        DisplayAlert("Deletion Complete", workout.WorkoutName + " was successfully deleted", "OK");
                    }
                }
            }
        }

        private void Show_All_Clicked(object sender, EventArgs e)
        {
            workoutListViewModel.ShowAllData();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}