﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

		public ManagePatients ()
		{
			InitializeComponent ();
            patientListViewModel = new PatientListViewModel();
            BindingContext = patientListViewModel;
		}
        private void Button_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Add Patient", "This will eventually send you to a \"Create Patient\" page.", "Dismiss");
        }
        private void Delete_Button_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Delete Selected Patient", "This will eventually remove this patient from your list.", "Dismiss");
        }        
    }
}