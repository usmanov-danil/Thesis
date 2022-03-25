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
using OxyPlot;
using Aggregator.models;
using Aggregator.services;
using Microsoft.Win32;
using Aggregator.communicators;
using WPFSurfacePlot3D;

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
                
                //mySurfacePlotModel.DataPoints = this.services.DrawWellModel3D(this.data);
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