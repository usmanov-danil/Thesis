using System;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Aggregator.models;
using Aggregator.services;
using Microsoft.Win32;
using WPFSurfacePlot3D;
using System.Windows.Media.Media3D;

namespace Aggregator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Services services;
        private SurfacePlotModel mySurfacePlotModel;
        private Data? data = null;
        private string delimiter = ";";
        public MainWindow()
        {
            InitializeComponent();
            this.services = new Services();
            mySurfacePlotModel = new SurfacePlotModel();
            mySurfacePlotView.DataContext = mySurfacePlotModel;
        }

        private void button_csv_Click(object sender, RoutedEventArgs e)
        {
           OpenFileDialog openFileDialog = new OpenFileDialog();
           openFileDialog.DefaultExt = "csv";
           openFileDialog.Title = "Select telemetry CSV file";
            if (openFileDialog.ShowDialog() == true)
                this.data = this.services.LoadData(openFileDialog.FileName, this.delimiter);
        }

        private void button_plot_Click(object sender, RoutedEventArgs e)
        {
            if (this.data != null)
            {
                ViewModel.Chart1Model = this.services.DrawWellProfileYZ(this.data);
                ViewModel.Chart2Model = this.services.DrawWellProfileXZ(this.data);
                ViewModel.Chart3Model = this.services.DrawWellProfileXY(this.data);

                mySurfacePlotModel.Title = "3D surface model";
                
                mySurfacePlotModel.PlotData(this.services.DrawWellModel3D(this.data));
                mySurfacePlotModel.ShowSurfaceMesh = true;
                mySurfacePlotModel.ShowContourLines = true;
                mySurfacePlotModel.ShowMiniCoordinates = true;
                mySurfacePlotModel.ShowCoordinateSystem = true;
                mySurfacePlotView.ShowMiniCoordinates = true;

                mySurfacePlotView.hViewport.ZoomExtents();
                PlotData(this.data);
                plot.CreateElements();
                plot.ZoomExtents();

                //mySurfacePlotModel.DataPoints = this.services.DrawWellModel3D(this.data);
                //mySurfacePlotView.DataPoints = this.services.DrawWellModel3D(this.data);



                //chart4 = this.services.DrawWellProfile3D(this.data);
                //chart5 = 
            }
            else
                MessageBox.Show("An csv file does not loaded!");
        }


    

        private void PlotData(Data data)
        {
            double[] x = data.X.ToArray();
            double[] y = data.Y.ToArray();
            double[] z = data.Z.ToArray();
            double minX = 100000;
            double minY = 100000;
            double minZ = 100000;
            double maxX = -100000;
            double maxY = -100000;
            double maxZ = -100000;
            for (int i = 0; i < data.Length(); i++)
            {
                plot.AddPoint(new Point3D(x[i], y[i], z[i]), Colors.Red, 1.5);
                minX = Math.Min(minX, x[i]);
                minY = Math.Min(minY, y[i]);
                minZ = Math.Min(minZ, z[i]);
                maxX = Math.Max(maxX, x[i]);
                maxY = Math.Max(maxY, y[i]);
                maxZ = Math.Max(maxZ, z[i]);
            }
            plot.BoundingBox = new Rect3D(minX, minY, minZ, maxX, maxY, maxZ);
        }









        public ViewModel ViewModel
        {
            get { return (ViewModel)DataContext; }
        }
        private void ComboBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            ComboBoxItem item = (ComboBoxItem)comboBox.SelectedItem;
            if (item != null)
                this.delimiter = item.Content.ToString();
        }
        

    }
} 