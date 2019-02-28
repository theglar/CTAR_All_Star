using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CTAR_All_Star.Views;
using CTAR_All_Star.Navigation;
using CTAR_All_Star.Models;
using SQLite;
using CTAR_All_Star.Database;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace CTAR_All_Star
{
    public partial class App : Application
    {
        public static string DB_PATH = string.Empty;
        DatabaseHelper dbHelper = new DatabaseHelper();

        public App()
        {
            InitializeComponent();

            MainPage = new HomePage();
            
        }

        public App(string DB_Path)
        {
            InitializeComponent();

            DB_PATH = DB_Path;
            
            dbHelper.initializeAllTables();

            //MainPage = new HomePage();
            MainPage = new SigninPage();

            // Listen for signal to update MainPage after successful login
            MessagingCenter.Subscribe<SigninPage>(this, "signInSuccessful", (sender) =>
            {
                MainPage = new HomePage();
            });

            // Listen for signal to update MainPage after successful logout
            MessagingCenter.Subscribe<LogoutPage>(this, "logOutSuccessful", (sender) =>
            {
                MainPage = new SigninPage();
            });

        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            
        }
    }
}
