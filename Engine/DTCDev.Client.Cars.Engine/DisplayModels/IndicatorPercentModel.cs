using DTCDev.Client.ViewModel;

namespace DTCDev.Client.Cars.Engine.DisplayModels
{
    public class IndicatorPercentModel : ViewModelBase
    {
        private double _maxValue = int.MaxValue;
        private double _value = 0;

        public IndicatorPercentModel(double maxValue)
        {
            _maxValue = maxValue;
            _value = 0;
        }

        public void Update(double maxValue, double value)
        {
            _maxValue = maxValue;
            _value = value;
            OnPropertyChanged("Percent100");
            OnPropertyChanged("Percent80");
            OnPropertyChanged("Percent60");
            OnPropertyChanged("Percent40");
            OnPropertyChanged("Percent20");
        }

        public void Update(double value)
        {
            _value = value;
            OnPropertyChanged("Percent100");
            OnPropertyChanged("Percent80");
            OnPropertyChanged("Percent60");
            OnPropertyChanged("Percent40");
            OnPropertyChanged("Percent20");
        }

        public bool Percent100
        {
            get { return _value/_maxValue >= 1; }
        }

        public bool Percent80
        {
            get { return _value/_maxValue >= .8; }
        }

        public bool Percent60
        {
            get { return _value / _maxValue >= .6; }
        }

        public bool Percent40
        {
            get { return _value / _maxValue >= .4; }
        }

        public bool Percent20
        {
            get { return _value / _maxValue >= .2; }
        }
    }
}
