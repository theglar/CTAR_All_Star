﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTAR_All_Star.Views;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CTAR_All_Star
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CreateExercise : ContentPage
	{
		public CreateExercise ()
		{
			InitializeComponent ();
		}
        private void Signin_Activated(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SigninPage());
        }
        private void History_Activated(object sender, EventArgs e)
        {
            Navigation.PushAsync(new HistoryPage());
        }
        private void Graph_Activated(object sender, EventArgs e)
        {
            Navigation.PushAsync(new GraphPage());
        }
        private void Setup_Activated(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SetupPage());
        }
        private void Measure_Activated(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CreatePage());
        }
        private void Remove_Activated(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RemovePage());
        }
        private void CreateExercise_Activated(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CreateExercise());
        }
        private void ChooseExercise_Activated(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ManageExercise());
        }
        private void Patients_Activated(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ManagePatients());
        }
        private void Home_Activated(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MainPage());
        }
    }
}