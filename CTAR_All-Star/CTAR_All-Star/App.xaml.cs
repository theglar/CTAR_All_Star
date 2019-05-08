using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CTAR_All_Star.Views;
using CTAR_All_Star.Models;
using CTAR_All_Star.Database;
using CTAR_All_Star.Navigation;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
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
        public static BLEViewModel ble = new BLEViewModel();
        public static string PatientFilter = "";
        public static string DateFilter = "";
        public static string SessionFilter = "";

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
                currentUser.OneRepMax = 250; //For testing
                MainPage = new HomePage();

            });

            // Listen for signal to update MainPage after successful logout
            MessagingCenter.Subscribe<LogoutPage>(this, "logOutSuccessful", (sender) =>
            {
                currentUser = null;
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
