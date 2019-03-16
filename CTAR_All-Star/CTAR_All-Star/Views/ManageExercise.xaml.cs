using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CTAR_All_Star.Views;
using CTAR_All_Star.ViewModels;
using CTAR_All_Star.Database;
using CTAR_All_Star.Models;
using Android.App;

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
		}  
        
        private void New_Button_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CreateExercise());
        }

        private async void Select_Button_Clicked(object sender, EventArgs e)
        {
            Workout workout;
            var button = sender as Button;
            var item = button.BindingContext as Workout;

            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<Workout>();
                workout = conn.Query<Workout>("select * from Workout where WorkId = " + item.WorkID).SingleOrDefault();
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