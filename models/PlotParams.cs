using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aggregator.models
{
    public class PlotParams
    {
        private int _id; 
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
        public int id
        {
            get { return _id; }
            set
            {
                _id = value;
            }
        }
    }
    public class SemPlotParams
    {
        private int _id;
        private float _power = 0;
        private float _s = 0;
        private float _I = 0;
        private float _efficiency = 0;
        private float _cos = 0;

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
        public float s
        {
            get { return _s; }
            set
            {
                if (value != _s)
                {
                    _s = value;
                }
            }
        }
        public float i
        {
            get { return _I; }
            set
            {
                if (value != _I)
                {
                    _I = value;
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
        public float cos
        {
            get { return _cos; }
            set
            {
                if (value != _cos)
                {
                    _cos = value;
                }
            }
        }
        public int id
        {
            get { return _id; }
            set
            {
                _id = value;
            }
        }
    }
}
