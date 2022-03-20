using System;
using System.Collections.Generic;
using System.Linq;

namespace Aggregator.models
{
    public class Data
    {
        public string Id = "";
        public List<double> Angle = new();
        public List<double> Azimuth = new();
        public List<double> X = new();
        public List<double> Y = new();
        public List<double> Z = new();

        public static Data FromCsv(string[] csv, string delimiter)
        {
            Data dataValues = new Data();

            dataValues.Id = csv[0].Split(delimiter)[0];
            dataValues.Angle.Add(0.0);
            dataValues.Azimuth.Add(0.0);
            dataValues.X.Add(0.0);
            dataValues.Y.Add(0.0);
            dataValues.Z.Add(0.0);

            for (int i=1; i<csv.Length; i++)
            {
                string[] prev_values = csv[i-1].Split(delimiter);
                string[] values = csv[i].Split(delimiter);

                double depht_diff = Convert.ToDouble(values[1]) - Convert.ToDouble(prev_values[1]);

                double AngleRec = Convert.ToDouble(values[3]) * Math.PI / 180;
                double Azimuth = Convert.ToDouble(values[4]) * Math.PI / 180;
                double X = depht_diff * Math.Sin(AngleRec) * Math.Cos(Azimuth) + dataValues.X.ElementAt(i - 1);
                double Y = depht_diff * Math.Sin(AngleRec) * Math.Sin(Azimuth) + dataValues.Y.ElementAt(i - 1);
                double Z = depht_diff * Math.Cos(AngleRec) + dataValues.Z.ElementAt(i - 1);

                dataValues.Angle.Add(AngleRec);
                dataValues.Azimuth.Add(Azimuth);
                dataValues.X.Add(X);
                dataValues.Y.Add(Y);
                dataValues.Z.Add(Z);
            }

            return dataValues;
        }

        public int Length()
        {
            return Y.Count;
        }
    }
}