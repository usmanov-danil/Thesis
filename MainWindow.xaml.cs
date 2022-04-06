using System;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Aggregator.models;
using Aggregator.services;
using Microsoft.Win32;

namespace Aggregator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Services services;
        private Data? data = null;
        private models.Image? image = null;
        private string delimiter = ";";
        public MainWindow()
        {
            InitializeComponent();
            this.services = new Services();
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
                ChartPlotVisual2D.Chart1Model = this.services.DrawWellProfileYZ(this.data);
                ChartPlotVisual2D.Chart2Model = this.services.DrawWellProfileXZ(this.data);
                ChartPlotVisual2D.Chart3Model = this.services.DrawWellProfileXY(this.data);


                ScatterPlot.AddPoints(this.services.DrawWellProfile3D(this.data), Colors.Red, 1.5);
                ScatterPlot.CreateElements();
                ScatterPlot.ZoomExtents();

            }
            else
                MessageBox.Show("An csv file does not loaded!");
        }

        public ChartPlotVisual2D ChartPlotVisual2D
        {
            get { return (ChartPlotVisual2D)DataContext; }
        }
        private void ComboBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            ComboBoxItem item = (ComboBoxItem)comboBox.SelectedItem;
            if (item != null)
                this.delimiter = item.Content.ToString();
        }

        private void button_open_img_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = "png";
            openFileDialog.Title = "Select image of passport characteristic";
            openFileDialog.Filter = "All supported graphics|*.jpg;*.jpeg;*.png;*.gif|" +
               "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
               "Portable Network Graphic (*.png)|*.png";
            if (openFileDialog.ShowDialog() == true)
            {
                this.image = this.services.LoadImage(openFileDialog.FileName);
                imgPhoto.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            }
                

        }
    }
} 