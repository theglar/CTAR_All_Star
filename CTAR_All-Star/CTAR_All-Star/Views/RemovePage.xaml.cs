using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTAR_All_Star.Database;
using CTAR_All_Star.Models;
using CTAR_All_Star.Views;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CTAR_All_Star
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RemovePage : ContentPage
	{
		public RemovePage()
		{
			InitializeComponent ();
		}
        private async void Button_Clicked(object sender, EventArgs e)
        {
            bool clear = await DisplayAlert("Delete History", "Are you sure this is what you want to do? THIS CANNOT BE UNDONE!", "Cancel", "Delete");
            if (!clear)
            {
                DatabaseHelper dbHelper = new DatabaseHelper();
                dbHelper.clearDatabase();
                DisplayAlert("Success", "HIstory has been cleared!", "Dismiss");
            }

            
        }        
    }
}