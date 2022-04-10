using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aggregator.models
{
    public class PlotParams
    {
        private float _power = 0;
        private float _height = 0;
        private float _kilowats = 0;
        private float _efficiency = 0;

        public float power
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
        public float height
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
        public float kilowats
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
        public float efficiency
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
