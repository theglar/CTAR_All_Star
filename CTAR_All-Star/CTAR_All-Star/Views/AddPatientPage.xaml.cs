﻿using CTAR_All_Star.Database;
using CTAR_All_Star.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                PatientEmrNumber = patientIdEntry.Text
            };

            dbHelper.addPatient(patient);
            DisplayAlert("Success", "You have added a patient!", "Dismiss");
            Navigation.PushAsync(new ManagePatients());
        }
    }
}