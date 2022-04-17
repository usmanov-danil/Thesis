using OxyPlot;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Aggregator.models;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Aggregator.services;
using System.Windows;
using System.Collections.Generic;

namespace Aggregator
{
    public class ViewModel : ViewModelBase
    {
        // Plots 2D
        private PlotModel? _Chart1Model;
        private PlotModel? _Chart2Model;
        private PlotModel? _Chart3Model;
        private PlotModel? _ChartImageModel;
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
        public PlotModel ChartImageModel
        {
            get { return _ChartImageModel; }
            set
            {
                if (value != _ChartImageModel)
                {
                    _ChartImageModel = value;
                    OnPropertyChanged();
                }
            }
        }


        // Image processing
        private string? _ImagePath;
        private BitmapImage? _Image;
        private string _FactoryName = "";
        private string _EcpBrand = "";
        private int _Stages = 5;
        private double _Steps = 2.5;
        private PlotParams _OptimalParams = new PlotParams();
        private PlotParams _MinPlotParams = new PlotParams();
        private PlotParams _MaxPlotParams = new PlotParams();
        private double _Mu = 0;
        private double _Ro = 0;
        private double _Q = 0;
        private bool _HIsChecked = true;
        private bool _NIsChecked = true;
        private bool _EffIsChecked = true;
        private Color _Color1;
        private Color _Color2;
        private Color _Color3;
        private Brush? _ColorBrush1;
        private Brush? _ColorBrush2;
        private Brush? _ColorBrush3;
        private ColorToBrushConverter _ColorToBrushConverter = new ColorToBrushConverter();
        private Point? _OriginPoint = null;
        private Point? _XPoint = null;
        private Point? _YPoint = null;
        private List<PlotParams> _Table;

        public string ImagePath
        {
            get { return _ImagePath; }
            set
            {
                if (value != _ImagePath)
                {
                    _ImagePath = value;
                    OnPropertyChanged();
                }
            }
        }
        public BitmapImage Image
        {
            get { return _Image; }
            set
            {
                if (value != _Image)
                {
                    _Image = value;
                    OnPropertyChanged();
                }
            }
        }
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
        public int Stages
        {
            get { return _Stages; }
            set
            {
                if (value != _Stages)
                {
                    _Stages = value;
                    OnPropertyChanged();
                }
            }
        }
        public double Steps
        {
            get { return _Steps; }
            set
            {
                if (value != _Steps)
                {
                    _Steps = value;
                    OnPropertyChanged();
                }
            }
        }
        public PlotParams OptimalParams
        {
            get { return _OptimalParams; }
            set
            {
                _OptimalParams = value;
                OnPropertyChanged();
            }
        }
        public PlotParams MinPlotParams
        {
            get { return _MinPlotParams; }
            set
            {
                _MinPlotParams = value;
                OnPropertyChanged(nameof(value));
            }
        }
        public PlotParams MaxPlotParams
        {
            get { return _MaxPlotParams; }
            set
            {
                _MaxPlotParams = value;
                OnPropertyChanged(nameof(value));
            }
        }

        public double Mu
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
        public double Ro
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
        public double Q
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

        public Color Color1
        {
            get { return _Color1; }
            set
            {
                if (value != _Color1)
                {
                    _Color1 = value;
                    ColorBrush1 = (Brush)_ColorToBrushConverter.Convert(_Color1, typeof(Brush), null, null);
                    OnPropertyChanged();
                }
            }
        }
        public Color Color2
        {
            get { return _Color2; }
            set
            {
                if (value != _Color2)
                {
                    _Color2 = value;
                    ColorBrush2 = (Brush)_ColorToBrushConverter.Convert(_Color2, typeof(Brush), null, null);
                    OnPropertyChanged();
                }
            }
        }
        public Color Color3
        {
            get { return _Color3; }
            set
            {
                if (value != _Color3)
                {
                    _Color3 = value;
                    ColorBrush3 = (Brush)_ColorToBrushConverter.Convert(_Color3, typeof(Brush), null, null);
                    OnPropertyChanged();
                }
            }
        }
        public Brush ColorBrush3
        {
            get { return _ColorBrush3; }
            set { _ColorBrush3 = value; OnPropertyChanged(); }
        }
        public Brush ColorBrush1
        {
            get { return _ColorBrush1; }
            set { _ColorBrush1 = value; OnPropertyChanged(); }
        }
        public Brush ColorBrush2
        {
            get { return _ColorBrush2; }
            set { _ColorBrush2 = value; OnPropertyChanged(); }
        }

        public Point OriginPoint
        {
            get { return (Point)_OriginPoint; }
            set { _OriginPoint = value; OnPropertyChanged(); }
        }
        public Point XPoint
        {
            get { return (Point)_XPoint; }
            set { _XPoint = value; OnPropertyChanged(); }
        }
        public Point YPoint
        {
            get { return (Point)_YPoint; }
            set { _YPoint = value; OnPropertyChanged(); }
        }
        public List<PlotParams> Table
        {
            get { return _Table; }
            set { _Table = value; OnPropertyChanged(); }
        }







        public bool IsPointsExist()
        {
            return _OriginPoint != null && _XPoint != null && _YPoint != null;
        }
    }

    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] String propName = null)
        {
            // C#6.O
            // PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}