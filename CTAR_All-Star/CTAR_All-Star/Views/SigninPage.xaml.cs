using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTAR_All_Star.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CTAR_All_Star.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SigninPage : ContentPage
	{
		public SigninPage ()
		{
			InitializeComponent();
            Init();
		}

        void Init()
        {
            BackgroundColor = Constants.BackgroundColor;
            Lbl_Username.TextColor = Constants.MainTextColor;
            Lbl_Password.TextColor = Constants.MainTextColor;
            ActivitySpinner.IsVisible = false;
            LoginIcon.HeightRequest = Constants.LoginIconHeight;

            Entry_Username.Completed += (s, e) => Entry_Password.Focus();
            Entry_Password.Completed += (s, e) => SignInProcedure(s, e);
        }

        void SignInProcedure(object sender, EventArgs e)
        {
			User user = new User(Entry_Username.Text, Entry_Password.Text);
            if (user.CheckInformation())
            {
                DisplayAlert("Login Success","You've successfully logged in.","Ok");
                //Notify App to change Main Page
                Device.BeginInvokeOnMainThread(() => MessagingCenter.Send<SigninPage>(this, "signInSuccessful"));
                Navigation.PushModalAsync(new MainPage());
            }

            else 
            {
                DisplayAlert("Login Failed", "Incorrect Username or Password", "Ok");
            }
        }

        void SignUpProcedure()
        {
            Navigation.PushModalAsync(new SignupPage());
        }

        
    }
}


