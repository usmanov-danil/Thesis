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

        public Point3D[,] DrawWellModel3D(Data data)
        {
           return Helix.Draw3DModel(data.X.ToArray(), data.Y.ToArray(), data.Z.ToArray());
        }

        public Point3D[] DrawWellProfile3D(Data data)
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
        public Tuple<int[], int[]> ParsePlot(string Path, System.Windows.Media.Color TargetColor)
        {
            return ParsePlot(Path, System.Drawing.Color.FromArgb(TargetColor.A, TargetColor.R, TargetColor.G, TargetColor.B));
        }
        public Tuple<int[], int[]> ParsePlot(string Path, System.Drawing.Color TargetColor)
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

            int[] yy = y.ConvertAll(z => (int)z).ToArray();
            int[] xx = x.ConvertAll(z => (int)z).ToArray();

            var f = Fit.PolynomialFunc(x.ToArray(), y.ToArray(), Degree);
            for (var i = 0; i < x.Count(); i++)
                yy[i] = (int)f(x.ElementAt(i));

            return new Tuple<int[], int[]>(xx, yy);

        }
        





    }
}