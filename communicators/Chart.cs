//
using System;
using OxyPlot;

namespace Aggregator.communicators
{
    public class Charting : PlotComunicator
    {


        public static void Draw3D(double[] x, double[] y, double[] z)
        {
            throw new NotImplementedException();
        }

        public static PlotModel Draw2D(double[] x, double[] y, OxyColor color)
        {
           PlotModel model = new PlotModel();
           OxyPlot.Series.LineSeries line = new OxyPlot.Series.LineSeries()
           {
                StrokeThickness = 1,
                MarkerSize = 2,
                MarkerType = MarkerType.Circle,
                Color = color
           };

           for (int i = 0; i < x.Length; i++)
                line.Points.Add(new DataPoint(x[i], y[i]));

            model.Series.Add(line);
            model.PlotType = PlotType.XY;

            return model;
        }
    }
}