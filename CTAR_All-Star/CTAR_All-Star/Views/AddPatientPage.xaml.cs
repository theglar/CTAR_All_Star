using CTAR_All_Star.Database;
using CTAR_All_Star.Models;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CTAR_All_Star.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddPatientPage : ContentPage
	{
		public AddPatientPage ()
		{
			InitializeComponent ();
		}

        private void Button_Clicked(object sender, EventArgs e)
        {
            DatabaseHelper dbHelper = new DatabaseHelper();

            Patient patient = new Patient()
            {
                PatientEmrNumber = patientIdEntry.Text,
                DoctorName = App.currentUser.Username
            };

            dbHelper.addPatient(patient);
            DisplayAlert("Success", "You have added a patient!", "Dismiss");
            Navigation.PopAsync();
        }
    }
}