//using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using System;
using Xamarin.Forms;
//using Plugin.BLE.Abstractions.Exceptions;

namespace CTAR_All_Star
{
    public partial class BLEView : ContentPage
    {
        StackLayout availableDevices = new StackLayout();
        IDevice selectedDevice;
        BLEViewModel ble;

        public BLEView()
        {
            InitializeComponent();
            ble = App.ble;
            lv.ItemsSource = ble.deviceListReadOnly;

            if(ble.deviceConnected)
            {
                btnConnectBluetooth.Text = "Connected! Tap to disconnect";
            }
            else if(ble.isScanning)
            {
                btnConnectBluetooth.Text = "Scanning... tap to stop";
            }
            else //not connected, not scanning
            {
                btnConnectBluetooth.Text = "Tap to scan for devices";
            }

            ble.PropertyChanged += async (s, e) =>
            {
                switch (e.PropertyName)
                {
                    case "state":
                        if (ble.isOn)
                        {
                            //Device.BeginInvokeOnMainThread(() =>
                            //{
                                await DisplayAlert("Notice", "Bluetooth is on", "OK");
                            //});
                        }
                        else
                        {
                            //Device.BeginInvokeOnMainThread(() =>
                            //{
                                await DisplayAlert("Notice", "Bluetooth is off", "OK");
                            //});
                        }
                        break;
                    case "scanTimeoutElapsed":
                        //Device.BeginInvokeOnMainThread(() =>
                        //{
                            await DisplayAlert("Notice", "Scan Timeout elapsed", "OK");
                            btnConnectBluetooth.Text  = "Tap to scan for devices";
                        //});
                        break;
                    case "deviceConnected":
                        if (ble.deviceConnected)
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                DisplayAlert("Notice", "Device Connected!", "OK");
                                btnConnectBluetooth.Text = "Connected! Tap to disconnect";
                            });
                        }
                        else
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                DisplayAlert("Notice", "Device Disconnected!", "OK");
                                btnConnectBluetooth.Text = "Tap to scan for devices";
                            });
                        }
                        break;
                    case "isScanning":
                        if (ble.isScanning)
                        {
                            //Device.BeginInvokeOnMainThread(() =>
                            //{
                                btnConnectBluetooth.Text = "Scanning... tap to stop";
                            //});
                        }
                        else
                        {
                            //Device.BeginInvokeOnMainThread(() =>
                            //{
                                btnConnectBluetooth.Text = "Tap to scan for devices";
                            //});
                        }
                        break;
                    case "pressure":
                        break;
                    default:
                        break;
                }
            };
        }

        //private void Ble_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

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
                ble.ConnectToDevice(selectedDevice);
            }
        }

        private void OnScanClicked(object sender, EventArgs args)
        {
            if(ble.isOn)
            {
                if (ble.deviceConnected)
                {
                    ble.DisconnectDevice();
                }
                else
                {
                    ble.ToggleScan();
                }
            }
            else
            {
                DisplayAlert("Notice", "Bluetooth is off! Please turn it on.", "OK");
            }
        }
    }
}
