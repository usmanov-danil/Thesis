using OxyPlot;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Aggregator.models;

namespace Aggregator
{
    public class ViewModel : ViewModelBase
    {
        // Plots 2D
        private PlotModel _Chart1Model;
        private PlotModel _Chart2Model;
        private PlotModel _Chart3Model;
        public PlotModel Chart1Model
        {
            get { return _Chart1Model; }
            set
            {
                if (value != _Chart1Model)
                {
                    _Chart1Model = value;
                    OnPropertyChanged();
                }
            }
        }
        public PlotModel Chart2Model
        {
            get { return _Chart2Model; }
            set
            {
                if (value != _Chart2Model)
                {
                    _Chart2Model = value;
                    OnPropertyChanged();
                }
            }
        }
        public PlotModel Chart3Model
        {
            get { return _Chart3Model; }
            set
            {
                if (value != _Chart3Model)
                {
                    _Chart3Model = value;
                    OnPropertyChanged();
                }
            }
        }

        // Image processing
        private string _FactoryName = "";
        private string _EcpBrand = "";
        private string _Stages = "5";
        private PlotParams _OptimalParams = new PlotParams();
        private PlotParams _MinPlotParams = new PlotParams();
        private PlotParams _MaxPlotParams = new PlotParams();
        private string _Mu = "0.0";
        private string _Ro= "0.0";
        private string _Q = "0.0";
        private bool _HIsChecked = true;
        private bool _NIsChecked = true;
        private bool _EffIsChecked = true;

        //   (float) Convert.ToDouble(OptEff.Text);
        public string FactoryName
        {
            get { return _FactoryName; }
            set
            {
                if (value != _FactoryName)
                {
                    _FactoryName = value;
                    OnPropertyChanged();
                }
            }
        }
        public string EcpBrand
        {
            get { return _EcpBrand; }
            set
            {
                if (value != _EcpBrand)
                {
                    _EcpBrand = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Stages
        {
            get { return _Stages.ToString(); }
            set
            {
                if (value != _Stages.ToString())
                {
                    _Stages = value;
                    OnPropertyChanged();
                }
            }
        }
        public PlotParams OptimalParams
        {
            get { return _OptimalParams; }
            set
            {
                if (value != _OptimalParams)
                {
                    _OptimalParams = value;
                    OnPropertyChanged();
                }
            }
        }
        public PlotParams MinPlotParams
        {
            get { return _MinPlotParams; }
            set
            {
                if (value != _MinPlotParams)
                {
                    _MinPlotParams = value;
                    OnPropertyChanged();
                }
            }
        }
        public PlotParams MaxPlotParams
        {
            get { return _MaxPlotParams; }
            set
            {
                if (value != _MaxPlotParams)
                {
                    _MaxPlotParams = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Mu
        {
            get { return _Mu; }
            set
            {
                if (value != _Mu)
                {
                    _Mu = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Ro
        {
            get { return _Ro; }
            set
            {
                if (value != _Ro)
                {
                    _Ro = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Q
        {
            get { return _Q; }
            set
            {
                if (value != _Q)
                {
                    _Q = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool HIsChecked
        {
            get { return _HIsChecked; }
            set
            {
                if (value != _HIsChecked)
                {
                    _HIsChecked = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool NIsChecked
        {
            get { return _NIsChecked; }
            set
            {
                if (value != _NIsChecked)
                {
                    _NIsChecked = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool EffIsChecked
        {
            get { return _EffIsChecked; }
            set
            {
                if (value != _EffIsChecked)
                {
                    _EffIsChecked = value;
                    OnPropertyChanged();
                }
            }
        }


    }

    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] String propName = null)
        {
            // C#6.O
            // PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}