using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace CTAR_All_Star.Models
{
    class BLEModel
    {
        IBluetoothLE ble;
        IAdapter adapter;
        ObservableCollection<IDevice> deviceList;
        //StackLayout availableDevices = new StackLayout();
        IDevice selectedDevice;
        IService deviceService;
        ICharacteristic pressureCharacteristic;
        string pressureStr;
        int pressureVal;

        public BLEModel()
        {
            //InitializeComponent();
            ble = CrossBluetoothLE.Current;
            adapter = CrossBluetoothLE.Current.Adapter;
            var state = ble.State;
            deviceList = new ObservableCollection<IDevice>();
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
                    //int size = deviceList.Count;
                    //Debug.WriteLine(size);
                }
            };
            adapter.DeviceConnected += async (s, a) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    //DisplayAlert("Notice", "Connected!", "OK");
                });
                //btnConnectBluetooth.Text = "Tap to scan for devices";
                deviceList.Clear();
                deviceService = await selectedDevice.GetServiceAsync(Guid.Parse("0000ffe0-0000-1000-8000-00805f9b34fb"));
                pressureCharacteristic = await deviceService.GetCharacteristicAsync(Guid.Parse("0000ffe1-0000-1000-8000-00805f9b34fb"));

                pressureCharacteristic.ValueUpdated += (o, args) =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        pressureStr = args.Characteristic.StringValue;
                        pressureVal = Convert.ToInt32(pressureStr);
                        //btnConnectBluetooth.Text = $"Value: {pressureVal}";
                    });
                };

                Device.BeginInvokeOnMainThread(() =>
                {
                    pressureCharacteristic.StartUpdatesAsync();
                    //System.Threading.Thread.Sleep(500);
                });
            };
        }

        public bool ConnectToDevice(IDevice device)
        {
            if (device == null)
            {
                //await DisplayAlert("Notice", "No Device selected", "OK");
                return false;
            }
            else
            {
                //selectedDevice = lv.SelectedItem as IDevice;
                try
                {
                    //await DisplayAlert("Notice", "Connected!", "OK")
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        adapter.StopScanningForDevicesAsync();
                        adapter.ConnectToDeviceAsync(device);
                    });
                    //btnConnectBluetooth.Text = "Tap to scan for devices";
                }
                catch (DeviceConnectionException ex)
                {
                    //await DisplayAlert("Notice", "Error connecting to device!", "OK");
                    return false;
                }
                catch (ArgumentNullException ex)
                {
                    //await DisplayAlert("Notice", "Selected device is null!", "OK");
                    return false;
                }
                catch (Exception ex)
                {
                    //await DisplayAlert("notice", "unknown exception!", "ok");
                    return false;
                }
                return true;
            }
        }

        public bool BeginScanning()
        {
            deviceList.Clear();

            if (!ble.Adapter.IsScanning)
            {
                if (ble.IsOn)
                {
                    //btnConnectBluetooth.Text = "Scanning... tap to stop";
                    adapter.ScanTimeout = 30000;
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        adapter.StartScanningForDevicesAsync();
                    });
                    return true;
                }
                else
                {
                    //await DisplayAlert("Notice", "Bluetooth is turned off. Please turn it on!", "OK");
                    return false;
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

        public bool StopScanning()
        {
            deviceList.Clear();

            if (ble.Adapter.IsScanning)
            {
                //btnConnectBluetooth.Text = "Scanning... tap to stop";
                Device.BeginInvokeOnMainThread(() =>
                {
                    adapter.StopScanningForDevicesAsync();
                });
                return true;
            }
            else
            {
                //await DisplayAlert("Notice", "Bluetooth is turned off. Please turn it on!", "OK");
                return false;
            }
        }
    }
}
