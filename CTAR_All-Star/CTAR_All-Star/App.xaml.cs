using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CTAR_All_Star.Views;
using CTAR_All_Star.Models;
using CTAR_All_Star.Database;
using System.Threading.Tasks;
using CTAR_All_Star.Navigation;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace CTAR_All_Star
{
    public partial class App : Application
    {
        public static string DB_PATH = string.Empty;
        DatabaseHelper dbHelper = new DatabaseHelper();

        //Globals
        public static User currentUser = new User();
        public static Workout currentWorkout = new Workout();
        public static Measurement currentMeasurement = new Measurement();

        public App()
        {
            //InitializeComponent();

            //MainPage = new HomePage();
            
        }

        public App(string DB_Path)
        {
            InitializeComponent();

            DB_PATH = DB_Path;

            //dbHelper.deleteAllTables(); //For whenever changes are made to database tables - run once, then comment out
            dbHelper.initializeAllTables();
            
            MainPage = new SigninPage();

            // Listen for signal to update MainPage after successful login
            MessagingCenter.Subscribe<SigninPage, User>(this, "signInSuccessful", (sender, user) =>
            {
                currentUser = user;
                currentUser.IsLoggedIn = true;
                currentUser.OneRepMax = 500; //For testing
                MainPage = new HomePage();

            });

            // Listen for signal to update MainPage after successful logout
            MessagingCenter.Subscribe<LogoutPage>(this, "logOutSuccessful", (sender) =>
            {
                dbHelper.removeUser(currentUser);
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
