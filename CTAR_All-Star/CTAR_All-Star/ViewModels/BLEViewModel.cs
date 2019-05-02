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
using System.ComponentModel;

namespace CTAR_All_Star
{
    public partial class BLEViewModel : INotifyPropertyChanged
    {
        private IBluetoothLE ble;
        private IAdapter adapter;
        private ObservableCollection<IDevice> deviceList;
        private IDevice device;
        private IService deviceService;
        private ICharacteristic pressureCharacteristic;
        public ReadOnlyObservableCollection<IDevice> deviceListReadOnly;
        public string pressureStr { get; private set; }
        public int pressureVal { get; private set; }
        public bool isOn { get; private set; }
        public bool deviceConnected { get; private set; }
        public bool isScanning { get; private set; }
        public event PropertyChangedEventHandler PropertyChanged;
        //public event EventHandler StateChanged;

        public BLEViewModel()
        {
            ble = CrossBluetoothLE.Current;
            adapter = CrossBluetoothLE.Current.Adapter;
            isOn = ble.IsOn;
            deviceConnected = false;
            isScanning = false; // adapter.IsScanning;
            deviceList = new ObservableCollection<IDevice>();
            deviceListReadOnly = new ReadOnlyObservableCollection<IDevice>(deviceList);
            //lv.ItemsSource = deviceList;

            ble.StateChanged += (s, e) =>
            {
                if(ble.State == BluetoothState.Off)
                {
                    isOn = false;
                    OnPropertyChanged("state");
                }
                else if(ble.State == BluetoothState.On)
                {
                    isOn = true;
                    OnPropertyChanged("state");
                }
            };
            adapter.ScanTimeoutElapsed += (s, e) =>
            {
                deviceList.Clear();
                //isScanning = adapter.IsScanning;
                isScanning = false;
                OnPropertyChanged("scanTimeoutElapsed");
                OnPropertyChanged("isScanning");
            };
            adapter.DeviceDiscovered += (s, a) =>
            {
                if (a.Device.Name != null && a.Device.Name.StartsWith("CTAR") && !deviceList.Contains(a.Device))
                {
                    deviceList.Add(a.Device);
                }
            };
            adapter.DeviceConnected += async (s, a) =>
            {
                deviceConnected = true;
                //App.currentUser.DeviceIsConnected = true;
                deviceList.Clear();
                deviceService = await device.GetServiceAsync(Guid.Parse("0000ffe0-0000-1000-8000-00805f9b34fb"));
                pressureCharacteristic = await deviceService.GetCharacteristicAsync(Guid.Parse("0000ffe1-0000-1000-8000-00805f9b34fb"));

                pressureCharacteristic.ValueUpdated += (o, args) =>
                {
                    //DatabaseHelper dbHelper = new DatabaseHelper();
                    //Measurement measurement = new Measurement();

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        pressureStr = args.Characteristic.StringValue;
                        pressureVal = Convert.ToInt32(pressureStr);
                    });

                    //// Get current date and time
                    //DateTime d = DateTime.Now;
                    //DateTime dt = DateTime.Parse(d.ToString());
                    //measurement.UserName = App.currentUser.Username;
                    //measurement.SessionNumber = App.currentUser.Session.ToString();
                    //measurement.TimeStamp = d;
                    //measurement.Pressure = pressureVal;
                    //measurement.Duration = "1"; //Do we need this?
                    //measurement.DisplayTime = dt.ToString("HH:mm:ss");

                    //dbHelper.addData(measurement);
                    OnPropertyChanged("pressure");
                };
                //StartUpdates();
                OnPropertyChanged("deviceConnected");
            };
            adapter.DeviceConnectionLost += (s, e) =>
            {
                deviceConnected = false;
                device = null;
                OnPropertyChanged("deviceConnected");
            };
            adapter.DeviceDisconnected += (s, e) =>
            {
                deviceConnected = false;
                device = null;
                OnPropertyChanged("deviceConnected");
            };
        }

        public async void StartUpdates()
        {
            if (deviceConnected)
            {
                try
                {
                    //Device.BeginInvokeOnMainThread(async() =>
                    //{
                        await pressureCharacteristic.StartUpdatesAsync();
                    //});

                }
                catch(InvalidOperationException)
                {

                }
                catch(Exception)
                {

                }
            }
        }

        public async void StopUpdates()
        {
            await pressureCharacteristic.StopUpdatesAsync();
        }

        public async void ConnectToDevice(IDevice selectedDevice)
        {
            try
            {
                //await DisplayAlert("Notice", "Connected!", "OK")
                device = selectedDevice;
                await adapter.StopScanningForDevicesAsync();
                await adapter.ConnectToDeviceAsync(device);
                isScanning = false;
                OnPropertyChanged("isScanning");
                //btnConnectBluetooth.Text = "Tap to scan for devices";
            }
            catch (DeviceConnectionException)
            {
                //await DisplayAlert("Notice", "Error connecting to device!", "OK");
                device = null;
                return;
            }
            catch (ArgumentNullException)
            {
                //await DisplayAlert("Notice", "Selected device is null!", "OK");
                device = null;
                return;
            }
            catch (Exception)
            {
                //await DisplayAlert("notice", "unknown exception!", "ok");
                device = null;
                return;
            }
        }

        public async void DisconnectDevice()
        {
            //Device.BeginInvokeOnMainThread(async () =>
            //{
                //StopUpdates();
                await adapter.DisconnectDeviceAsync(device);
            //});
        }

        public bool ToggleScan()
        {
            if(isScanning)
            {
                StopScan();
            }
            else
            {
                StartScan();
            }
            return isScanning;
        }

        public void StartScan()
        {
            deviceList.Clear();
            if(ble.IsOn)
            {
                adapter.ScanTimeout = 15000;
                Device.BeginInvokeOnMainThread(() =>
                {
                    adapter.StartScanningForDevicesAsync();
                });
                //isScanning = adapter.IsScanning;
                isScanning = true;
                OnPropertyChanged("isScanning");
            }
        }

        public void StopScan()
        {
            deviceList.Clear();
            Device.BeginInvokeOnMainThread(() =>
            {
                adapter.StopScanningForDevicesAsync();
            });
            //isScanning = adapter.IsScanning;
            isScanning = false;
            OnPropertyChanged("isScanning");
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //protected virtual void OnStateChanged(EventArgs e)
        //{
        //    EventHandler handler = StateChanged;
        //    handler?.Invoke(this, e);
        //}
    }
}
