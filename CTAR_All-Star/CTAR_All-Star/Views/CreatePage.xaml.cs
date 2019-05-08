using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTAR_All_Star.Models;
using CTAR_All_Star;
using CTAR_All_Star.Views;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CTAR_All_Star.Database;

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
            DatabaseHelper dbHelper = new DatabaseHelper();

            // Get current date and time
            DateTime d = DateTime.Now;
            DateTime dt = DateTime.Parse(d.ToString());

            Measurement measurement = new Measurement()
            {
                UserName = nameEntry.Text,
                DocID = String.Empty,
                SessionNumber = sessionEntry.Text,
                TimeStamp = d,
                Pressure = Convert.ToDouble(pressureEntry.Text),
                DisplayTime = dt.ToString("HH:mm:ss"),
                DisplayDate = dt.ToString("MM/dd/yy"),
                OneRepMax = App.currentUser.OneRepMax
            };

            dbHelper.addData(measurement);
            DisplayAlert("Success", "You have added a measurement!", "Dismiss");
        }
    }
}