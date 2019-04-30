using CTAR_All_Star.Database;
using CTAR_All_Star.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CTAR_All_Star.ViewModels;

namespace CTAR_All_Star
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TutorialsPage : ContentPage
	{
        public TutorialsPage()
		{
			InitializeComponent ();
		}

        private void Tutorial_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var vm = BindingContext as TutorialViewModel;
            var tutorial = e.Item as Tutorial;
            vm.HideorShowTutorial(tutorial);
        }

        private void WebClick(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri("https://theglar.wixsite.com/ctar"));
        }
    }
}