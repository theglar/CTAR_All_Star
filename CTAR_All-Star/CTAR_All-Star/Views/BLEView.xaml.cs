using CTAR_All_Star.ViewModels;
using Plugin.BLE.Abstractions.Contracts;
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
	public partial class BLEView : ContentPage
	{
        private IDevice selectedDevice;
        private BLEViewModel vm;

		public BLEView ()
		{
			InitializeComponent ();
		}

        private async void lv_ItemSelected(object sender, EventArgs e)
        {
            if (lv.SelectedItem == null)
            {
                await DisplayAlert("Notice", "No Device selected", "OK");
                return;
            }
            else
            {
                selectedDevice = lv.SelectedItem as IDevice;
                //try
                //{
                    //await DisplayAlert("Notice", "Connected!", "OK")
                    //Device.BeginInvokeOnMainThread(() =>
                    //{
                        vm.StopScan();
                        vm.ConnectToDevice(selectedDevice);
                    //});
                    btnConnectBluetooth.Text = "Tap to scan for devices";
                //}
                //catch (DeviceConnectionException ex)
                //{
                //    await DisplayAlert("Notice", "Error connecting to device!", "OK");
                //}
                //catch (ArgumentNullException ex)
                //{
                //    await DisplayAlert("Notice", "Selected device is null!", "OK");
                //}
                //catch (Exception ex)
                //{
                //    await DisplayAlert("notice", "unknown exception!", "ok");
                //}
            }
        }
        private async void OnScanClicked(object sender, EventArgs args)
        {
            //Button button = (Button)sender;
            vm.clearDevices();

            if (!vm.IsScanning)
            {
                if (vm.IsOn)
                {
                    btnConnectBluetooth.Text = "Scanning... tap to stop";
                    vm.setScanTimeout = 30000;
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        vm.StartScan();
                    });
                }
                else
                {
                    await DisplayAlert("Notice", "Bluetooth is turned off. Please turn it on!", "OK");
                }
            }
            else
            {
                btnConnectBluetooth.Text = "Tap to scan for devices";
                Device.BeginInvokeOnMainThread(() =>
                {
                    vm.StopScan();
                });
            }
        }
    }
}