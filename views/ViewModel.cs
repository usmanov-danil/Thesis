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

        // Images
        private ColorToBrushConverter _ColorToBrushConverter = new ColorToBrushConverter();
        #region ECP
        // Image processing 1
        private string? _ImagePath1;
        private BitmapImage? _Image1;
        private string _FactoryName1 = "";
        private string _EcpBrand1 = "";
        private int _Stages1 = 5;
        private double _Steps1 = 2.5;
        private PlotParams _OptimalParams1 = new PlotParams();
        private PlotParams _MinPlotParams1 = new PlotParams();
        private PlotParams _MaxPlotParams1 = new PlotParams();
        private double _Mu1 = 0;
        private double _Ro1 = 0;
        private double _Q1 = 0;
        private bool _HIsChecked1 = true;
        private bool _NIsChecked1 = true;
        private bool _EffIsChecked1 = true;
        private Color _Color11;
        private Color _Color12;
        private Color _Color13;
        private Brush? _ColorBrush11;
        private Brush? _ColorBrush12;
        private Brush? _ColorBrush13;
        private Point? _OriginPoint1 = null;
        private Point? _XPoint1 = null;
        private Point? _YPoint1 = null;
        private List<PlotParams> _Table1;

        public string ImagePath1
        {
            get { return _ImagePath1; }
            set
            {
                if (value != _ImagePath1)
                {
                    _ImagePath1 = value;
                    OnPropertyChanged();
                }
            }
        }
        public BitmapImage Image1
        {
            get { return _Image1; }
            set
            {
                if (value != _Image1)
                {
                    _Image1 = value;
                    OnPropertyChanged();
                }
            }
        }
        public string FactoryName1
        {
            get { return _FactoryName1; }
            set
            {
                if (value != _FactoryName1)
                {
                    _FactoryName1 = value;
                    OnPropertyChanged();
                }
            }
        }
        public string EcpBrand1
        {
            get { return _EcpBrand1; }
            set
            {
                if (value != _EcpBrand1)
                {
                    _EcpBrand1 = value;
                    OnPropertyChanged();
                }
            }
        }
        public int Stages1
        {
            get { return _Stages1; }
            set
            {
                if (value != _Stages1)
                {
                    _Stages1 = value;
                    OnPropertyChanged();
                }
            }
        }
        public double Steps1
        {
            get { return _Steps1; }
            set
            {
                if (value != _Steps1)
                {
                    _Steps1 = value;
                    OnPropertyChanged();
                }
            }
        }
        public PlotParams OptimalParams1
        {
            get { return _OptimalParams1; }
            set
            {
                _OptimalParams1 = value;
                OnPropertyChanged();
            }
        }
        public PlotParams MinPlotParams1
        {
            get { return _MinPlotParams1; }
            set
            {
                _MinPlotParams1 = value;
                OnPropertyChanged(nameof(value));
            }
        }
        public PlotParams MaxPlotParams1
        {
            get { return _MaxPlotParams1; }
            set
            {
                _MaxPlotParams1 = value;
                OnPropertyChanged(nameof(value));
            }
        }
        public double Mu1
        {
            get { return _Mu1; }
            set
            {
                if (value != _Mu1)
                {
                    _Mu1 = value;
                    OnPropertyChanged();
                }
            }
        }
        public double Ro1
        {
            get { return _Ro1; }
            set
            {
                if (value != _Ro1)
                {
                    _Ro1 = value;
                    OnPropertyChanged();
                }
            }
        }
        public double Q1
        {
            get { return _Q1; }
            set
            {
                if (value != _Q1)
                {
                    _Q1 = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool HIsChecked1
        {
            get { return _HIsChecked1; }
            set
            {
                if (value != _HIsChecked1)
                {
                    _HIsChecked1 = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool NIsChecked1
        {
            get { return _NIsChecked1; }
            set
            {
                if (value != _NIsChecked1)
                {
                    _NIsChecked1 = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool EffIsChecked1
        {
            get { return _EffIsChecked1; }
            set
            {
                if (value != _EffIsChecked1)
                {
                    _EffIsChecked1 = value;
                    OnPropertyChanged();
                }
            }
        }
        public Color Color11
        {
            get { return _Color11; }
            set
            {
                if (value != _Color11)
                {
                    _Color11 = value;
                    ColorBrush11 = (Brush)_ColorToBrushConverter.Convert(_Color11, typeof(Brush), null, null);
                    OnPropertyChanged();
                }
            }
        }
        public Color Color12
        {
            get { return _Color12; }
            set
            {
                if (value != _Color12)
                {
                    _Color12 = value;
                    ColorBrush12 = (Brush)_ColorToBrushConverter.Convert(_Color12, typeof(Brush), null, null);
                    OnPropertyChanged();
                }
            }
        }
        public Color Color13
        {
            get { return _Color13; }
            set
            {
                if (value != _Color13)
                {
                    _Color13 = value;
                    ColorBrush13 = (Brush)_ColorToBrushConverter.Convert(_Color13, typeof(Brush), null, null);
                    OnPropertyChanged();
                }
            }
        }
        public Brush ColorBrush13
        {
            get { return _ColorBrush13; }
            set { _ColorBrush13 = value; OnPropertyChanged(); }
        }
        public Brush ColorBrush11
        {
            get { return _ColorBrush11; }
            set { _ColorBrush11 = value; OnPropertyChanged(); }
        }
        public Brush ColorBrush12
        {
            get { return _ColorBrush12; }
            set { _ColorBrush12 = value; OnPropertyChanged(); }
        }
        public Point OriginPoint1
        {
            get { return (Point)_OriginPoint1; }
            set { _OriginPoint1 = value; OnPropertyChanged(); }
        }
        public Point XPoint1
        {
            get { return (Point)_XPoint1; }
            set { _XPoint1 = value; OnPropertyChanged(); }
        }
        public Point YPoint1
        {
            get { return (Point)_YPoint1; }
            set { _YPoint1 = value; OnPropertyChanged(); }
        }
        public List<PlotParams> Table1
        {
            get { return _Table1; }
            set { _Table1 = value; OnPropertyChanged(); }
        }
        public bool IsPoints1Exist()
        {
            return _OriginPoint1 != null && _XPoint1 != null && _YPoint1 != null;
        }
        #endregion


        #region SEM
        private string? _ImagePath2;
        private BitmapImage? _Image2;
        private string _FactoryName2 = "";
        private string _SemBrand = "";
        private SemPlotParams _MinPlotParams2 = new SemPlotParams();
        private SemPlotParams _MaxPlotParams2 = new SemPlotParams();
        private double _Steps2 = 2.5;
        private Point? _OriginPoint2 = null;
        private Point? _XPoint2 = null;
        private Point? _YPoint2 = null;
        private Color _Color21;
        private Color _Color22;
        private Color _Color23;
        private Color _Color24;
        private Brush? _ColorBrush21;
        private Brush? _ColorBrush22;
        private Brush? _ColorBrush23;
        private Brush? _ColorBrush24;
        private bool _SIsChecked2 = true;
        private bool _IIsChecked2 = true;
        private bool _EffIsChecked2 = true;
        private bool _CosIsChecked2 = true;
        private List<SemPlotParams> _Table2;

        public string ImagePath2
        {
            get { return _ImagePath2; }
            set
            {
                if (value != _ImagePath2)
                {
                    _ImagePath2 = value;
                    OnPropertyChanged();
                }
            }
        }
        public BitmapImage Image2
        {
            get { return _Image2; }
            set
            {
                if (value != _Image2)
                {
                    _Image2 = value;
                    OnPropertyChanged();
                }
            }
        }
        public string FactoryName2
        {
            get { return _FactoryName2; }
            set
            {
                if (value != _FactoryName2)
                {
                    _FactoryName2 = value;
                    OnPropertyChanged();
                }
            }
        }
        public string SemBrand
        {
            get { return _SemBrand; }
            set
            {
                if (value != _SemBrand)
                {
                    _SemBrand = value;
                    OnPropertyChanged();
                }
            }
        }
        public SemPlotParams MinPlotParams2
        {
            get { return _MinPlotParams2; }
            set
            {
                _MinPlotParams2 = value;
                OnPropertyChanged(nameof(value));
            }
        }
        public SemPlotParams MaxPlotParams2
        {
            get { return _MaxPlotParams2; }
            set
            {
                _MaxPlotParams2 = value;
                OnPropertyChanged(nameof(value));
            }
        }
        public double Steps2
        {
            get { return _Steps2; }
            set
            {
                if (value != _Steps2)
                {
                    _Steps2 = value;
                    OnPropertyChanged();
                }
            }
        }
        public Point OriginPoint2
        {
            get { return (Point)_OriginPoint2; }
            set { _OriginPoint2 = value; OnPropertyChanged(); }
        }
        public Point XPoint2
        {
            get { return (Point)_XPoint2; }
            set { _XPoint2 = value; OnPropertyChanged(); }
        }
        public Point YPoint2
        {
            get { return (Point)_YPoint2; }
            set { _YPoint2 = value; OnPropertyChanged(); }
        }
        public Color Color21
        {
            get { return _Color21; }
            set
            {
                if (value != _Color21)
                {
                    _Color21 = value;
                    ColorBrush21 = (Brush)_ColorToBrushConverter.Convert(_Color21, typeof(Brush), null, null);
                    OnPropertyChanged();
                }
            }
        }
        public Color Color22
        {
            get { return _Color22; }
            set
            {
                if (value != _Color22)
                {
                    _Color22 = value;
                    ColorBrush22 = (Brush)_ColorToBrushConverter.Convert(_Color22, typeof(Brush), null, null);
                    OnPropertyChanged();
                }
            }
        }
        public Color Color23
        {
            get { return _Color23; }
            set
            {
                if (value != _Color23)
                {
                    _Color23 = value;
                    ColorBrush23 = (Brush)_ColorToBrushConverter.Convert(_Color23, typeof(Brush), null, null);
                    OnPropertyChanged();
                }
            }
        }
        public Color Color24
        {
            get { return _Color24; }
            set
            {
                if (value != _Color24)
                {
                    _Color24 = value;
                    ColorBrush24 = (Brush)_ColorToBrushConverter.Convert(_Color24, typeof(Brush), null, null);
                    OnPropertyChanged();
                }
            }
        }
        
        public Brush ColorBrush21
        {
            get { return _ColorBrush21; }
            set { _ColorBrush21 = value; OnPropertyChanged(); }
        }
        public Brush ColorBrush22
        {
            get { return _ColorBrush22; }
            set { _ColorBrush22 = value; OnPropertyChanged(); }
        }
        public Brush ColorBrush23
        {
            get { return _ColorBrush23; }
            set { _ColorBrush23 = value; OnPropertyChanged(); }
        }
        public Brush ColorBrush24
        {
            get { return _ColorBrush24; }
            set { _ColorBrush24 = value; OnPropertyChanged(); }
        }
        public bool SIsChecked2
        {
            get { return _SIsChecked2; }
            set
            {
                if (value != _SIsChecked2)
                {
                    _SIsChecked2 = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool IIsChecked2
        {
            get { return _IIsChecked2; }
            set
            {
                if (value != _IIsChecked2)
                {
                    _IIsChecked2 = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool EffIsChecked2
        {
            get { return _EffIsChecked2; }
            set
            {
                if (value != _EffIsChecked2)
                {
                    _EffIsChecked2 = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool CosIsChecked2
        {
            get { return _CosIsChecked2; }
            set
            {
                if (value != _CosIsChecked2)
                {
                    _CosIsChecked2 = value;
                    OnPropertyChanged();
                }
            }
        }
        public List<SemPlotParams> Table2
        {
            get { return _Table2; }
            set { _Table2 = value; OnPropertyChanged(); }
        }

        public bool IsPoints2Exist()
        {
            return _OriginPoint2 != null && _XPoint2 != null && _YPoint2 != null;
        }
        #endregion
    }

    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] String propName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}