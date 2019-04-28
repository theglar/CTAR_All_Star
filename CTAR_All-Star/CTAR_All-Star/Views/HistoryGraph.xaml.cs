﻿using System;
using SQLite;

using Xamarin.Forms;
using CTAR_All_Star.Models;
using Syncfusion.SfChart.XForms;
using CTAR_All_Star.Database;
using System.Timers;
using System.Threading;
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

            PatientLabel.Text = App.PatientFilter;
            DateLabel.Text = App.DateFilter;
            SessionLabel.Text = App.SessionFilter;

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
    }
}