﻿using System;
using CTAR_All_Star.Database;
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
            User user = new User();
            DatabaseHelper dbHelper = new DatabaseHelper();
            if (dbHelper.verifyUser(Entry_Username.Text, Entry_Password.Text))
            {
                DisplayAlert("Login Success","You've successfully logged in.","Ok");
                user = dbHelper.GetUser(Entry_Username.Text);
                //Notify App to change Main Page
                Device.BeginInvokeOnMainThread(() => MessagingCenter.Send<SigninPage, User>(this, "signInSuccessful", user));
                
                //Navigation.PushModalAsync(new MainPage());
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

/*
 * This login page was created based off of Bert Bosch's youtube tutorial 
 * series called "Xamarin Tutorial" at the following link 
 * https://www.youtube.com/watch?v=spPKRD0x_DQ
 */

