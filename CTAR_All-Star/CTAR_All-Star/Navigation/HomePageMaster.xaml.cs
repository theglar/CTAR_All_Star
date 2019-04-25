﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CTAR_All_Star.Views;

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
                if(App.currentUser.userType.Equals("Doctor"))
                {
                    MenuItems = new ObservableCollection<HomePageMenuItem>(new[]
                    {
                        new HomePageMenuItem { Id = 0, Title = "Home", TargetType = typeof(HomePageDetail) },
                        new HomePageMenuItem { Id = 1, Title = "Tutorials", TargetType = typeof(TutorialsPage) },
                        new HomePageMenuItem { Id = 2, Title = "Bluetooth", TargetType = typeof(MainPage) },
                        new HomePageMenuItem { Id = 3, Title = "Create Exercise", TargetType = typeof(CreateExercise)  },
                        new HomePageMenuItem { Id = 4, Title = "Manage Exercises", TargetType = typeof(ManageExercise)  },
                        new HomePageMenuItem { Id = 5, Title = "Manage Patients", TargetType = typeof(ManagePatients)  },
                        new HomePageMenuItem { Id = 6, Title = "History", TargetType = typeof(HistoryPage)  },
                        new HomePageMenuItem { Id = 7, Title = "Exercise" , TargetType = typeof(GraphPage) },
                        new HomePageMenuItem { Id = 8, Title = "Settings", TargetType = typeof(SetupPage)  },
                        //new HomePageMenuItem { Id = 9, Title = "Add Measurement", TargetType = typeof(CreatePage)  },
                        new HomePageMenuItem { Id = 9, Title = "Clear Database", TargetType = typeof(RemovePage)  },
                        new HomePageMenuItem { Id = 10, Title = "Logout", TargetType = typeof(LogoutPage) },
                        new HomePageMenuItem { Id = 11, Title = "History by Date", TargetType = typeof(HistoryDatesListPage)}
                    });
                }
                //Patient is the default
                else
                {
                    MenuItems = new ObservableCollection<HomePageMenuItem>(new[]
                    {
                        new HomePageMenuItem { Id = 0, Title = "Home", TargetType = typeof(HomePageDetail) },
                        new HomePageMenuItem { Id = 1, Title = "Tutorials", TargetType = typeof(TutorialsPage) },
                        new HomePageMenuItem { Id = 2, Title = "Bluetooth", TargetType = typeof(MainPage) },
                        new HomePageMenuItem { Id = 3, Title = "Choose Exercise", TargetType = typeof(ManageExercise)  },                        
                        new HomePageMenuItem { Id = 4, Title = "History", TargetType = typeof(HistoryPage)  },
                        new HomePageMenuItem { Id = 5, Title = "Exercise" , TargetType = typeof(GraphPage) },
                        new HomePageMenuItem { Id = 6, Title = "Settings", TargetType = typeof(SetupPage)  },
                        new HomePageMenuItem { Id = 7, Title = "Logout", TargetType = typeof(LogoutPage) },
                    });
                }                
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