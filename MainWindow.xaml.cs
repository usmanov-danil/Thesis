using System;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Aggregator.models;
using Aggregator.services;
using Microsoft.Win32;
using System.Windows.Data;
using System.Drawing;
using System.Windows.Shapes;

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

        private void button_line_color_3_Click(object sender, RoutedEventArgs e)
        {
            if (imgPhoto.Source != null)
                ViewModel.Color3 = imgPhoto.SelectedColor;
        }
        

        private void button_make_table_Click(object sender, RoutedEventArgs e)
        {
            if (imgPhoto.Source == null)
            {
                MessageBox.Show("An image should be uploaded!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (!ViewModel.IsPointsExist())
            {
                MessageBox.Show("An plot corner points should be selected!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var parsed1 = services.ParsePlot(ViewModel.ImagePath, ViewModel.Color1, new Tuple<double, double>(imgPhoto.ActualWidth, imgPhoto.ActualHeight));
            var (x1, y1, poly1) = services.DigitizeImage(parsed1, imgPhoto.ActualHeight);
            Binding myBinding = new Binding();
            myBinding.Source = ViewModel;
            myBinding.Path = new PropertyPath("HIsChecked");
            myBinding.Mode = BindingMode.OneWay;
           myBinding.UpdateSourceTrigger = UpdateSourceTrigger.Default;
            poly1.SetBinding(VisibilityProperty, myBinding);
            Canvas.Children.Add(poly1);

            var parsed2 = services.ParsePlot(ViewModel.ImagePath, ViewModel.Color2, new Tuple<double, double>(imgPhoto.ActualWidth, imgPhoto.ActualHeight));
            var(x2, y2, poly2) = services.DigitizeImage(parsed2, imgPhoto.ActualHeight);
            Canvas.Children.Add(poly2);

            var parsed3 = services.ParsePlot(ViewModel.ImagePath, ViewModel.Color3, new Tuple<double, double>(imgPhoto.ActualWidth, imgPhoto.ActualHeight));
            var (x3, y3, poly3) = services.DigitizeImage(parsed3, imgPhoto.ActualHeight);
            Canvas.Children.Add(poly3);

            // TODO:
            // Transform data to table
            // Add checkboxes



        }

        private void orig_Click(object sender, RoutedEventArgs e)
        {
            var p = imgPhoto.SelectedPosition;
            ViewModel.OriginPoint = p;
            orig.Content = $"({(int)p.X};{(int)p.Y})px";
        }
        private void x_Click(object sender, RoutedEventArgs e)
        {
            var p = imgPhoto.SelectedPosition;
            ViewModel.XPoint = p;
            x.Content = $"({(int)p.X};{(int)p.Y})px";
        }
        private void y_Click(object sender, RoutedEventArgs e)
        {
            var p = imgPhoto.SelectedPosition;
            ViewModel.YPoint = p;
            y.Content = $"({(int)p.X};{(int)p.Y})px";
        }

    }
} 