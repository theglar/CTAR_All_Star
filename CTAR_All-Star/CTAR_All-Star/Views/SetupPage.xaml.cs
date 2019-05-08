using System;

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
    }
}