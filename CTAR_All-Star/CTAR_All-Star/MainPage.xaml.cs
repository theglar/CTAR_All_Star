using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using CTAR_All_Star.Models;
using Plugin.BLE.Abstractions.Exceptions;


namespace CTAR_All_Star
{
    using Xamarin.Forms.PlatformConfiguration;

    public partial class MainPage : ContentPage
    {
        IBluetoothLE ble;
        IAdapter adapter;
        ObservableCollection<IDevice> deviceList;
        StackLayout availableDevices = new StackLayout();
        IDevice selectedDevice;
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
                DisplayAlert("Notice", $"Bluetooth: {e.NewState}", "OK");
            };
            adapter.ScanTimeoutElapsed += (s, e) =>
            {
                DisplayAlert("Notice", "timeout elapsed", "OK");
                btnConnectBluetooth.Text = "Tap to scan for devices";
            };
            adapter.DeviceConnected += (s, a) =>
            {
                btnConnectBluetooth.Text = "Tap to scan for devices";
                DisplayAlert("Notice", "Connected!", "OK");
            };
            adapter.DeviceDiscovered += (s, a) =>
            {
                deviceList.Add(a.Device);
            };
        }

        private void Signin_Activated(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SigninPage());
        }
        private void History_Activated(object sender, EventArgs e)
        {
            Navigation.PushAsync(new HistoryPage());
        }
        private void Graph_Activated(object sender, EventArgs e)
        {
            Navigation.PushAsync(new GraphPage());
        }
        private void Setup_Activated(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SetupPage());
        }
        private void Measure_Activated(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CreatePage());
        }
        private void Remove_Activated(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RemovePage());
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
                    await adapter.ConnectToDeviceAsync(selectedDevice);
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
                    await DisplayAlert("Notice", "Unknown exception!", "OK");
                }
            }
        }

        private async void OnButtonClicked(object sender, EventArgs args)
        {
            //Button button = (Button)sender;
            deviceList.Clear();

            if (!ble.Adapter.IsScanning)
            {
                btnConnectBluetooth.Text = "Scanning... tap to stop";
                adapter.ScanTimeout = 30000;
                await adapter.StartScanningForDevicesAsync();
            }
            else
            {
                btnConnectBluetooth.Text = "Tap to scan for devices";
                await adapter.StopScanningForDevicesAsync();
            }

        }

        //Needs work - I used online code that didn't work but pretty sure this will be usable when integrating the permissions plugin
        private async void GetPermissions(object sender, global::System.EventArgs e)
        {
            var myAction = await DisplayAlert("Permissions Required", "Please allow CTAR All-Star to access your location through your settings", "OK", "CANCEL");
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
                    DisplayAlert("Device", "You are not using an Android device", "YEP");
                }
            }
        }
    }
}
