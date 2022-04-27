using System;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Aggregator.models;
using Aggregator.services;
using Microsoft.Win32;
using System.Windows.Data;
using System.Windows.Shapes;
using System.IO;

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
                    ViewModel.Image1 = this.services.LoadImage(openFileDialog.FileName);
                    ViewModel.ImagePath1 = openFileDialog.FileName;
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
            if (imgPhoto1.Source != null)
                ViewModel.Color11 = imgPhoto1.SelectedColor;
        }

        private void button_line_color_2_Click(object sender, RoutedEventArgs e)
        {
            if (imgPhoto1.Source != null)
                ViewModel.Color12 = imgPhoto1.SelectedColor;
        }

        private void button_line_color_3_Click(object sender, RoutedEventArgs e)
        {
            if (imgPhoto1.Source != null)
                ViewModel.Color13 = imgPhoto1.SelectedColor;
        }
        

        private void button_make_table_Click(object sender, RoutedEventArgs e)
        {
            if (imgPhoto1.Source == null)
            {
                MessageBox.Show("An image should be uploaded!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (!ViewModel.IsPointsExist())
            {
                MessageBox.Show("An plot corner points should be selected!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            Canvas.Children.Clear();
            


            var MaxYPoint = (Point)ViewModel.YPoint1;
            var MaxXPoint = (Point)ViewModel.XPoint1;
            var OrigPoint = (Point)ViewModel.OriginPoint1;
            var MinXPlot = ViewModel.MinPlotParams1.power;
            var MaxXPlot = ViewModel.MaxPlotParams1.power;

            var boundaries = new Tuple<Point, Point, Point>(MaxYPoint, MaxXPoint, OrigPoint);

            var (f1, kx1, ky1, xmax1, xmin1) = services.ParsePlot(ViewModel.ImagePath1, ViewModel.Color11, new Tuple<double, double>(imgPhoto1.ActualWidth, imgPhoto1.ActualHeight), boundaries);
            var (f2, kx2, ky2, xmax2, xmin2) = services.ParsePlot(ViewModel.ImagePath1, ViewModel.Color12, new Tuple<double, double>(imgPhoto1.ActualWidth, imgPhoto1.ActualHeight), boundaries);
            var (f3, kx3, ky3, xmax3, xmin3) = services.ParsePlot(ViewModel.ImagePath1, ViewModel.Color13, new Tuple<double, double>(imgPhoto1.ActualWidth, imgPhoto1.ActualHeight), boundaries);
            
            var MinMaxX = Math.Min(xmax1, Math.Min(xmax2, xmax3));
            var MaxMinX = Math.Max(xmin1, Math.Max(xmin2, xmin3));
            var X = services.GenerateX(ViewModel.Steps1, MinMaxX, MaxMinX);

            var Y1 = services.GenerateY(f1, X);
            var Y2 = services.GenerateY(f2, X);
            var Y3 = services.GenerateY(f3, X);

            var coefs = services.CalculateCoefs(ViewModel.MaxPlotParams1, ViewModel.MinPlotParams1, ViewModel.OriginPoint1, ViewModel.XPoint1,ViewModel.YPoint1);
            var DeltaX = ViewModel.OriginPoint1.X;
            var DeltaY = imgPhoto1.ActualHeight - ViewModel.OriginPoint1.Y;
            ViewModel.Table1 = services.PopulateTable(X, Y1, Y2, Y3, coefs, DeltaX, DeltaY);



            var poly1 = services.DigitizeImage(new Tuple<double[], double[]>(X, Y1), kx1, ky1, imgPhoto1.ActualHeight);
            var poly2 = services.DigitizeImage(new Tuple<double[], double[]>(X, Y2), kx2, ky2, imgPhoto1.ActualHeight);
            var poly3 = services.DigitizeImage(new Tuple<double[], double[]>(X, Y3), kx3, ky3, imgPhoto1.ActualHeight);

            Binding binding1 = new Binding();
            Binding binding2 = new Binding();
            Binding binding3 = new Binding();

            binding1.Converter = binding2.Converter = binding3.Converter  = new BooleanToVisibilityConverter();
            binding1.Source = binding2.Source = binding3.Source = ViewModel;
            binding1.UpdateSourceTrigger = binding2.UpdateSourceTrigger = binding3.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
           
            binding1.Path = new PropertyPath("HIsChecked1");
            binding2.Path = new PropertyPath("NIsChecked1");
            binding3.Path = new PropertyPath("EffIsChecked1");
        
            BindingOperations.SetBinding(poly1, Polyline.VisibilityProperty, binding1);
            BindingOperations.SetBinding(poly2, Polyline.VisibilityProperty, binding2);
            BindingOperations.SetBinding(poly3, Polyline.VisibilityProperty, binding3);

            Canvas.Children.Clear();
            Canvas.Children.Add(poly1);
            Canvas.Children.Add(poly2);
            Canvas.Children.Add(poly3);
        }
        
        private void orig_Click(object sender, RoutedEventArgs e)
        {
            var p = imgPhoto1.SelectedPosition;
            ViewModel.OriginPoint1 = p;
            orig.Content = $"({(int)p.X};{(int)p.Y})px";
        }
        private void x_Click(object sender, RoutedEventArgs e)
        {
            var p = imgPhoto1.SelectedPosition;
            ViewModel.XPoint1 = p;
            x.Content = $"({(int)p.X};{(int)p.Y})px";
        }
        private void y_Click(object sender, RoutedEventArgs e)
        {
            var p = imgPhoto1.SelectedPosition;
            ViewModel.YPoint1 = p;
            y.Content = $"({(int)p.X};{(int)p.Y})px";
        }
        private void button_download_csv_Click(object sender, RoutedEventArgs e)
        {
            if (ImageParsedGrid.ItemsSource == null)
            {
                MessageBox.Show("There is processed data!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
           

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            string filter = "CSV file (*.csv)|*.csv| All Files (*.*)|*.*";
            saveFileDialog.Filter = filter;
            saveFileDialog.FileName = $"{ViewModel.FactoryName1}-{ViewModel.EcpBrand1}";
            if (saveFileDialog.ShowDialog() == true)
            {
                filter = saveFileDialog.FileName;
                StreamWriter writer = new StreamWriter(filter);
                services.SaveDataToCsv(writer, ViewModel.Table1);
                writer.Close();
            }


        }

    }
} 