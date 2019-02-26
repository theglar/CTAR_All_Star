using System;
using System.Collections.Generic;

using Xamarin.Forms;
using CTAR_All_Star.Models;
using CTAR_All_Star.Database;

namespace CTAR_All_Star.Views
{
    public partial class SignupPage : ContentPage
    {
        public int typeIndex { get; set; }
        public string userType { get; set; }

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
            typePicker.BackgroundColor = Constants.MainTextColor;

            Entry_NewUser.Completed += (s, e) => Entry_NewPassword.Focus();
            Entry_NewPassword.Completed += (s, e) => Entry_ConfirmPass.Focus();
            Entry_ConfirmPass.Completed += (s, e) => SignUpProcedure(s, e);
        }


        void SignUpProcedure(object sender, EventArgs e)
        {
            DatabaseHelper dbHelper = new DatabaseHelper();
            User user = new User(Entry_NewUser.Text, Entry_NewPassword.Text, userType);
            if (dbHelper.UniqueUser(Entry_NewUser.Text) == true)
                if (user.VerifySignUp() && Entry_ConfirmPass.Text == Entry_NewPassword.Text)
                {
                    DisplayAlert("Account Created", "You've successfully  created an account.", "Ok");

                    dbHelper.addUser(user);
                    Navigation.PushAsync(new SigninPage());                

                    Navigation.PushModalAsync(new SigninPage());
                }

                else
                {
                    DisplayAlert("Account Failed", "There was an error creating a new account", "Ok");
                }
            else //say username already exists
                DisplayAlert("Account Not Created", "Username already exists", "Ok");
        }

        void typeChosen(object sender, EventArgs args)
        {
            Picker typePicker = (Picker)sender;
            typeIndex = typePicker.SelectedIndex;

            if (typeIndex == 0)
            {
                userType = "Patient";
            }
            else if (typeIndex == 1)
            {
                userType = "Doctor";
            }

            else
            {
                //it may be impossible for this case to ever be reached.
                DisplayAlert("Invalid Selection", "Choose a valid user type", "OK");
            }


            }


        //void SignUpProcedure()
        //{
        //    Navigation.PushAsync(new SigninPage());
        //}
    }
}
