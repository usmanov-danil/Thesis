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
using System.Drawing.Imaging;
using NumSharp;
using Emgu;
using Emgu.CV.Structure;
using Emgu.CV;

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
        private byte[][] conver1dTo2d(byte[] arr, int w, int h)
        {
            byte[][] answer = new byte[h][];
            for (int i = 0; i < arr.Length; i++)
                answer[i / w][i % w] = arr[i];

            return answer;
        }
        
        public void ParsePlot(ViewModel model)
        {
            var image =  BitmapImageToBitmap(model.Image.Clone()).ToNDArray(flat: false, copy: false);
            Emgu.CV.Image<Bgr, Byte> myImage = new Emgu.CV.Image<Bgr, Byte>("C:\\Users\\User\\Desktop\\Расчет_РХ_ЭЦН\\DATA_ECN\\Алнас\\1.png");
            NDArray bottom = new int[] { model.Color1.R, model.Color1.G, model.Color1.B };
            NDArray upper = new int[] { model.Color1.R, model.Color1.G, model.Color1.B };


            bottom -= 10;
            upper += 10;

            Emgu.CV.ScalarArray bottom2 = new Emgu.CV.ScalarArray(new Emgu.CV.Structure.MCvScalar((byte)model.Color1.B - 10, (byte)model.Color1.G - 10, (byte)model.Color1.R - 10, (byte)model.Color1.A - 10));
            Emgu.CV.ScalarArray upper2 = new Emgu.CV.ScalarArray(new Emgu.CV.Structure.MCvScalar((byte)model.Color1.B + 10, (byte)model.Color1.G + 10, (byte)model.Color1.R + 10, (byte)model.Color1.A + 10));

            Emgu.CV.CvInvoke.InRange(myImage, bottom2, upper2, myImage);

            myImage.Save("C:\\Users\\User\\Desktop\\filename.png");
        }
      
        public static Bitmap BitmapImageToBitmap(BitmapImage bitmapImage)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                return new Bitmap(bitmap);
            }

           
        }





    }
}