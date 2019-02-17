using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTAR_All_Star.Database;
using CTAR_All_Star.Models;
using CTAR_All_Star.ViewModels;
using CTAR_All_Star.Views;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CTAR_All_Star
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ManagePatients : ContentPage
	{
        PatientListViewModel patientListViewModel;
        DatabaseHelper dbHelper = new DatabaseHelper();

		public ManagePatients ()
		{
			InitializeComponent ();
            patientListViewModel = new PatientListViewModel();
            BindingContext = patientListViewModel;
		}

        private void Button_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
            Navigation.PushAsync(new AddPatientPage());
        }

        private void Delete_Button_Clicked(object sender, EventArgs e)
        {
            Patient patient;
            var button = sender as Button;
            var item = button.BindingContext as Patient;

            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<Patient>();
                patient = conn.Query<Patient>("select * from Patient where patientId =" + item.PatientId).SingleOrDefault();
                if (patient != null)
                {
                    conn.Delete(patient);
                    DisplayAlert("Deleted", patient.PatientEmrNumber + " deleted", "OK");
                    Navigation.PopAsync();
                    Navigation.PushAsync(new ManagePatients());
                }
                else
                    DisplayAlert("Failed", "patient is null", "ok");
            }
            
            
            
           
        }

        //public string DeleteItem(int itemId)
        //{
        //    string result = string.Empty;
        //    using (var dbConn = new SQLiteConnection(App.SQLITE_PLATFORM, App.DB_PATH))
        //    {
        //        var existingItem = dbConn.Query<Patient>("select * from Media where Id =" + itemId).FirstOrDefault();
        //        if (existingItem != null)
        //        {
        //            dbConn.RunInTransaction(() =>
        //            {
        //                dbConn.Delete(existingItem);

        //                if (dbConn.Delete(existingItem) > 0)
        //                {
        //                    result = "Success";
        //                }
        //                else
        //                {
        //                    result = "This item was not removed";
        //                }

        //            });
        //        }

        //        return result;
        //    }
        //}
    }
}