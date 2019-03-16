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
        Button selectBtn, runBtn, detailsBtn, assignBtn, removeBtn;

		public ManageExercise ()
		{            
			InitializeComponent ();
            InitializeButtons();
            workoutListViewModel = new WorkoutListViewModel();
            BindingContext = workoutListViewModel;

            //Set up correct views
            if(App.currentUser.userType.Equals("Doctor"))
            {
                selectBtn.IsVisible = false;
            }
            //Patient is the default
            else
            {

            }
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

        private void Details_Button_Clicked()
        {

        }
        private void Assign_Button_Clicked()
        {

        }
        private void Remove_Button_Clicked()
        {

        }

        private void Show_All_Clicked(object sender, EventArgs e)
        {
            workoutListViewModel.ShowAllData();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private void InitializeButtons()
        {
            selectBtn = this.FindByName<Button>("btnSelect");
        }
    }
}