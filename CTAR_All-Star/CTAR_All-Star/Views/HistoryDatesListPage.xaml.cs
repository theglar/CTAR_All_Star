using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTAR_All_Star.Database;
using CTAR_All_Star.Models;
using CTAR_All_Star.Views;
using CTAR_All_Star.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CTAR_All_Star
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HistoryDatesListPage : ContentPage
	{
        HistoryListViewModel historyListViewModel;
        DatabaseHelper dbHelper = new DatabaseHelper();

		public HistoryDatesListPage()
		{
			InitializeComponent ();
            historyListViewModel = new HistoryListViewModel();
            BindingContext = historyListViewModel;
            Init();
		}

        void Init()
        {
            if (App.currentUser.userType.Equals("Doctor"))
            {
                //Add options for doctors later
            }
        }

        private async void View_Clicked(object sender, EventArgs e)
        {
            if(historyList.SelectedItem == null)
            {
                DisplayAlert("No Date Selected", "Please select a date.", "OK");
                return;
            }

            Measurement measurement = historyList.SelectedItem as Measurement;

            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<Measurement>();
                measurement = conn.Query<Measurement>("select * from Measurement where Id = " + measurement.Id).SingleOrDefault();
                if (measurement != null)
                {
                    //App.currentWorkout = measurement;
                    //bool startExercise = await DisplayAlert("You selected " + measurement.WorkoutName, "Begin workout?", "Yes", "Cancel");
                    //if (startExercise)
                    //    Navigation.PushAsync(new GraphPage());
                }
                else
                    DisplayAlert("Failed", "workout is null", "ok");
            }

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}