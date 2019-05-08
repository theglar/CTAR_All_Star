using System;
using System.Collections.Generic;
using System.Linq;
//using Android.Util;
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
        private string SQLcommand = "";

        public HistoryDoctor()
		{
			InitializeComponent();
            InitializeLists();
            InititalizePickerListeners();

            //Clear globals
            App.PatientFilter = "";
            App.DateFilter = "";
            App.SessionFilter = "";

            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<Measurement>();

                //Set up source items for the initial View model
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
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            InitializePickers(); 
            LoadPickers();
            ResetTable();
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
            if(NamePicker.SelectedItem != null && NamePicker.SelectedItem.ToString() != "All")
            {
                App.PatientFilter = NamePicker.SelectedItem.ToString();
            }
            FilterData();
        }

        public void SessionPickerIndexChanged(object sender, EventArgs e)
        {
            if (SessionPicker.SelectedItem != null && SessionPicker.SelectedItem.ToString() != "All")
            {
                App.SessionFilter = SessionPicker.SelectedItem.ToString();
            }
            FilterData();
        }

        public void DatePickerIndexChanged(object sender, EventArgs e)
        {
            if (DatePicker.SelectedItem != null && DatePicker.SelectedItem.ToString() != "All")
            {
                App.DateFilter = DatePicker.SelectedItem.ToString();
            }
            FilterData();
        }

        public void FilterData()
        {            
            if(!App.PatientFilter.Equals(String.Empty) || !App.DateFilter.Equals(String.Empty) || !App.SessionFilter.Equals(String.Empty))
            {
                //Create database query command
                SQLcommand = "select * from Measurement where ";
                bool firstFilter = true;

                //Check patient filter
                if (!App.PatientFilter.Equals(String.Empty) && firstFilter)
                {
                    SQLcommand = SQLcommand + "UserName = '" + App.PatientFilter + "'";
                    firstFilter = false;
                }
                else if (!App.PatientFilter.Equals(String.Empty))
                {
                    SQLcommand = SQLcommand + "and UserName = '" + App.PatientFilter + "'";
                }

                //Check date filter
                if (!App.DateFilter.Equals(String.Empty) && firstFilter)
                {
                    SQLcommand = SQLcommand + "DisplayDate = '" + App.DateFilter + "'";
                    firstFilter = false;
                }
                else if (!App.DateFilter.Equals(String.Empty))
                {
                    SQLcommand = SQLcommand + "and DisplayDate = '" + App.DateFilter + "'";
                }

                //Check session filter
                if (!App.SessionFilter.Equals(String.Empty) && firstFilter)
                {
                    SQLcommand = SQLcommand + "SessionNumber = '" + App.SessionFilter + "'";
                    firstFilter = false;
                }
                else if (!App.SessionFilter.Equals(String.Empty))
                {
                    SQLcommand = SQLcommand + "and SessionNumber = '" + App.SessionFilter + "'";
                }

                //Query database based on desired filters
                using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
                {
                    conn.CreateTable<Measurement>();
                    var measurements = conn.Query<Measurement>(SQLcommand).ToList();
                    if (measurements != null)
                    {
                        measurementsView.ItemsSource = measurements;
                    }
                    filteredList = measurements;
                }
            }            
        }

        public void View_Graph()
        {
            Navigation.PushAsync(new HistoryGraph(filteredList));
        }

        public void ResetTable()
        {
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<Measurement>();

                //Set up source items for the initial View model
                var measurements = conn.Table<Measurement>().ToList();
                if (measurements != null)
                {
                    measurementsView.ItemsSource = measurements;
                }
            }
        }
    }
}