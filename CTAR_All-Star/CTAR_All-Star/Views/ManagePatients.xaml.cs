using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private void Button_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddPatientPage());
        }

        private void Delete_Button_Clicked(object sender, EventArgs e)
        {
            var item = ((Button)sender);
            DisplayAlert("Delete", item.CommandParameter + " delete", "OK");
            //int index = patientListViewModel.Patients.IndexOf();
           // dbHelper.removePatient(index);
        }  
    }
}