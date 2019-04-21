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
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using CTAR_All_Star.Database;

namespace CTAR_All_Star
{
    public partial class BLEViewModel
    {
        private IBluetoothLE ble;
        private IAdapter adapter;
        private ObservableCollection<IDevice> deviceList;
        public ReadOnlyObservableCollection<IDevice> deviceListReadOnly;
        //StackLayout availableDevices = new StackLayout();
        IDevice selectedDevice;
        IService deviceService;
        ICharacteristic pressureCharacteristic;
        string pressureStr;
        int pressureVal;

        public BLEViewModel()
        {
            ble = CrossBluetoothLE.Current;
            adapter = CrossBluetoothLE.Current.Adapter;
            var state = ble.State;
            deviceList = new ObservableCollection<IDevice>();
            deviceListReadOnly = new ReadOnlyObservableCollection<IDevice>(deviceList);
            //lv.ItemsSource = deviceList;

            ble.StateChanged += (s, e) =>
            {
                if (ble.IsOn)
                {
                    //DisplayAlert("Notice", $"Bluetooth: {e.NewState}", "OK");
                }
            };
            adapter.ScanTimeoutElapsed += (s, e) =>
            {
                //DisplayAlert("Notice", "timeout elapsed", "OK");
                //btnConnectBluetooth.Text = "Tap to scan for devices";
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
                }
            };
            adapter.DeviceConnected += async (s, a) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    //DisplayAlert("Notice", "Connected!", "OK");
                });
                App.currentUser.DeviceIsConnected = true;
                //btnConnectBluetooth.Text = "Tap to scan for devices";
                deviceList.Clear();
                deviceService = await selectedDevice.GetServiceAsync(Guid.Parse("0000ffe0-0000-1000-8000-00805f9b34fb"));
                pressureCharacteristic = await deviceService.GetCharacteristicAsync(Guid.Parse("0000ffe1-0000-1000-8000-00805f9b34fb"));

                pressureCharacteristic.ValueUpdated += (o, args) =>
                {
                    DatabaseHelper dbHelper = new DatabaseHelper();
                    Measurement measurement = new Measurement();

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        pressureStr = args.Characteristic.StringValue;
                        pressureVal = Convert.ToInt32(pressureStr);
                        //btnConnectBluetooth.Text = $"Value: {pressureVal}";
                    });

                    // Get current date and time
                    DateTime d = DateTime.Now;
                    DateTime dt = DateTime.Parse(d.ToString());
                    measurement.UserName = App.currentUser.Username;
                    measurement.SessionNumber = App.currentUser.Session.ToString();
                    measurement.TimeStamp = d;
                    measurement.Pressure = pressureVal;
                    measurement.Duration = "1"; //Do we need this?
                    measurement.DisplayTime = dt.ToString("HH:mm:ss");

                    dbHelper.addData(measurement);

                };
            };
        }

        public async void StartUpdates()
        {
            //Device.BeginInvokeOnMainThread(() =>
            //{
                await pressureCharacteristic.StartUpdatesAsync();
            //    //System.Threading.Thread.Sleep(500);
            //});
        }

        public async void StopUpdates()
        {
            //Device.BeginInvokeOnMainThread(() =>
            //{
            await pressureCharacteristic.StopUpdatesAsync();
            //    //System.Threading.Thread.Sleep(500);
            //});
        }

        public async void ConnectToDevice(IDevice selectedDevice)
        {
            try
            {
                //await DisplayAlert("Notice", "Connected!", "OK")
                await adapter.StopScanningForDevicesAsync();
                await adapter.ConnectToDeviceAsync(selectedDevice);
                //btnConnectBluetooth.Text = "Tap to scan for devices";
            }
            catch (DeviceConnectionException ex)
            {
                //await DisplayAlert("Notice", "Error connecting to device!", "OK");
            }
            catch (ArgumentNullException ex)
            {
                //await DisplayAlert("Notice", "Selected device is null!", "OK");
            }
            catch (Exception ex)
            {
                //await DisplayAlert("notice", "unknown exception!", "ok");
            }
        }

        public async void StartScan()
        {
            //Button button = (Button)sender;
            deviceList.Clear();

            if (!ble.Adapter.IsScanning)
            {
                if (!ble.IsOn)
                {
                    //await DisplayAlert("Notice", "Bluetooth is turned off. Please turn it on!", "OK");
                }
                else
                {
                    //btnConnectBluetooth.Text = "Scanning... tap to stop";
                    adapter.ScanTimeout = 30000;

                    await adapter.StartScanningForDevicesAsync();
                }
            }
        }

        public async void StopScan()
        {
            if(ble.Adapter.IsScanning)
            {
                await adapter.StopScanningForDevicesAsync();
            }
            else
            {
                //await DisplayAlert("Notice", "Not Currently scanning", "OK");
            }
        }
    }
}
