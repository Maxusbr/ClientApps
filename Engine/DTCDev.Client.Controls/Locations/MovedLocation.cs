using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DTCDev.Client.Controls.Map
{
    public class MovedLocation : Location, System.ComponentModel.INotifyPropertyChanged
    {
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

        public MovedLocation(double latitude, double longitude)
            : base(latitude, longitude)
        {
            Current = this;
        }

        public MovedLocation(Location location)
            : base(location.Latitude, location.Longitude)
        { Current = this; }

        Location _Current;
        public new Location Current
        {
            get
            {
                return _Current;
            }
            set
            {
                _Current = value;
                OnPropertyChanged("Current");
            }
        }

        private DateTime _dates;
        public DateTime Dates
        {
            get
            {
                return _dates;
            }
            set
            {
                _dates = value;
                OnPropertyChanged("Dates");
            }
        }

        private bool enableEdit = false;
        public bool EnableEdit
        {
            get { return enableEdit; }
            set
            {
                if (this.enableEdit != value)
                {
                    this.enableEdit = value;
                    OnPropertyChanged("EnableEdit");
                }
            }
        }

        public void Update()
        {
            OnPropertyChanged("Current");
        }
        public void Remove()
        {
            OnPropertyChanged("Remove");
        }

        public new static MovedLocation Parse(string s)
        {
            var tokens = s.Split(new char[] { ',' });
            if (tokens.Length != 2)
            {
                throw new FormatException("Location string must be a comma-separated pair of double values");
            }

            return new MovedLocation(
                double.Parse(tokens[0], NumberStyles.Float, CultureInfo.InvariantCulture),
                double.Parse(tokens[1], NumberStyles.Float, CultureInfo.InvariantCulture));
        }
    }    
}
