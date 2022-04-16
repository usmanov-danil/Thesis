

using System.Linq;

namespace Aggregator.models
{
    public class TabledData
    {
        private double[] _Q;
        private double[] _N;
        private double[] _H;
        private double[] _Eff;
        public int[] Id { 
            get 
            { 
                if (Q != null)
                    return Enumerable.Range(1, Q.Length + 1).ToArray();
                else 
                    return System.Array.Empty<int>();
            } 
        }

        public double[] Q { 
            get
            {
                return _Q;
            } 
            set
            {
                _Q = value;
            } 
        }
        public double[] H
        {
            get
            {
                return _H;
            }
            set
            {
                _H = value;
            }
        }
        public double[] N
        {
            get
            {
                return _N;
            }
            set
            {
                _N = value;
            }
        }
        public double[] Eff
        {
            get
            {
                return _Eff;
            }
            set
            {
                _Eff = value;
            }
        }
    }
    
}
