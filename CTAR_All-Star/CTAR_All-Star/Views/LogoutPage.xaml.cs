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
	public partial class LogoutPage : ContentPage
	{
		public LogoutPage ()
		{
			InitializeComponent ();
		}
        void LogOutButtonClicked(object sender, EventArgs e)
        {
                DisplayAlert("Logout Success", "You've successfully logged out.", "Ok");
                //Notify App to change Main Page
                Device.BeginInvokeOnMainThread(() => MessagingCenter.Send<LogoutPage>(this, "logOutSuccessful"));
                Navigation.PushModalAsync(new MainPage());            
        }
    }
}