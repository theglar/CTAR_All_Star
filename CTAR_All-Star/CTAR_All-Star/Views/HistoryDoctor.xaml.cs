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
                        if (!dateList.Contains(m.DisplayDate) && m.DisplayDate != null)
                        {
                            dateList.Add(m.DisplayDate);
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
            //using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            //{
            //    conn.CreateTable<Measurement>();

            //    //Show all
            //    if (NamePicker.SelectedItem.Equals("All"))
            //    {
            //        //Set up source items for the View model
            //        var measurements = conn.Table<Measurement>().ToList();
            //        if (measurements != null)
            //        {
            //            measurementsView.ItemsSource = measurements;
            //        }
            //        //NamePicker.SelectedItem = null;
            //    }
            //    //Filter by name
            //    else
            //    {
            //        //Set up source items for the View model
            //        List<Measurement> list = new List<Measurement>();
            //        var measurements = conn.Table<Measurement>();
            //        if (measurements != null)
            //        {
            //            foreach (var m in measurements)
            //            {
            //                if (m.UserName.Equals(NamePicker.SelectedItem))
            //                {
            //                    list.Add(m);
            //                }
            //            }
            //            measurementsView.ItemsSource = list;
            //        }
            //    }                
            //}
        }

        public void SessionPickerIndexChanged(object sender, EventArgs e)
        {
            FilterData();
            //using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            //{
            //    conn.CreateTable<Measurement>();

            //    //Show all
            //    if (SessionPicker.SelectedItem.Equals("All"))
            //    {
            //        //Set up source items for the View model
            //        var measurements = conn.Table<Measurement>().ToList();
            //        if (measurements != null)
            //        {
            //            measurementsView.ItemsSource = measurements;
            //        }
            //    }
            //    //Filter by session
            //    else
            //    {
            //        //Set up source items for the View model
            //        List<Measurement> list = new List<Measurement>();
            //        var measurements = conn.Table<Measurement>();
            //        if (measurements != null)
            //        {
            //            foreach (var m in measurements)
            //            {
            //                if (m.SessionNumber.Equals(SessionPicker.SelectedItem))
            //                {
            //                    list.Add(m);
            //                }
            //            }
            //            measurementsView.ItemsSource = list;
            //        }
            //    }
            //}
        }

        public void DatePickerIndexChanged(object sender, EventArgs e)
        {
            FilterData();
            //using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            //{
            //    conn.CreateTable<Measurement>();

            //    //Show all
            //    if (DatePicker.SelectedItem.Equals("All"))
            //    {
            //        //Set up source items for the View model
            //        var measurements = conn.Table<Measurement>().ToList();
            //        if (measurements != null)
            //        {
            //            measurementsView.ItemsSource = measurements;
            //        }
            //    }
            //    //Filter by date
            //    else
            //    {
            //        //Set up source items for the View model
            //        List<Measurement> list = new List<Measurement>();
            //        var measurements = conn.Table<Measurement>();
            //        if (measurements != null)
            //        {
            //            foreach (var m in measurements)
            //            {
            //                if (m.DisplayDate.Equals(DatePicker.SelectedItem))
            //                {
            //                    list.Add(m);
            //                }
            //            }
            //            measurementsView.ItemsSource = list;
            //        }
            //    }
            //}
        }

        public void FilterData()
        {
            //Pull all data from the database and make a list
            filteredList = new List<Measurement>();

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

            //Check Patient Filter
            if(NamePicker.SelectedIndex == -1 || NamePicker.SelectedItem.Equals("All"))
            {
                //Do nothing, no filter requested.
            }
            else
            {
                //Remove measurments not wanted
                foreach (var m in filteredList)
                {
                    if (!m.UserName.Equals(NamePicker.SelectedItem))
                    {
                        filteredList.Remove(m);
                    }
                }
            }

            //Check Date Filter
            if (datePicker.SelectedIndex == -1 || DatePicker.SelectedItem.Equals("All"))
            {
                //Do nothing, no filter requested.
            }
            else
            {
                //Remove measurments not wanted
                foreach (var m in filteredList)
                {
                    if (!m.DisplayDate.Equals(DatePicker.SelectedItem))
                    {
                        filteredList.Remove(m);
                    }
                }
            }

            //Check Session Filter
            if (SessionPicker.SelectedIndex == -1 || SessionPicker.SelectedItem.Equals("All"))
            {
                //Do nothing, no filter requested.
            }
            else
            {
                //Remove measurments not wanted
                foreach (var m in filteredList)
                {
                    if (!m.SessionNumber.Equals(SessionPicker.SelectedItem))
                    {
                        filteredList.Remove(m);
                    }
                }
            }

            //Send filtered list to the view
            measurementsView.ItemsSource = filteredList;
        }
    }
}