using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aggregator.models
{
    public class PlotParams
    {
        private string _power = "0.0";
        private string _height = "0.0";
        private string _kilowats = "0.0";
        private string _efficiency = "0.0";

        public string power
        {
            get { return _power; }
            set
            {
                if (value != _power)
                {
                    _power = value;
                }
            }
        }
        public string height
        {
            get { return _height; }
            set
            {
                if (value != _height)
                {
                    _height = value;
                }
            }
        }
        public string kilowats
        {
            get { return _kilowats; }
            set
            {
                if (value != _kilowats)
                {
                    _kilowats = value;
                }
            }
        }
        public string efficiency
        {
            get { return _efficiency; }
            set
            {
                if (value != _efficiency)
                {
                    _efficiency = value;
                }
            }
        }
    }
}
