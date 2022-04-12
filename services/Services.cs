using Aggregator.models;
using Aggregator.communicators;
using OxyPlot;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Media3D;
using System.Windows.Media.Imaging;
using System.Drawing;
using MathNet.Numerics;

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

        
        public void ParsePlot(ViewModel model)
        {
            List<double> x = new List<double>();
            List<double> y = new List<double>();
            var img = new Bitmap(Image.FromFile("C:\\Users\\User\\Desktop\\Расчет_РХ_ЭЦН\\DATA_ECN\\Алнас\\1.png"));
            for (int i = 0; i < img.Width; i++)
                for (int j = 0; j < img.Height; j++)
                {
                    int ret = (int)(model.Color1.R * 0.299 + model.Color1.G * 0.578 + model.Color1.B * 0.114);
                    Color pixel = img.GetPixel(i, j);
                    int g_pix  = (int)(pixel.R * 0.299 + pixel.G * 0.578 + pixel.B * 0.114);
                    if (g_pix > ret - 10 && g_pix < ret + 10)
                    {
                        x.Add(i);
                        y.Add(img.Height - j);
                        img.SetPixel(i, j, Color.Black);
                    }
                    else
                        img.SetPixel(i, j, Color.White);

                }

            int[] yy = y.ConvertAll(z => (int)z).ToArray();
            int[] xx = x.ConvertAll(z => (int)z).ToArray();
            var f = Fit.PolynomialFunc(x.ToArray(), y.ToArray(), 7);
            for (var i = 0; i < x.Count(); i++)
                yy[i] = (int)f(x.ElementAt(i));


            using (var sw = new StreamWriter("C:\\Users\\User\\Desktop\\data.csv"))
            {
                for (var i = 0; i < x.Count(); i++)
                {
                    yy[i] = (int)f(x.ElementAt(i));
                    sw.WriteLine($"{x.ElementAt(i)};{y.ElementAt(i)}", i);
                }
            }

        }
        





    }
}