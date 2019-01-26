using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CTAR_All_Star.Views;
using CTAR_All_Star.Navigation;
using CTAR_All_Star.Models;
using SQLite;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace CTAR_All_Star
{
    public partial class App : Application
    {
        public static string DB_PATH = string.Empty;

        // Initialize a starting point
        Double pressure = 0;


        public App()
        {
            InitializeComponent();

            MainPage = new HomePage();
            
        }

        public App(string DB_Path)
        {
            InitializeComponent();

            DB_PATH = DB_Path;

            MainPage = new HomePage();
            
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
            for(int i = 0; i <10; i++)
            {               
                testAddItem();
            }
        }

        private void testAddItem()
        {
            //Create and add a measurement to the database
            // Get current date and time
            DateTime d = DateTime.Now;
            DateTime dt = DateTime.Parse(d.ToString());

            pressure = Math.Sin(Convert.ToDouble(d.Millisecond) / 10);

            Measurement measurement = new Measurement()
            {
                UserName = "Tester 1",
                SessionNumber = "1",
                TimeStamp = d,
                Pressure = pressure,
                Duration = "1",
                DisplayTime = dt.ToString("HH:mm:ss")
            };

            using (SQLiteConnection conn = new SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<Measurement>();
                conn.Insert(measurement);
            }

            // Notify ViewModel of changes
            MessagingCenter.Send<App>(this, "newMeasurement");
        }
    }
}
