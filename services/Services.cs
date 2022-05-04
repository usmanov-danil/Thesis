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
using System.Windows.Shapes;
using System.Reflection;
using Color = System.Drawing.Color;
using Brushes = System.Windows.Media.Brushes;


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
        public Tuple<Func<double, double>, double, double, double, double> ParsePlot(string Path, System.Windows.Media.Color TargetColor, Tuple<double, double> sizes, Tuple<System.Windows.Point, System.Windows.Point, System.Windows.Point> boundaries)
        {
            return ParsePlot(Path, System.Drawing.Color.FromArgb(TargetColor.A, TargetColor.R, TargetColor.G, TargetColor.B), sizes, boundaries);
        }
        public Tuple<Func<double, double>, double, double, double, double> ParsePlot(string Path, Color TargetColor, Tuple<double, double> sizes, Tuple<System.Windows.Point, System.Windows.Point, System.Windows.Point> boundaries)
        {
            var (YPoint, XPoint, OriginPoint) = boundaries;
            int ColorDelta = 3;
            int Degree = 9;
            
            Bitmap img = new Bitmap(Image.FromFile(Path));
            double kx = sizes.Item1 / img.Width;
            double ky = sizes.Item2 / img.Height;


            List<double> x = new List<double>();
            List<double> y = new List<double>();
            for (int i = 0; i < img.Width; i++)
                for (int j = 0; j < img.Height; j++)
                {
                    int TargetGrey = ColorToGrayScale(TargetColor);
                    int PixelGrey = ColorToGrayScale(img.GetPixel(i, j));
                    if (PixelGrey > TargetGrey - ColorDelta 
                        && PixelGrey < TargetGrey + ColorDelta
                        && j  < OriginPoint.Y * ky && j > YPoint.Y * ky
                        && i > OriginPoint.X * kx  && i < XPoint.X * kx)
                    {
                        x.Add(i);
                        y.Add(img.Height - j); // To mirror
                    }
                }

            var f = Fit.PolynomialFunc(x.ToArray(), y.ToArray(), Degree);
            

            return new Tuple<Func<double, double>, double, double, double, double>(f, kx, ky, x.Max(), x.Min());

        }
        public double[] GenerateX(double step, double MaxX, double MinX)
        {
            List<double> xx = new List<double>();
            for (double i = MinX; i < MaxX; i += step)
                xx.Add(i);

            return xx.ToArray();
        }
        public double[] GenerateY(Func<double, double> func, double[] x)
        {
            double[] y = new double[x.Length];
            for (int i = 0; i < x.Length; i++)
                y[i] = func(x[i]);


            return y;
        }
            
        public Polyline DigitizeImage(Tuple<double[], double[]> coords, double kx, double ky, double height)
        {
            var (x, y) = coords;
            
            var poly = new Polyline();
            poly.Points = new PointCollection();
            for (int i = 0; i < x.Length; i++)
                poly.Points.Add(new System.Windows.Point(x[i] * kx, height - y[i] * ky));

            poly.StrokeDashCap = PenLineCap.Flat;
            poly.Stroke = PickRandomBrush();
            poly.StrokeThickness = 10;
            var foo = new double[] { 1, 2 };
            poly.StrokeDashArray = new DoubleCollection(foo);

            return poly;
        }

        private System.Windows.Media.Brush PickRandomBrush()
        {
            System.Windows.Media.Brush result = Brushes.Black;

            Random rnd = new Random();

            Type brushesType = typeof(Brushes);

            PropertyInfo[] properties = brushesType.GetProperties();

            int random = rnd.Next(properties.Length);
            return (System.Windows.Media.Brush)properties[random].GetValue(null, null);
        }

        public Tuple<double, double, double, double> CalculateCoefs(PlotParams MaxParams, PlotParams MinParams, System.Windows.Point Origin, System.Windows.Point X, System.Windows.Point Y)
        {
            var DeltaY = Origin.Y - Y.Y;
            var kq = (MaxParams.power - MinParams.power) / (X.X - Origin.X);
            var kh = (MaxParams.height - MinParams.height) / DeltaY;
            var kn = (MaxParams.kilowats - MinParams.kilowats) / DeltaY;
            var keff = (MaxParams.efficiency - MinParams.efficiency) / DeltaY;

            return new Tuple<double, double, double, double>(kq, kh, kn, keff);
        }
        public Tuple<double, double, double, double, double> CalculateCoefs(SemPlotParams MaxParams, SemPlotParams MinParams, System.Windows.Point Origin, System.Windows.Point X, System.Windows.Point Y)
        {
            var DeltaY = Origin.Y - Y.Y;
            var kq = (MaxParams.power - MinParams.power) / (X.X - Origin.X);
            var ks = (MaxParams.s - MinParams.s) / DeltaY;
            var ki = (MaxParams.i - MinParams.i) / DeltaY;
            var keff = (MaxParams.efficiency - MinParams.efficiency) / DeltaY;
            var kcos = (MaxParams.cos - MinParams.cos) / DeltaY;

            return new Tuple<double, double, double, double, double>(kq, ks, ki, keff, kcos);
        }

        public List<PlotParams> PopulateTable(double[] Q, double[] H, double[] N, double[] Eff, Tuple<double, double, double, double> coefs, double DeltaPower, double DeltaY)
        {
            var (kq, kh, kn, keff) = coefs;

            var table = new List<PlotParams>();
            for (int i = 0; i < Q.Length; i++)
            {
                var data = new PlotParams();
                data.id = i + 1;
                data.power = (float)((Q[i] - DeltaPower) * kq);
                data.height = (float)((H[i] - DeltaY) * kh);
                data.kilowats = (float)((N[i] - DeltaY) * kn);
                data.efficiency = (float)((Eff[i] - DeltaY) * keff);
                table.Add(data);
            }

            return table;
        }
        public List<SemPlotParams> PopulateSemTable(double[] Q, double[] S, double[] I, double[] Eff, double[] Cos, Tuple<double, double, double, double, double> coefs, double DeltaX, double DeltaY)
        {
            var (kq, ks, ki, keff, kcos) = coefs;
            var table = new List<SemPlotParams>();
            for (int i = 0; i < Q.Length; i++)
            {
                var data = new SemPlotParams();
                data.id = i + 1;
                data.power = (float)((Q[i] - DeltaX) * kq);
                data.s = (float)((S[i] - DeltaY) * ks);
                data.i = (float)((I[i] - DeltaY) * ki);
                data.efficiency = (float)((Eff[i] - DeltaY) * keff);
                data.cos = (float)((Cos[i] - DeltaY) * kcos);
                table.Add(data);
            }

            return table;
        }
        public void SaveDataToCsv(StreamWriter writer, List<PlotParams> data)
        {
            writer.WriteLine("№;Q;H;N;Eff");
            foreach (PlotParams param in data)
                writer.WriteLine($"{param.id};{param.power};{param.height};{param.kilowats};{param.efficiency}");
        }
        public void SaveDataToCsv(StreamWriter writer, List<SemPlotParams> data)
        {
            writer.WriteLine("№;N;S;I;Eff;CosF");
            foreach (SemPlotParams param in data)
                writer.WriteLine($"{param.id};{param.power};{param.s};{param.i};{param.efficiency};{param.cos}");
        }

    }
}