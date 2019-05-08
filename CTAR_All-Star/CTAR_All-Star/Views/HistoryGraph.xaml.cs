using System;

using Xamarin.Forms;
using CTAR_All_Star.Models;
using System.Collections.Generic;

namespace CTAR_All_Star
{
    public partial class HistoryGraph : ContentPage
    {
        private Workout workout = new Workout();

        public HistoryGraph()
        {
           
            InitializeComponent();            
        }

        public HistoryGraph( List<Measurement> data)
        {
            InitializeComponent();

            SetLabels();            

            //Notify ViewModel of data
            Device.BeginInvokeOnMainThread(() => MessagingCenter.Send<HistoryGraph, List<Measurement>>(this, "databaseChange", data));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if(App.DateFilter.Equals(String.Empty) || App.PatientFilter.Equals(String.Empty) || App.SessionFilter.Equals(String.Empty))
            {
                Instructions();
            }
            
        }

        public async void Instructions()
        {
            bool show = await DisplayAlert("Graph View", "For best results, set all filters before viewing the graph. If no filters are selected the graph will be blank." +
                " Additionally, turn phone to landscape for best graph view.", "OK", "Go Back");
            if(!show)
            {
                Navigation.PopAsync();
            }
        }

        private void SetLabels()
        {
            if (App.PatientFilter.Equals(String.Empty))
            {
                PatientLabel.Text = "All Patients";
            }
            else
            {
                PatientLabel.Text = App.PatientFilter;
            }

            if (App.DateFilter.Equals(String.Empty))
            {
                DateLabel.Text = "All Dates";
            }
            else
            {
                DateLabel.Text = App.DateFilter;
            }

            if (App.SessionFilter.Equals(String.Empty))
            {
                SessionLabel.Text = "All Sessions";
            }
            else
            {
                SessionLabel.Text = "Session #" + App.SessionFilter;
            }
        }
    }
}