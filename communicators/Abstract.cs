using OxyPlot;

namespace Aggregator.communicators
{
    public interface PlotComunicator
    {
        public static PlotModel Draw2D(double[] x, double[] y) { return new PlotModel(); }

    }

    public interface GraphicComunicator
    {
        public static void Draw3DModel(double[] x, double[] y, double[] z) { }
        public static void Draw3DScatter(double[] x, double[] y, double[] z) { }
    }
}