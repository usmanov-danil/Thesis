using Aggregator.models;
using Aggregator.communicators;
using OxyPlot;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Media3D;

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

        public byte[] LoadImage(string Path)
        { 
            return File.ReadAllBytes(Path);
        }
        private byte[][] conver1dTo2d(byte[] arr, int w, int h)
        {
            byte[][] answer = new byte[h][];
            for (int i = 0; i < arr.Length; i++)
                answer[i / w][i % w] = arr[i];

            return answer;
        }
        
        public void ParsePlot()
        {

        }
        //  private byte[][] ImageToArray(System.Drawing.Image img)
        // {
        //      using (var ms = new MemoryStream())
        //     {
        //         img.Save(ms, img.RawFormat);
        //        return this.conver1dTo2d(ms.ToArray(), img.Width, img.Height);
        //  }
        // }
        //   public Tuple<List<int>, List<int>> ExtractPointsFromPic(System.Drawing.Image img)
        // {
        //  byte[][] byte_img = this.ImageToArray(img);
        //  List<int> x = new();
        //  List<int> y = new();

        //   for (int i=0; i < byte_img.Length; i++)
        //      for (int j=0; j < byte_img[i].Length; j++)
        //     {
        //         if (byte_img[i][j] > 125)
        //          {
        //x.Add(i);
        //   y.Add(j);
        //   }

        //}

        //      return Tuple.Create(x, y);
        // }
        // }
    }
}