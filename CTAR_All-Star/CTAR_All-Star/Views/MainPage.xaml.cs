using Plugin.BLE;
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
using Plugin.BLE.Abstractions.Exceptions;
using System.Diagnostics;
using Xamarin.Forms.PlatformConfiguration;


namespace CTAR_All_Star
{
    public partial class MainPage : ContentPage
    {
        IBluetoothLE ble;
        IAdapter adapter;
        ObservableCollection<IDevice> deviceList;
        StackLayout availableDevices = new StackLayout();
        IDevice selectedDevice;
        IList<IService> deviceServices;
        IList<ICharacteristic> serviceCharacteristics;
        ICharacteristic reportCharacteristic;
        //Button button = btnConnectBluetooth;

        public MainPage()
        {
            InitializeComponent();
            ble = CrossBluetoothLE.Current;
            adapter = CrossBluetoothLE.Current.Adapter;
            var state = ble.State;
            deviceList = new ObservableCollection<IDevice>();
            lv.ItemsSource = deviceList;

            ble.StateChanged += (s, e) =>
            {
                if (ble.IsOn)
                {
                    DisplayAlert("Notice", $"Bluetooth: {e.NewState}", "OK");
                }
            };
            adapter.ScanTimeoutElapsed += (s, e) =>
            {
                DisplayAlert("Notice", "timeout elapsed", "OK");
                btnConnectBluetooth.Text = "Tap to scan for devices";
                deviceList.Clear();
            };

            adapter.DeviceDiscovered += (s, a) =>
            {
                if (a.Device.Name != null && !deviceList.Contains(a.Device))
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        deviceList.Add(a.Device);
                    });
                    int size = deviceList.Count;
                    //Debug.WriteLine(size);
                }
            };
            adapter.DeviceConnected += async (s, a) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    DisplayAlert("Notice", "Connected!", "OK");
                });
                btnConnectBluetooth.Text = "Tap to scan for devices";
                deviceList.Clear();
                deviceServices = await selectedDevice.GetServicesAsync();
                int numServices = deviceServices.Count;
                for(int i = 0; i < numServices; i++)
                {
                    Debug.WriteLine("Service: " + deviceServices[i].Name);
                    serviceCharacteristics = await deviceServices[i].GetCharacteristicsAsync();
                    int numCharacteristics = serviceCharacteristics.Count;
                    for (int j = 0; j < numCharacteristics; j++)
                    {
                        Debug.WriteLine("Characteristic: " + serviceCharacteristics[j].Name);
                        if (serviceCharacteristics[j].Name == "Report")
                        {
                            reportCharacteristic = serviceCharacteristics[j];
                            Debug.WriteLine("CanRead:" + reportCharacteristic.CanRead);
                            reportCharacteristic.ValueUpdated += (o, args) =>
                            {
                                //var bytes = args.Characteristic.Value;
                                //Debug.WriteLine(args.Characteristic.Value);
                                //Device.BeginInvokeOnMainThread(() =>
                                //{
                                //    byte[] charVal = reportCharacteristic.ReadAsync();
                                //});
                                //Debug.WriteLine(reportCharacteristic.Value);
                                Debug.WriteLine("Received update");
                            };

                            Device.BeginInvokeOnMainThread(() =>
                            {
                                reportCharacteristic.StartUpdatesAsync();
                                System.Threading.Thread.Sleep(5000);
                            });
                            //while (true)
                            //{
                            //    Device.BeginInvokeOnMainThread(() =>
                            //    {
                            //        reportCharacteristic.ReadAsync();
                            //    });
                            //}
                        }
                    }
                }
                //deviceCharacteristics = await service.getCharacteristicsAsync();
            };
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
                try
                {
                    //await DisplayAlert("Notice", "Connected!", "OK")
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        adapter.StopScanningForDevicesAsync();
                        adapter.ConnectToDeviceAsync(selectedDevice);
                    });
                    btnConnectBluetooth.Text = "Tap to scan for devices";
                }
                catch (DeviceConnectionException ex)
                {
                    await DisplayAlert("Notice", "Error connecting to device!", "OK");
                }
                catch (ArgumentNullException ex)
                {
                    await DisplayAlert("Notice", "Selected device is null!", "OK");
                }
                catch (Exception ex)
                {
                    await DisplayAlert("notice", "unknown exception!", "ok");
                }
            }
        }

        private async void OnScanClicked(object sender, EventArgs args)
        {
            //Button button = (Button)sender;
            deviceList.Clear();

            if (!ble.Adapter.IsScanning)
            {
                if (ble.IsOn)
                {
                    btnConnectBluetooth.Text = "Scanning... tap to stop";
                    adapter.ScanTimeout = 30000;
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        adapter.StartScanningForDevicesAsync();
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
                    adapter.StopScanningForDevicesAsync();
                });
            }
        }


        //Needs work - I used online code that didn't work but pretty sure this will be usable when integrating the permissions plugin
        private async void GetPermissions(object sender, global::System.EventArgs e)
        {
            var myAction = await DisplayAlert("Permissions Required", "This will eventually setup location permissions through our app.", "OK", "CANCEL");
            if (myAction)
            {
                if (Device.RuntimePlatform == global::Xamarin.Forms.Device.Android)
                {

                    //DependencyService.Get<ISettingsService>().OpenSettings();
                    //global::Xamarin.Forms.DependencyService.Get<global::CTAR_All_Star.PermissionsInterface>().OpenSettings();

                    btnPermissions.IsVisible = false;
                }
                else
                {
                    await DisplayAlert("Device", "You are not using an Android device", "YEP");
                }
            }
        }
    }
}
