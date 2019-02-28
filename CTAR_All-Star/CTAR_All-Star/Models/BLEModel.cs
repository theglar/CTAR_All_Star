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
        private IBluetoothLE BLE;
        private IAdapter Adapter;
        public readonly ObservableCollection<IDevice> DeviceList;
        //StackLayout availaBLEDevices = new StackLayout();
        //private IDevice SelectedDevice;
        private IService DeviceService;
        private ICharacteristic PressureCharacteristic;
        private string PressureStr;
        private int PressureVal;

        public BLEModel()
        {
            //InitializeComponent();
            BLE = CrossBluetoothLE.Current;
            Adapter = CrossBluetoothLE.Current.Adapter;
            //var state = BLE.State;
            DeviceList = new ObservableCollection<IDevice>();
            //lv.ItemsSource = DeviceList;

            BLE.StateChanged += (s, e) =>
            {
                if (BLE.IsOn)
                {
                    //DisplayAlert("Notice", $"Bluetooth: {e.NewState}", "OK");
                }
            };
            Adapter.ScanTimeoutElapsed += (s, e) =>
            {
                //DisplayAlert("Notice", "timeout elapsed", "OK");
                //btnConnectBluetooth.Text = "Tap to scan for Devices";
                DeviceList.Clear();
            };

            Adapter.DeviceDiscovered += (s, a) =>
            {
                if (a.Device.Name != null && !DeviceList.Contains(a.Device))
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        DeviceList.Add(a.Device);
                    });
                }
            };
            Adapter.DeviceConnected += async (s, a) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    //DisplayAlert("Notice", "Connected!", "OK");
                });
                //btnConnectBluetooth.Text = "Tap to scan for Devices";
                DeviceList.Clear();
                DeviceService = await SelectedDevice.GetServiceAsync(Guid.Parse("0000ffe0-0000-1000-8000-00805f9b34fb"));
                PressureCharacteristic = await DeviceService.GetCharacteristicAsync(Guid.Parse("0000ffe1-0000-1000-8000-00805f9b34fb"));

                PressureCharacteristic.ValueUpdated += (o, args) =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        PressureStr = args.Characteristic.StringValue;
                        PressureVal = Convert.ToInt32(PressureStr);
                        //btnConnectBluetooth.Text = $"Value: {PressureVal}";
                    });
                };

                Device.BeginInvokeOnMainThread(() =>
                {
                    PressureCharacteristic.StartUpdatesAsync();
                    //System.Threading.Thread.Sleep(500);
                });
            };
        }

        public bool ConnectToDevice(IDevice SelectedDevice)
        {
            if (SelectedDevice == null)
            {
                //await DisplayAlert("Notice", "No Device selected", "OK");
                return false;
            }
            else
            {
                //SelectedDevice = lv.SelectedItem as IDevice;
                try
                {
                    //await DisplayAlert("Notice", "Connected!", "OK")
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Adapter.StopScanningForDevicesAsync();
                        Adapter.ConnectToDeviceAsync(SelectedDevice);
                    });
                    //btnConnectBluetooth.Text = "Tap to scan for Devices";
                }
                catch (DeviceConnectionException ex)
                {
                    //await DisplayAlert("Notice", "Error connecting to Device!", "OK");
                    return false;
                }
                catch (ArgumentNullException ex)
                {
                    //await DisplayAlert("Notice", "Selected Device is null!", "OK");
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

        public bool StartScan()
        {
            DeviceList.Clear();

            if (!BLE.Adapter.IsScanning)
            {
                if (BLE.IsOn)
                {
                    //btnConnectBluetooth.Text = "Scanning... tap to stop";
                    Adapter.ScanTimeout = 30000;
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Adapter.StartScanningForDevicesAsync();
                    });
                    return true;
                }
                else //bluetooth is off
                {
                    //await DisplayAlert("Notice", "Bluetooth is turned off. Please turn it on!", "OK");
                    return false;
                }
            }
            else //
            {
                return false;
            }
        }

        public bool StopScan()
        {
            DeviceList.Clear();

            if (BLE.Adapter.IsScanning)
            {
                //btnConnectBluetooth.Text = "Scanning... tap to stop";
                Device.BeginInvokeOnMainThread(() =>
                {
                    Adapter.StopScanningForDevicesAsync();
                });
                return true;
            }
            else
            {
                //await DisplayAlert("Notice", "Bluetooth is turned off. Please turn it on!", "OK");
                return false;
            }
        }

        public void ClearDeviceList()
        {
            DeviceList.Clear();
        }

        public bool IsScanning()
        {
            return BLE.Adapter.IsScanning;
        }

        public bool IsOn()
        {
            return BLE.IsOn;
        }

        public void SetScanTimeout(Int32 Duration)
        {
            Adapter.ScanTimeout = Duration;
        }

        public ObservableCollection<IDevice> GetDeviceList()
        {

        }
    }
}
