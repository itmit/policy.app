using System.Collections.Generic;
using Microcharts;
using SkiaSharp;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace policy.app.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StatisticsPage : ContentPage
    {
        List<MyChart> MyCharts;
        public StatisticsPage()
        {
            InitializeComponent();
            MyCharts = new List<MyChart>();
            PopulateCharts();
        }
        private void PopulateCharts()
        {
            MyCharts.Add(new MyChart() { ChartData = Chart1 });
            MyCharts.Add(new MyChart() { ChartData = Chart2 });
            MyListview.ItemsSource = MyCharts;
        }

        public class MyChart
        {
            public Chart ChartData { get; set; }
        }

        public Chart Chart1 => new BarChart()
        {
            Entries = new[]
            {
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
        }},
            BackgroundColor = SKColors.White
        };


        public Chart Chart2 => new BarChart()
        {
            Entries = new[]
            {
             new Microcharts.Entry(128)
             {
                 Label = "Android",
                 ValueLabel = "128",
                 Color = SKColor.Parse("#b455b6")
             },
             new Microcharts.Entry(514)
             {
                 Label = "UWP",
                 ValueLabel = "514",
                 Color = SKColor.Parse("#3498db")
        }},
            BackgroundColor = SKColors.Yellow
        };
    }
}