using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PlotResult
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public PlotModel MyPlotModel { get; private set; }

        public MainWindow()
        {
            string fileName = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Model_0_2023_12_10-09_22.bin";

            if (!File.Exists(fileName))
                throw new ArgumentException("File name not existant");

            InitializeComponent();
            MyPlotModel = CreatePlotModel(DataImporter.ImportData(fileName));
            DataContext = this;
        }

        private PlotModel CreatePlotModel(List<double> dataList)
        {
            var plotModel = new PlotModel { Title = "Data Plot" };
            plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
            var lineSeries = new LineSeries();

            for (int i = 0; i < dataList.Count; i++)
            {
                lineSeries.Points.Add(new DataPoint(i, dataList[i]));
            }

            plotModel.Series.Add(lineSeries);

            return plotModel;
        }
    }
}
