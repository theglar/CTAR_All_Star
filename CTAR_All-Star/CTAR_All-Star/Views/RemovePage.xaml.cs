using System;
using CTAR_All_Star.Database;

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
        private void Button_Clicked(object sender, EventArgs e)
        {
            DatabaseHelper dbHelper = new DatabaseHelper();
            dbHelper.clearDatabase();
            DisplayAlert("Success", "Table has been cleared!", "Dismiss");
        }        
    }
}