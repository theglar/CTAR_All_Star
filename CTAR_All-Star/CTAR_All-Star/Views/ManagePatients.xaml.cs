using System;
using System.Linq;
using CTAR_All_Star.Database;
using CTAR_All_Star.Models;
using CTAR_All_Star.ViewModels;
using CTAR_All_Star.Views;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CTAR_All_Star
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ManagePatients : ContentPage
	{
        PatientListViewModel patientListViewModel;
        DatabaseHelper dbHelper = new DatabaseHelper();

		public ManagePatients ()
		{
			InitializeComponent ();
            patientListViewModel = new PatientListViewModel();
            BindingContext = patientListViewModel;
		}

        private void Add_Button_Clicked(object sender, EventArgs e)
        {
            //Navigation.PopAsync();
            Navigation.PushAsync(new AddPatientPage());
        }

        private async void Delete_Button_Clicked(object sender, EventArgs e)
        {
            if (patientList == null)
            {
                DisplayAlert("No Patient Selected", "Please select a patient.", "OK");
                return;
            }

            Patient patient = patientList.SelectedItem as Patient;

            bool removePatient = await DisplayAlert("Remove " + patient.PatientEmrNumber, "Continue? This cannot be undone.", "Yes", "Cancel");
            if(removePatient)
            {
                using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
                {
                    conn.CreateTable<Patient>();
                    patient = conn.Query<Patient>("select * from Patient where PatientEmrNumber = " + patient.PatientEmrNumber).SingleOrDefault();
                    if (patient != null)
                    {
                        DatabaseHelper dbHelper = new DatabaseHelper();
                        dbHelper.removePatient(patient);
                        DisplayAlert("Deletion Complete", patient.PatientEmrNumber + " was successfully deleted", "OK");
                    }
                }
            }
        }

        private void Assign_Button_Clicked(object sender, EventArgs e)
        {

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}