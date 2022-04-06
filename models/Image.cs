using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aggregator.models
{
    internal class PlotParams
    {
        private float? power;
        private float? height;
        private float? kilowats;
        private float? efficiency;
    }
    public class Image
    {
        private byte[]? img;
        private string? manufacture_name;
        private string? ecp_brand;
        private PlotParams? optimal_params;
        private int? number_stages;

        private PlotParams? min_plot_params;
        private PlotParams? max_plot_params;
        private PlotParams? step_grid;

        private int? feed;
        private float? tab_step;
        private int num_of_table_points;

        private float? ro;
        private float? mu;
        private float? q;


        public Image(byte[] image)
        {
            img = image;
        }
    }
}
