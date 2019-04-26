using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTAR_All_Star.Models;
using CTAR_All_Star.Views;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CTAR_All_Star
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HistoryPage : ContentPage
	{
        private Picker namePicker, sessionPicker, pressurePicker, datePicker, timePicker;
        private List<String> nameList, sessionList, dateList, timeList;
        private List<Double?> pressureList;

        public HistoryPage()
		{
			InitializeComponent();
            InitializeLists();
            InititalizePickerListeners();            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            InitializePickers();            

            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<Measurement>();

                //Set up source items for the View model
                var measurements = conn.Table<Measurement>().ToList();
                if(measurements != null)
                {
                    measurementsView.ItemsSource = measurements;
                }               

                //Load the picker items - this should probably be done in the constructor only
                var table = conn.Table<Measurement>();
                if(table != null)
                {
                    foreach (var m in table)
                    {                        
                        if (!nameList.Contains(m.UserName) && m.UserName != null)
                        {
                            nameList.Add(m.UserName);
                        }
                        if(!sessionList.Contains(m.SessionNumber) && m.SessionNumber != null)
                        {
                            sessionList.Add(m.SessionNumber);
                        }
                        if (!pressureList.Contains(m.Pressure) && m.Pressure != null)
                        {
                            pressureList.Add(m.Pressure);
                        }
                        if (!dateList.Contains(m.DisplayDate) && m.DisplayDate != null)
                        {
                            dateList.Add(m.DisplayDate);
                        }
                        if (!timeList.Contains(m.DisplayTime) && m.DisplayTime != null)
                        {
                            timeList.Add(m.DisplayTime);
                        }
                    }
                }              
            }
            LoadPickers();
        } 
        
        public void InitializeLists()
        {
            nameList = new List<String>();
            sessionList = new List<String>();
            pressureList = new List<Double?>();
            dateList = new List<String>();
            timeList = new List<String>();
        }

        public void InitializePickers()
        {
            namePicker = NamePicker;
            sessionPicker = SessionPicker;
            pressurePicker = PressurePicker;
            datePicker = DatePicker;
            timePicker = TimePicker;
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

            if (pressureList != null)
            {
                pressurePicker.Items.Clear();
                foreach (var n in pressureList)
                {
                    pressurePicker.Items.Add(n?.ToString());
                }
                pressurePicker.Items.Add("All");
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

            if (timeList != null)
            {
                timePicker.Items.Clear();
                foreach (var n in timeList)
                {
                    timePicker.Items.Add(n);
                }
                timePicker.Items.Add("All");
            }
        }

        void InititalizePickerListeners()
        {
            NamePicker.SelectedIndexChanged += this.NamePickerIndexChanged;
            SessionPicker.SelectedIndexChanged += this.SessionPickerIndexChanged;
            PressurePicker.SelectedIndexChanged += this.PressurePickerIndexChanged;
            DatePicker.SelectedIndexChanged += this.DatePickerIndexChanged;
            TimePicker.SelectedIndexChanged += this.TimePickerIndexChanged;
        }

        public void NamePickerIndexChanged(object sender, EventArgs e)
        {
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<Measurement>();

                //Show all
                if (NamePicker.SelectedItem.Equals("All"))
                {
                    //Set up source items for the View model
                    var measurements = conn.Table<Measurement>().ToList();
                    if (measurements != null)
                    {
                        measurementsView.ItemsSource = measurements;
                    }
                    //NamePicker.SelectedItem = null;
                }
                //Filter by name
                else
                {
                    //Set up source items for the View model
                    List<Measurement> list = new List<Measurement>();
                    var measurements = conn.Table<Measurement>();
                    if (measurements != null)
                    {
                        foreach (var m in measurements)
                        {
                            if (m.UserName.Equals(NamePicker.SelectedItem))
                            {
                                list.Add(m);
                            }
                        }
                        measurementsView.ItemsSource = list;
                    }
                }                
            }
        }

        public void SessionPickerIndexChanged(object sender, EventArgs e)
        {
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<Measurement>();

                //Show all
                if (SessionPicker.SelectedItem.Equals("All"))
                {
                    //Set up source items for the View model
                    var measurements = conn.Table<Measurement>().ToList();
                    if (measurements != null)
                    {
                        measurementsView.ItemsSource = measurements;
                    }
                }
                //Filter by session
                else
                {
                    //Set up source items for the View model
                    List<Measurement> list = new List<Measurement>();
                    var measurements = conn.Table<Measurement>();
                    if (measurements != null)
                    {
                        foreach (var m in measurements)
                        {
                            if (m.SessionNumber.Equals(SessionPicker.SelectedItem))
                            {
                                list.Add(m);
                            }
                        }
                        measurementsView.ItemsSource = list;
                    }
                }
            }
        }

        public void PressurePickerIndexChanged(object sender, EventArgs e)
        {
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<Measurement>();

                //Show all
                if (PressurePicker.SelectedItem.Equals("All"))
                {
                    //Set up source items for the View model
                    var measurements = conn.Table<Measurement>().ToList();
                    if (measurements != null)
                    {
                        measurementsView.ItemsSource = measurements;
                    }
                }
                //Filter by pressure
                else
                {
                    //Set up source items for the View model
                    List<Measurement> list = new List<Measurement>();
                    var measurements = conn.Table<Measurement>();
                    if (measurements != null)
                    {
                        foreach (var m in measurements)
                        {
                            if (m.Pressure.Equals(Convert.ToDouble(PressurePicker.SelectedItem)))
                            {
                                list.Add(m);
                            }
                        }
                        measurementsView.ItemsSource = list;
                    }
                }
            }
        }
        public void DatePickerIndexChanged(object sender, EventArgs e)
        {
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<Measurement>();

                //Show all
                if (DatePicker.SelectedItem.Equals("All"))
                {
                    //Set up source items for the View model
                    var measurements = conn.Table<Measurement>().ToList();
                    if (measurements != null)
                    {
                        measurementsView.ItemsSource = measurements;
                    }
                }
                //Filter by date
                else
                {
                    //Set up source items for the View model
                    List<Measurement> list = new List<Measurement>();
                    var measurements = conn.Table<Measurement>();
                    if (measurements != null)
                    {
                        foreach (var m in measurements)
                        {
                            if (m.DisplayDate.Equals(DatePicker.SelectedItem))
                            {
                                list.Add(m);
                            }
                        }
                        measurementsView.ItemsSource = list;
                    }
                }
            }
        }
        public void TimePickerIndexChanged(object sender, EventArgs e)
        {
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<Measurement>();

                //Show all
                if (TimePicker.SelectedItem.Equals("All"))
                {
                    //Set up source items for the View model
                    var measurements = conn.Table<Measurement>().ToList();
                    if (measurements != null)
                    {
                        measurementsView.ItemsSource = measurements;
                    }
                }
                //Filter by time
                else
                {
                    //Set up source items for the View model
                    List<Measurement> list = new List<Measurement>();
                    var measurements = conn.Table<Measurement>();
                    if (measurements != null)
                    {
                        foreach (var m in measurements)
                        {
                            if (m.DisplayTime.Equals(TimePicker.SelectedItem))
                            {
                                list.Add(m);
                            }
                        }
                        measurementsView.ItemsSource = list;
                    }
                }
            }
        }
    }
}