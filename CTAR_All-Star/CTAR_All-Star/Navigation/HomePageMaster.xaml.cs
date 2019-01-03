using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CTAR_All_Star.Navigation
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePageMaster : ContentPage
    {
        public ListView ListView;

        public HomePageMaster()
        {
            InitializeComponent();

            BindingContext = new HomePageMasterViewModel();
            ListView = MenuItemsListView;
        }

        class HomePageMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<HomePageMenuItem> MenuItems { get; set; }
            
            public HomePageMasterViewModel()
            {
                MenuItems = new ObservableCollection<HomePageMenuItem>(new[]
                {
                    new HomePageMenuItem { Id = 0, Title = "Home"},
                    new HomePageMenuItem { Id = 1, Title = "Login" },
                    new HomePageMenuItem { Id = 2, Title = "Create Exercise" },
                    new HomePageMenuItem { Id = 3, Title = "Choose Exercise" },
                    new HomePageMenuItem { Id = 4, Title = "Manage Patients" },
                    new HomePageMenuItem { Id = 4, Title = "History" },
                    new HomePageMenuItem { Id = 4, Title = "Graph" },
                    new HomePageMenuItem { Id = 4, Title = "Settings" },
                    new HomePageMenuItem { Id = 4, Title = "Add Measurment" },
                    new HomePageMenuItem { Id = 4, Title = "Clear Database" },
                });
            }
            
            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}