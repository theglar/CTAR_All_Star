﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Measurement measurement = new Measurement()
            {
                UserName = nameEntry.Text,
                SessionNumber = sessionEntry.Text,
                TimeStamp = DateTime.Now,
                Pressure = pressureEntry.Text,
                Duration = durationEntry.Text

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
    }
}