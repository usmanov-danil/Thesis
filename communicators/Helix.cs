using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using System.Windows;

namespace Aggregator.communicators
{
    public class Helix : GraphicComunicator
    {
        public static Point3D[] Draw3DScatter(double[] x, double[] y, double[] z) 
        {
            Point3D[] data = new Point3D[x.Length];
            for (int i = 0; i < x.Length; i++)
                data[i] = new Point3D(x[i], -y[i], z[i]);   // dirty hack with y
            return data;
        }

        public static Point3D[,] Draw3DModel(double[] x, double[] y, double[] z)
        {

            Point3D[,] data = new Point3D[x.Length, y.Length];
            for (int i = 0; i < x.Length; i++)
            {
                for (int j = 0; j < y.Length; j++)
                {
                    int z_index = (int)Math.Round((double)((i + j) / 2));
                    data[i, j] = new Point3D(x[i], y[j], z[z_index]);
                }
            }
            return data;
        }

        

    
    }
}
