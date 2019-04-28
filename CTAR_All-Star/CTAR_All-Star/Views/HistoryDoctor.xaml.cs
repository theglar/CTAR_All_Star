using System;
using System.Collections.Generic;
using System.Linq;
using Android.Util;
using CTAR_All_Star.Models;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CTAR_All_Star
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HistoryDoctor : ContentPage
	{
        private Picker namePicker, sessionPicker, datePicker;
        private List<String> nameList, sessionList, dateList;
        private List<Measurement> filteredList;

        public HistoryDoctor()
		{
			InitializeComponent();
            InitializeLists();
            InititalizePickerListeners();

            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<Measurement>();

                //Set up source items for the View model
                var measurements = conn.Table<Measurement>().ToList();
                if (measurements != null)
                {
                    measurementsView.ItemsSource = measurements;
                }

                //Load the picker items - this should probably be done in the constructor only
                var table = conn.Table<Measurement>();
                if (table != null)
                {
                    foreach (var m in table)
                    {
                        if (!nameList.Contains(m.UserName) && m.UserName != null)
                        {
                            nameList.Add(m.UserName);
                        }
                        if (!sessionList.Contains(m.SessionNumber) && m.SessionNumber != null)
                        {
                            sessionList.Add(m.SessionNumber);
                        }
                        if (!dateList.Contains(m.DisplayDate) && m.DisplayDate != null)
                        {
                            dateList.Add(m.DisplayDate);
                        }
                    }
                }
            }
            Test();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            InitializePickers();            

            LoadPickers();
        } 
        
        public void InitializeLists()
        {
            nameList = new List<String>();
            sessionList = new List<String>();
            dateList = new List<String>();
        }

        public void InitializePickers()
        {
            namePicker = NamePicker;
            sessionPicker = SessionPicker;
            datePicker = DatePicker;
        }

        public void LoadPickers()
        {
            if (nameList != null)
            {
                namePicker.Items.Clear();
                foreach (var n in nameList)
                {
                    namePicker.Items.Add(n);
                }
                namePicker.Items.Add("All");
            }

            if (sessionList != null)
            {
                sessionPicker.Items.Clear();
                foreach (var n in sessionList)
                {
                    sessionPicker.Items.Add(n);
                }
                sessionPicker.Items.Add("All");
            }

            if (dateList != null)
            {
                datePicker.Items.Clear();
                foreach (var n in dateList)
                {
                    datePicker.Items.Add(n);
                }
                datePicker.Items.Add("All");
            }
        }

        void InititalizePickerListeners()
        {
            NamePicker.SelectedIndexChanged += this.NamePickerIndexChanged;
            SessionPicker.SelectedIndexChanged += this.SessionPickerIndexChanged;
            DatePicker.SelectedIndexChanged += this.DatePickerIndexChanged;
        }

        public void NamePickerIndexChanged(object sender, EventArgs e)
        {
            FilterData();
        }

        public void SessionPickerIndexChanged(object sender, EventArgs e)
        {
            FilterData();
        }

        public void DatePickerIndexChanged(object sender, EventArgs e)
        {
            FilterData();
        }

        public void FilterData()
        {
            //Pull all data from the database and make a list
            filteredList = new List<Measurement>();

            //Clear globals
            App.PatientFilter = "";
            App.DateFilter = "";
            App.SessionFilter = "";

            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<Measurement>();
                var measurements = conn.Table<Measurement>().ToList();
                if (measurements != null)
                {
                    //Load our list to filter
                    foreach (var m in measurements)
                    {
                        filteredList.Add(m);
                    }
                }
            }

            if(filteredList != null)
            {
                var tempList = new List<Measurement>();

                //Check Patient Filter
                if(NamePicker.SelectedItem != null)
                {
                    if (!NamePicker.SelectedItem.Equals("All"))
                    {
                        foreach (var m in filteredList)
                        {
                            if (m.UserName.Equals(NamePicker.SelectedItem))
                            {
                                tempList.Add(m);
                            }
                        }
                        filteredList.Clear();
                        foreach(var m in tempList)
                        {
                            filteredList.Add(m);
                        }                        
                        tempList.Clear();

                        App.PatientFilter = NamePicker.SelectedItem.ToString();
                    }
                }
                

                //Check Date Filter
                if(DatePicker.SelectedItem != null)
                {
                    if (!DatePicker.SelectedItem.Equals("All"))
                    {
                        foreach (var m in filteredList)
                        {
                            if (m.DisplayDate.Equals(datePicker.SelectedItem) || NamePicker.SelectedItem.Equals(null))
                            {
                                tempList.Add(m);
                            }
                        }
                        filteredList.Clear();
                        foreach (var m in tempList)
                        {
                            filteredList.Add(m);
                        }
                        tempList.Clear();
                        App.DateFilter = DatePicker.SelectedItem.ToString();
                    }
                }
                

                //Check Session Filter
                if(SessionPicker.SelectedItem != null)
                {
                    if (!SessionPicker.SelectedItem.Equals("All"))
                    {
                        foreach (var m in filteredList)
                        {
                            if (m.SessionNumber.Equals(SessionPicker.SelectedItem))
                            {
                                tempList.Add(m);
                            }
                        }
                        filteredList.Clear();
                        foreach (var m in tempList)
                        {
                            filteredList.Add(m);
                        }
                        tempList.Clear();
                        App.SessionFilter = "Session #" + SessionPicker.SelectedItem.ToString();
                    }
                }
                
            }            

            //Send filtered list to the view
            measurementsView.ItemsSource = filteredList;
        }

        public void Test()
        {
            Log.Debug("tag", "in test");
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                string filter = "UserName = 'jdj'";
                string filter2 = "SessionNumber = '9'";
                string filter3 = "DocID = 'Not'";
                conn.CreateTable<Measurement>();
                var measurement = conn.Query<Measurement>("select * from Measurement where " + filter + " and " + filter2 + " and " + filter3).SingleOrDefault();
                if (measurement != null)
                {
                    Log.Debug("tag", "m != null");
                }
                else
                {
                    Log.Debug("tag", "m == null");
                }
            }
        }

        public void View_Graph()
        {
            Navigation.PushAsync(new HistoryGraph(filteredList));
        }
    }
}