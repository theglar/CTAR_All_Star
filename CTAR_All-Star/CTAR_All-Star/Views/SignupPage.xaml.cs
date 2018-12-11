using System;
using System.Collections.Generic;

using Xamarin.Forms;
using CTAR_All_Star.Models;

namespace CTAR_All_Star.Views
{
    public partial class SignupPage : ContentPage
    {
        public SignupPage()
        {
            InitializeComponent();
            Init();
        }

        void Init()
        {
            BackgroundColor = Constants.BackgroundColor;
            Lbl_newUser.TextColor = Constants.MainTextColor;
            Lbl_NewPassword.TextColor = Constants.MainTextColor;
            Lbl_ConfirmPass.TextColor = Constants.MainTextColor;
            ActivitySpinner.IsVisible = false;

          
            Entry_NewUser.Completed += (s, e) => Entry_NewPassword.Focus();
            Entry_NewPassword.Completed += (s, e) => Entry_ConfirmPass.Focus();
            Entry_ConfirmPass.Completed += (s, e) => SignUpProcedure(s, e);
        }


        void SignUpProcedure(object sender, EventArgs e)
        {
            User user = new User(Entry_NewUser.Text, Entry_NewPassword.Text);
            if (user.CheckInformation())
            {
                DisplayAlert("Account Created", "You've successfully  created an account.", "Ok");
                Navigation.PushAsync(new SigninPage());
            }

            else
            {
                DisplayAlert("Account Failed", "There was an error creating a new account", "Ok");
            }
        }

        void SignUpProcedure()
        {
            Navigation.PushAsync(new SigninPage());
        }
    }
}
