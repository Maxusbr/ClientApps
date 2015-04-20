using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using DTCDev.Client.ViewModel;

namespace DTCDev.Client.Cars.Controls.ViewModels.Car
{
    public class CarInOutViewModel : ViewModelBase
    {
        private string _carId;
        private string _carNumber;
        private DateTime _inZone = new DateTime(1, 1, 1);
        private DateTime _outZone = new DateTime(1, 1, 1);
        private string _zoneName;
        private int _zoneId;

        public CarInOutViewModel() { }

        public string CarID
        {
            get { return _carId; }
            set
            {
                _carId = value;
                this.OnPropertyChanged("CarID");
            }
        }

        public string CarNumber
        {
            get { return _carNumber; }
            set
            {
                _carNumber = value;
                this.OnPropertyChanged("CarNumber");
            }
        }

        public int ZoneId
        {
            get { return _zoneId; }
            set
            {
                _zoneId = value;
                this.OnPropertyChanged("ZoneId");
            }
        }

        public string ZoneName
        {
            get { return _zoneName; }
            set
            {
                _zoneName = value;
                this.OnPropertyChanged("ZoneName");
            }
        }

        public DateTime InZoneDate
        {
            get { return _inZone; }
            set
            {
                _inZone = value;
                this.OnPropertyChanged("StrInZoneDate");
                OnPropertyChanged("IsInZone");
            }
        }
        public string StrInZoneDate { get { return InZoneDate.ToString("dd.MM.yyyy hh:mm"); } }
        public DateTime OutZoneDate
        {
            get { return _outZone; }
            set
            {
                _outZone = value;
                this.OnPropertyChanged("StrOutZoneDate");
                OnPropertyChanged("IsOutZone");
            }
        }
        public string StrOutZoneDate { get { return OutZoneDate.ToString("dd.MM.yyyy hh:mm"); } }
        public bool IsInZone
        {
            get { return InZoneDate != new DateTime(1, 1, 1); }
        }

        public bool IsOutZone
        {
            get { return  OutZoneDate != new DateTime(1, 1, 1); }
        }

        private ICommand _okCommand;
        public ICommand OkCommand
        {
            get { return _okCommand ?? (_okCommand = new RelayCommand(OKHandler)); }
        }

        private void OKHandler(object obj)
        {
            OnPropertyChanged("RemoveElement");
        }
    }
}
