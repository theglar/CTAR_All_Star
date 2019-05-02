using CTAR_All_Star.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CTAR_All_Star.Navigation
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePageDetail : ContentPage
    {
        public HomePageDetail()
        {
            InitializeComponent();
            Greeting();
        }

        public void Greeting()
        {
            string greeting = "Welcome " + App.currentUser.Username + ", to the CTAR All-Star application!";
            GreetingLabel.Text = greeting;           
        }

        public void Start_Button_Selected()
        {
            Navigation.PushAsync(new ManageExercise());                       
        }

        public async void Help_Button_Selected()
        {
            bool help = await DisplayAlert("Welcome " + App.currentUser.Username, "Push the Start button to begin exercising or choose another option in the navigation \"hamburger\" menu. There is also a Tutorials section for more detailed instructions.", "View Tutorials", "OK");
            if(help)
            {
                await Navigation.PushAsync(new TutorialsPage());
            }
        }
    }
}