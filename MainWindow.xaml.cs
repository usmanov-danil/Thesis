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
        private models.Image image = new models.Image();
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
                this.image.img = this.services.LoadImage(openFileDialog.FileName);
                imgPhoto.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            }
                

        }
        // --------------------------------------------------------------------
        //// Image processing
        private void FactoryName_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.image.manufacture_name = FactoryName.Text;
        }

        private void EcpBrand_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.image.ecp_brand = EcpBrand.Text;
        }
        
        // opt params

        private void OptQ_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.image.optimal_params.power = (float)Convert.ToDouble(OptQ.Text);
        }
        private void OptH_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.image.optimal_params.height = (float)Convert.ToDouble(OptH.Text);
        }
        private void OptPower_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.image.optimal_params.kilowats = (float)Convert.ToDouble(OptPower.Text);
        }
        private void OptEff_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.image.optimal_params.efficiency = (float)Convert.ToDouble(OptEff.Text);
        }

        // Number of stages
        private void NumStages_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.image.number_stages = Convert.ToInt32(NumStages.Text);
        }
        // Plot params
        private void MinQ_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void MinH_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void MinPower_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        // Buttons

        private void button_line_color_1_Click(object sender, RoutedEventArgs e)
        {
            if (imgPhoto.Source != null)
                button_line_color_1.Background = (Brush)converter.Convert(imgPhoto.SelectedColor, typeof(Brush), null, null);
        }

        private void button_line_color_2_Click(object sender, RoutedEventArgs e)
        {
            if (imgPhoto.Source != null)
                button_line_color_2.Background = (Brush)converter.Convert(imgPhoto.SelectedColor, typeof(Brush), null, null);
        }

        private void button_make_table_Click(object sender, RoutedEventArgs e)
        {
            if (imgPhoto.Source != null)
            {
                MessageBox.Show("An image should be uploaded!");
                return;
            }
            if (!this.image.IsDataExists())
            {
                MessageBox.Show("Data above should be entered!");
                return;
            }

            services.ParsePlot();
            services.ParsePlot();

        }
    }
} 