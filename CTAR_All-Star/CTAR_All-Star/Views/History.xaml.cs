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

                //Load the picker items
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
                foreach (var n in nameList)
                {
                    namePicker.Items.Add(n);
                }
                namePicker.Items.Add("All");
            }

            if (sessionList != null)
            {
                foreach (var n in sessionList)
                {
                    sessionPicker.Items.Add(n);
                }
            }

            if (pressureList != null)
            {
                foreach (var n in pressureList)
                {
                    pressurePicker.Items.Add(n?.ToString());
                }
            }

            if (dateList != null)
            {
                foreach (var n in dateList)
                {
                    datePicker.Items.Add(n);
                }
            }

            if (timeList != null)
            {
                foreach (var n in timeList)
                {
                    timePicker.Items.Add(n);
                }
            }
        }

        void InititalizePickerListeners()
        {
            NamePicker.SelectedIndexChanged += this.NamePickerIndexChanged;
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
    }
}