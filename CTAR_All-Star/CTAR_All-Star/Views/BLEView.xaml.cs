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
        private IDevice SelectedDevice;
        private BLEViewModel VM;

		public BLEView ()
		{
			InitializeComponent ();
            VM = new BLEViewModel();
            LV.ItemsSource = VM.GetDeviceList();
		}

        private async void LV_ItemSelected(object Sender, EventArgs E)
        {
            if (LV.SelectedItem == null)
            {
                await DisplayAlert("Notice", "No Device selected", "OK");
                return;
            }
            else
            {
                SelectedDevice = LV.SelectedItem as IDevice;
                //try
                //{
                    //await DisplayAlert("Notice", "Connected!", "OK")
                    //Device.BeginInvokeOnMainThread(() =>
                    //{
                        VM.StopScan();
                        VM.ConnectToDevice(SelectedDevice);
                    //});
                    BtnConnectBluetooth.Text = "Tap to scan for devices";
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
        private async void OnScanClicked(object Sender, EventArgs args)
        {
            //Button button = (Button)Sender;
            VM.ClearDeviceList();

            if (!VM.IsScanning())
            {
                if (VM.IsOn())
                {
                    BtnConnectBluetooth.Text = "Scanning... tap to stop";
                    VM.SetScanTimeout(30000);
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        VM.StartScan();
                    });
                }
                else
                {
                    await DisplayAlert("Notice", "Bluetooth is turned off. Please turn it on!", "OK");
                }
            }
            else
            {
                BtnConnectBluetooth.Text = "Tap to scan for devices";
                Device.BeginInvokeOnMainThread(() =>
                {
                    VM.StopScan();
                });
            }
        }
    }
}