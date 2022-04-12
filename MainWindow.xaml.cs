using System;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Aggregator.models;
using Aggregator.services;
using Microsoft.Win32;
using System.Windows.Data;

namespace Aggregator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Services services;
        private IValueConverter converter;
        private Data? data = null;
        private string delimiter = ";";
        public MainWindow()
        {
            InitializeComponent();
            this.services = new Services();
            this.converter = new ColorToBrushConverter();
            
        }

        private void button_csv_Click(object sender, RoutedEventArgs e)
        {
           OpenFileDialog openFileDialog = new OpenFileDialog();
           openFileDialog.DefaultExt = "csv";
           openFileDialog.Title = "Select telemetry CSV file";
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    this.data = this.services.LoadData(openFileDialog.FileName, this.delimiter);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("The uploaded CSV file has an incompatible structure\nError: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
                
        }

        private void button_plot_Click(object sender, RoutedEventArgs e)
        {
            if (this.data != null)
            {
                ViewModel.Chart1Model = this.services.DrawWellProfileYZ(this.data);
                ViewModel.Chart2Model = this.services.DrawWellProfileXZ(this.data);
                ViewModel.Chart3Model = this.services.DrawWellProfileXY(this.data);


                ScatterPlot.AddPoints(this.services.DrawWellProfile3D(this.data), Colors.Red, 1.5);
                ScatterPlot.Title = "3D Profile plot";
                ScatterPlot.CreateElements();
                ScatterPlot.ZoomExtents();

            }
            else
                MessageBox.Show("An csv file does not loaded!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
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
                try
                {
                    ViewModel.Image = this.services.LoadImage(openFileDialog.FileName);
                    ViewModel.ImagePath = openFileDialog.FileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("The uploaded image file in an incompatible\nError: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
                

        }
        // --------------------------------------------------------------------
        //// Image processing

       

        // Buttons

        private void button_line_color_1_Click(object sender, RoutedEventArgs e)
        {
            if (imgPhoto.Source != null)
                ViewModel.Color1 = imgPhoto.SelectedColor;
        }

        private void button_line_color_2_Click(object sender, RoutedEventArgs e)
        {
            if (imgPhoto.Source != null)
                ViewModel.Color2 = imgPhoto.SelectedColor;
        }

        private void button_make_table_Click(object sender, RoutedEventArgs e)
        {
            if (imgPhoto.Source == null)
            {
                MessageBox.Show("An image should be uploaded!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            
            var (x1, y1) = services.ParsePlot(ViewModel.ImagePath, ViewModel.Color1);
            var (x2, y2) = services.ParsePlot(ViewModel.ImagePath, ViewModel.Color2);
            // TODO:
            // Add plots on image
            // Transform data to table


        }
    }
} 