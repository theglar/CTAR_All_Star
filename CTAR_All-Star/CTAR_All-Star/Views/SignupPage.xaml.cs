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
            Lbl_DocID.TextColor = Constants.MainTextColor;
            Lbl_EMR.TextColor = Constants.MainTextColor;
            Lbl_NewPassword.TextColor = Constants.MainTextColor;
            ActivitySpinner.IsVisible = false;

            Entry_DocID.Completed += (s, e) => Entry_NewPassword.Focus();
            Entry_EMR.Completed += (s, e) => Entry_NewPassword.Focus();
            Entry_NewPassword.Completed += (s, e) => SignUpProcedure(s, e);
        }

        void SignUpProcedure(object sender, EventArgs e)
        {

        }
    }
}
