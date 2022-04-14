using Aggregator.models;
using Aggregator.communicators;
using OxyPlot;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing;
using MathNet.Numerics;
using Color = System.Drawing.Color;
using System.Windows.Shapes;
using Brushes = System.Windows.Media.Brushes;
using System.Reflection;

namespace Aggregator.services
{
    public class Services
    {
        // Drawwing
        
        public PlotModel DrawWellProfileXY(Data data)
        {
            PlotModel model = Charting.Draw2D(data.X.ToArray(), data.Y.ToArray(), OxyColors.Red);
            model.Title = "XY";
            return model;   
        }
        public PlotModel DrawWellProfileXZ(Data data)
        {
            PlotModel model = Charting.Draw2D(data.X.ToArray(), data.Z.ToArray(), OxyColors.Blue);
            model.Title = "XZ";
            return model;
        }

        public PlotModel DrawWellProfileYZ(Data data)
        {
            PlotModel model = Charting.Draw2D(data.X.ToArray(), data.Z.ToArray(), OxyColors.Green);
            model.Title = "YZ";
            return model;
        }

        public System.Windows.Media.Media3D.Point3D[,] DrawWellModel3D(Data data)
        {
           return Helix.Draw3DModel(data.X.ToArray(), data.Y.ToArray(), data.Z.ToArray());
        }

        public System.Windows.Media.Media3D.Point3D[] DrawWellProfile3D(Data data)
        {
            return Helix.Draw3DScatter(data.X.ToArray(), data.Y.ToArray(), data.Z.ToArray());
        }

        // Data services

        public Data LoadData(string Path, string delimiter)
        {
            string[] csv = File.ReadAllLines(Path);
            Data data = Data.FromCsv(csv.Skip(1).ToArray(), delimiter);
            // save to db
            return data;
        }

        // Image services

        public BitmapImage LoadImage(string Path)
        { 
            return new BitmapImage(new Uri(Path));
        }

        private int ColorToGrayScale(Color color)
        {
            return (int)(color.R * 0.299 + color.G * 0.578 + color.B * 0.114);
        }
        public Tuple<double[], double[], double, double> ParsePlot(string Path, System.Windows.Media.Color TargetColor, Tuple<double, double> sizes)
        {
            return ParsePlot(Path, System.Drawing.Color.FromArgb(TargetColor.A, TargetColor.R, TargetColor.G, TargetColor.B), sizes);
        }
        public Tuple<double[], double[], double, double> ParsePlot(string Path, System.Drawing.Color TargetColor, Tuple<double, double> sizes)
        {
            int ColorDelta = 10;
            int Degree = 7;
            List<double> x = new List<double>();
            List<double> y = new List<double>();
            Bitmap img = new Bitmap(Image.FromFile(Path));

            for (int i = 0; i < img.Width; i++)
                for (int j = 0; j < img.Height; j++)
                {
                    int TargetGrey = ColorToGrayScale(TargetColor);
                    int PixelGrey = ColorToGrayScale(img.GetPixel(i, j));
                    if (PixelGrey > TargetGrey - ColorDelta && PixelGrey < TargetGrey + ColorDelta)
                    {
                        x.Add(i);
                        y.Add(img.Height - j); // To mirror
                    }
                }

            double[] yy = y.ConvertAll(z => (double)z).ToArray();
            double[] xx = x.ConvertAll(z => (double)z).ToArray();

            var f = Fit.PolynomialFunc(x.ToArray(), y.ToArray(), Degree);
            double kx = sizes.Item1 / img.Width;
            double ky = sizes.Item2 / img.Height;
            for (var i = 0; i < x.Count(); i++)
            {
                xx[i] = x.ElementAt(i);
                yy[i] = f(x.ElementAt(i));
            }
                
            

            return new Tuple<double[], double[], double, double>(xx, yy, kx, ky);
            
        }

        public Tuple<double[], double[], Polyline> DigitizeImage(Tuple<double[], double[], double, double> coords, double height)
        {
            var (x, y, kx, ky) = coords;
            var poly = new Polyline();
            poly.Points = new PointCollection();
            for (int i = 0; i < x.Length; i++)
            {
                x[i] = x[i] * kx;
                y[i] = height - y[i] * ky; // As y goes from top to down
                poly.Points.Add(new System.Windows.Point(x[i], y[i]));

            }
            poly.StrokeDashCap = PenLineCap.Flat;
            poly.Stroke = PickBrush();
            poly.StrokeThickness = 10;
            var foo = new double[] { 1, 2 };
            poly.StrokeDashArray = new DoubleCollection(foo);

            return new Tuple<double[], double[], Polyline>(x, y, poly);
        }



        private System.Windows.Media.Brush PickBrush()
        {
            System.Windows.Media.Brush result = Brushes.Black;

            Random rnd = new Random();

            Type brushesType = typeof(Brushes);

            PropertyInfo[] properties = brushesType.GetProperties();

            int random = rnd.Next(properties.Length);
            return (System.Windows.Media.Brush)properties[random].GetValue(null, null);
        }


    }
}