using System;
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
	public partial class SetupPage : ContentPage
	{
		public SetupPage ()
		{
			InitializeComponent ();
		}
        private void BT_Button_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Bluetooth Manager", "This will eventually send you to a blutooth setup/manage page.", "Dismiss");
        }
        private void Train_Button_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Train 1 Rep Max", "This will eventually send you to a training page.", "Dismiss");
        }
        private void Timer_Button_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Timer", "This will eventually allow you to set/reset a timer.", "Dismiss");
        }
        private void Profile_Button_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Patient Profile", "This will eventually lead you to your profile page.", "Dismiss");
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