using Plugin.BLE.Abstractions.Contracts;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CTAR_All_Star.Models;

namespace CTAR_All_Star.ViewModels
{
    class DeviceViewModel : INotifyPropertyChanged
    {
        private IDevice _nativeDevice;


        public event PropertyChangedEventHandler PropertyChanged;


        public IDevice NativeDevice
        {
            get
            {
                return _nativeDevice;
            }
            set
            {
                _nativeDevice = value;
                RaisePropertyChanged();
            }
        }
        protected void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(caller));
            }
        }
    }
}
