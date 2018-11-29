using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CTAR_All_Star
{
    public partial class MainPage : ContentPage
    {
        IBluetoothLE ble;
        IAdapter adapter;
        ObservableCollection<IDevice> deviceList;
        StackLayout availableDevices = new StackLayout();

        public MainPage()
        {
            InitializeComponent();
            ble = CrossBluetoothLE.Current;
            adapter = CrossBluetoothLE.Current.Adapter;
            deviceList = new ObservableCollection<IDevice>();
            //lv.ItemSource = deviceList;
            Button btnConnectBluetooth = new Button
            {
                Text = "No Bluetooth Device Connected",
                BackgroundColor = Color.Blue,
                TextColor = Color.White,
            };
            ListView lv = new ListView
            {
                ItemsSource = deviceList
            };
            btnConnectBluetooth.Clicked += OnButtonClicked;

            Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                Children =
                {
                    btnConnectBluetooth,
                    lv
                }
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<Measurement>();

                var measurements = conn.Table<Measurement>().ToList();
                measurementsView.ItemsSource = measurements;
            }
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

        private async void OnButtonClicked(object sender, EventArgs args)
        {
            //            string deviceName;
            Button button = (Button)sender;
            //            button.Text = $"Connecting...";
            //            NavigationPage newDeviceConnectionPage = new NavigationPage(new ConnectDevicePage());
            //            Navigation.PushAsync(newDeviceConnectionPage);
            //            //Navigation.PushAsync(new NavigationPage(new ConnectDevicePage()));
            //            MessagingCenter.Subscribe<ConnectDevicePage, string>(this, "Device Name", (s, e) =>
            //            {
            //                button.Text = $"Connected to {e}";
            //            });
            deviceList.Clear();

            adapter.DeviceDiscovered += (s, a) =>
            {
                deviceList.Add(a.Device);
            };
            button.Text = "Scanning...";
            if (!ble.Adapter.IsScanning)
            {
                await adapter.StartScanningForDevicesAsync();
            }

        }

    }
}
