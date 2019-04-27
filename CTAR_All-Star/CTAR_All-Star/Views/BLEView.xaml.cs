//using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using CTAR_All_Star.Views;
using CTAR_All_Star.Models;
using CTAR_All_Star.ViewModels;
//using Plugin.BLE.Abstractions.Exceptions;
using System.Diagnostics;
using Xamarin.Forms.PlatformConfiguration;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using CTAR_All_Star.Database;

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
            ble = new BLEViewModel();
            lv.ItemsSource = ble.deviceListReadOnly;

            ble.PropertyChanged += async (s, e) =>
            {
                switch (e.PropertyName)
                {
                    case "state":
                        if (ble.isOn)
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                DisplayAlert("Notice", "Bluetooth is on", "OK");
                            });
                        else
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                DisplayAlert("Notice", "Bluetooth is off", "OK");
                            });
                        break;
                    case "scanTimeout":
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            DisplayAlert("Notice", "Scan Timeout elapsed", "OK");
                        });
                        break;
                    case "deviceConnected":
                        if (ble.deviceConnected)
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                DisplayAlert("Notice", "Device Connected!", "OK");
                            });
                        else
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                DisplayAlert("Notice", "Device Disconnected!", "OK");
                            });
                        break;
                    case "isScanning":
                        if (ble.isScanning)
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                btnConnectBluetooth.Text = "Scanning... tap to stop";
                            });
                        }
                        else
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                btnConnectBluetooth.Text = "Tap to scan for devices";
                            });
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
            if(ble.isScanning)
            {
                ble.StopScan();
                //btnConnectBluetooth.Text = "Tap to scan for devices";
            }
            else
            {
                if(ble.isOn)
                {
                    ble.StartScan();
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        DisplayAlert("Notice", "Bluetooth is off! Please turn it on.", "OK");
                    });
                }
                //btnConnectBluetooth.Text = "Scanning... tap to stop";
            }
        }
    }
}
