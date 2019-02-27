using CTAR_All_Star.Models;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CTAR_All_Star.ViewModels
{
    class BLEViewModel
    {
        private BLEModel BLE;

        public BLEViewModel()
        {
            BLE = new BLEModel;
        }

        public bool StopScan()
        {
            return BLE.StopScan();
        }

        public bool StartScan()
        {
            return BLE.StartScan();
        }

        public bool ConnectToDevice(IDevice SelectedDevice)
        {
            return BLE.ConnectToDevice(SelectedDevice);
        }

        public void ClearDeviceList()
        {
            BLE.ClearDeviceList();
        }

        public bool IsScanning()
        {
            return BLE.IsScanning();
        }

        public bool IsOn()
        {
            return BLE.IsOn();
        }

        public void SetScanTimeout(Int32 Duration)
        {
            BLE.SetScanTimeout(Duration);
        }

        public ObservableCollection<IDevice> GetDeviceList()
        {
            return BLE.GetDeviceList();
        }
    }
}
