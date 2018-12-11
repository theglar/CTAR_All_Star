using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTAR_All_Star.Models;
using CTAR_All_Star;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CTAR_All_Star
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CreatePage : ContentPage
	{
		public CreatePage ()
		{
			InitializeComponent ();
		}
        private void Button_Clicked(object sender, EventArgs e)
        {
            // Get current date and time
            DateTime d = DateTime.Now;
            DateTime dt = DateTime.Parse(d.ToString());

            Measurement measurement = new Measurement()
            {
                UserName = nameEntry.Text,
                SessionNumber = sessionEntry.Text,
                TimeStamp = d,
                Pressure = Convert.ToDouble(pressureEntry.Text),
                Duration = durationEntry.Text,
                DisplayTime = dt.ToString("HH:mm:ss")
            };

            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<Measurement>();
                var numberOfRows = conn.Insert(measurement);

                if (numberOfRows > 0)
                {
                    DisplayAlert("Success", "You have added a measurement!", "Dismiss");
                }
                else
                {
                    DisplayAlert("Failure", "Measurement failed to be inserted!", "Dismiss");
                }
            }
        }

        private void Signin_Activated(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SigninPage());
        }
        private void History_Activated(object sender, EventArgs e)
        {
            Navigation.PushAsync(new HistoryPage());
        }
        private void Graph_Activated(object sender, EventArgs e)
        {
            Navigation.PushAsync(new GraphPage());
        }
        private void Setup_Activated(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SetupPage());
        }
        private void Measure_Activated(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CreatePage());
        }
        private void Remove_Activated(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RemovePage());
        }
    }
}