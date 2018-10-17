using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microcharts.Forms;
using SkiaSharp;
using Microcharts;

namespace CTAR_All_Star
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class GraphPage : ContentPage
	{
        List<Microcharts.Entry> entries = new List<Microcharts.Entry>
        {
            new Microcharts.Entry(212)
            {
                Label = "UWP",
                ValueLabel = "212",
                Color = SKColor.Parse("#2c3e50")
            },
            new Microcharts.Entry(248)
            {
                Label = "Android",
                ValueLabel = "248",
                Color = SKColor.Parse("#77d065")
            },
            new Microcharts.Entry(128)
            {
                Label = "iOS",
                ValueLabel = "128",
                Color = SKColor.Parse("#b455b6")
            },
            new Microcharts.Entry(514)
            {
                Label = "Shared",
                ValueLabel = "514",
                Color = SKColor.Parse("#3498db")
            }
        };


        public GraphPage()
        {
            InitializeComponent();

            Chart1.Chart = new LineChart()
            {
                Entries = entries,
                LineMode = LineMode.Straight,
                LineSize = 8,
                PointMode = PointMode.Square,
                PointSize = 18
            };
        }
    }
}