using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CTAR_All_Star.Views
{

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CalibrationPage : ContentPage
	{
        public CalibrationPage ()
		{
			InitializeComponent ();
            PressureLabel.Text = "0.00";
		}
    }
}