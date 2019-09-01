using CTAR_All_Star.Helper;
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
        public String pressure = "Device Not Connected";

        public CalibrationPage ()
		{
			InitializeComponent ();

            App.ble.StartUpdates();
            App.ble.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == "pressure")
                {
                    changePressure(PressureConverter.convertToMMHG(App.ble.pressureVal).ToString());
                }
            };
        }

        protected override void OnDisappearing()
        {
            App.ble.StopUpdates();
            base.OnDisappearing();

        }

        private void changePressure(string pressure)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                PressureLabel.Text = pressure;
            });
        }
    }
}