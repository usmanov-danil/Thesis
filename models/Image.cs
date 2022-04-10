

namespace Aggregator.models
{
    public class PlotParams
    {
        public float? power;
        public float? height;
        public float? kilowats;
        public float? efficiency;

        public bool IsDataExists()
        {
            return this.power != null &&
                this.height != null &&
                this.efficiency != null &&
                this.efficiency != null;
        }
    }
    public class Image
    {
        public byte[]? img;
        public string? manufacture_name;
        public string? ecp_brand;
        public PlotParams optimal_params = new PlotParams();
        public int? number_stages;

        public PlotParams min_plot_params = new PlotParams();
        public PlotParams max_plot_params = new PlotParams();
        //public PlotParams step_grid = new PlotParams();

        //public int? feed;
        //public float? tab_step;
        //public int num_of_table_points;

        public float? ro;
        public float? mu;
        public float? q;

        public bool IsDataExists()
        {
            return this.img != null &&
                this.manufacture_name != null &&
                this.ecp_brand != null &&
                this.number_stages != null &&
                this.ro != null &&
                this.mu != null &&
                this.q != null &&
                this.max_plot_params.IsDataExists() &&
                this.min_plot_params.IsDataExists() &&
                this.optimal_params.IsDataExists();
        }
    }
}
