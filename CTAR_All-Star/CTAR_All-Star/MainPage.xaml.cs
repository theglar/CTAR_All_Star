﻿using Plugin.BLE;
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
                if (ble.IsOn)
                {
                    DisplayAlert("Notice", $"Bluetooth: {e.NewState}", "OK");
                }
                //if (!ble.IsOn)
                //{
                //    DisplayAlert("Notice", $"Bluetooth: {e.NewState}", "OK");
                //}
            };
            adapter.ScanTimeoutElapsed += (s, e) =>
            {
                DisplayAlert("Notice", "timeout elapsed", "OK");
                btnConnectBluetooth.Text = "Tap to scan for devices";
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
            adapter.DeviceConnected += (s, a) =>
            {
                DisplayAlert("Notice", "Connected!", "OK");
                btnConnectBluetooth.Text = "Tap to scan for devices";
            };
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
                    await DisplayAlert("Notice", "Connected!", "OK");
                    btnConnectBluetooth.Text = "Tap to scan for devices";
                    deviceList.Clear();
                    await adapter.StopScanningForDevicesAsync();
                //await adapter.ConnectToDeviceAsync(selectedDevice);
                //}
                //catch (DeviceConnectionException ex)
                //{
                //    await DisplayAlert("Notice", "Error connecting to device!", "OK");
                //}
                //catch (ArgumentNullException ex)
                //{
                //    await DisplayAlert("Notice", "Selected device is null!", "OK");
                //}
                //catch (exception ex)
                //{
                //    await displayalert("notice", "unknown exception!", "ok");
                //}
            }
        }

        private async void OnScanClicked(object sender, EventArgs args)
        {
            //Button button = (Button)sender;
            deviceList.Clear();

            if (!ble.Adapter.IsScanning)
            {
                if(ble.IsOn)
                {
                    btnConnectBluetooth.Text = "Scanning... tap to stop";
                    adapter.ScanTimeout = 30000;
                    await adapter.StartScanningForDevicesAsync();
                }
                else
                {
                    await DisplayAlert("Notice", "Bluetooth is turned off. Please turn it on!", "OK");
                }
            }
            else
            {
                btnConnectBluetooth.Text = "Tap to scan for devices";
                await adapter.StopScanningForDevicesAsync();
            }

        }
    }
}
