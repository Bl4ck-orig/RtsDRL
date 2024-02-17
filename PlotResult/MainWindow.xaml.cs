using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace PlotResult
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public PlotModel MyPlotModel { get; private set; }

        private static string _1stMagnitudes = Directory.GetCurrentDirectory() + "\\Data\\1st_Magnitudes.txt";
        private static string _2ndMagnitudes = Directory.GetCurrentDirectory() + "\\Data\\2nd_Magnitudes.txt";
        private static string _3rdMagnitudes = Directory.GetCurrentDirectory() + "\\Data\\3rd_Magnitudes.txt";
        private static string _4thMagnitudes = Directory.GetCurrentDirectory() + "\\Data\\4th_Magnitudes.txt";
        private static string _4thRewards = Directory.GetCurrentDirectory() + "\\Data\\4th_Rewards.txt";

        public MainWindow()
        {
            /*
             * Fuer eine exakte Duplizierung der Resultate aus dem E-Learning Dokument 
             * ist eine Anpassung der Skalierung und Begrenzung noetig.
             */

            string fileName = _4thRewards;

            if (!File.Exists(fileName))
                throw new ArgumentException("File name not existant");

            InitializeComponent();
            //MyPlotModel = CreatePlotModelMagnitudes(DataImporter.ImportData(fileName), "4th",  1000);
            MyPlotModel = CreatePlotModelRewards(DataImporter.ImportData(fileName), "4th", 2500);
            DataContext = this;
        }

        private PlotModel CreatePlotModelMagnitudes(List<double> dataList, string _label, int _overrideLimit = -1)
        {
            int limit = _overrideLimit == -1 ? dataList.Count : _overrideLimit;
            var plotModel = new PlotModel { Title = _label + " Magnitudes # " + limit };

            // Add the X-axis (Bottom) and set its title
            var xAxis = new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Lerniteration" // Replace with your desired title
            };
            plotModel.Axes.Add(xAxis);

            // Add the Y-axis (Left) and set its title
            var yAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Gradient Länge", // Replace with your desired title
            };
            plotModel.Axes.Add(yAxis);

            var lineSeries = new LineSeries();

            for (int i = 0; i < limit; i++)
            {
                lineSeries.Points.Add(new DataPoint(i, dataList[i]));
            }

            plotModel.Series.Add(lineSeries);

            return plotModel;
        }

        private PlotModel CreatePlotModelRewards(List<double> dataList, string _label, int _overrideStart = -1, int _overrideLimit = -1)
        {
            int start = _overrideStart == -1 ? 0 : _overrideStart;
            int limit = _overrideLimit == -1 ? dataList.Count : _overrideLimit;
            var plotModel = new PlotModel { Title = _label + " Rewards # " + limit };

            // Add the X-axis (Bottom) and set its title
            var xAxis = new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Episode" // Replace with your desired title
            };
            plotModel.Axes.Add(xAxis);

            // Add the Y-axis (Left) and set its title
            var yAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Reward", // Replace with your desired title
            };
            plotModel.Axes.Add(yAxis);

            var lineSeries = new LineSeries();

            for (int i = start; i < limit; i++)
            {
                lineSeries.Points.Add(new DataPoint(i, dataList[i]));
            }

            plotModel.Series.Add(lineSeries);

            return plotModel;
        }
    }
}
