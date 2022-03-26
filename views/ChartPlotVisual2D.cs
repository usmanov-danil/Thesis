using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using OxyPlot;

namespace Aggregator
{
    public class ChartPlotVisual2D : ViewModelBase
    {
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