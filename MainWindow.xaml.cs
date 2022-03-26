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
        private SurfacePlotModel SurfacePlotModel;
        private Data? data = null;
        private string delimiter = ";";
        public MainWindow()
        {
            InitializeComponent();
            this.services = new Services();
            SurfacePlotModel = new SurfacePlotModel();
            mySurfacePlotView.DataContext = SurfacePlotModel;
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

                SurfacePlotModel.Title = "3D surface model";
                
                SurfacePlotModel.PlotData(this.services.DrawWellModel3D(this.data));
                SurfacePlotModel.ShowSurfaceMesh = true;
                SurfacePlotModel.ShowContourLines = true;
                SurfacePlotModel.ShowMiniCoordinates = true;
                SurfacePlotModel.ShowCoordinateSystem = true;
                mySurfacePlotView.ShowMiniCoordinates = true;

                mySurfacePlotView.hViewport.ZoomExtents();

                plot.AddPoints(this.services.DrawWellProfile3D(this.data), Colors.Red, 1.5);
               // plot.BoundingBox = new Rect3D(minX, minY, minZ, maxX, maxY, maxZ);
                plot.CreateElements();
                plot.ZoomExtents();

                //SurfacePlotModel.DataPoints = this.services.DrawWellModel3D(this.data);
                //mySurfacePlotView.DataPoints = this.services.DrawWellModel3D(this.data);



                //chart4 = this.services.DrawWellProfile3D(this.data);
                //chart5 = 
            }
            else
                MessageBox.Show("An csv file does not loaded!");
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