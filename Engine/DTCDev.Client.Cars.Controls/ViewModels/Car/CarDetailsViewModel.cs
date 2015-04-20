using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using DTCDev.Client.Cars.Engine.DisplayModels;
using DTCDev.Client.Controls.Map;
using DTCDev.Client.ViewModel;

namespace DTCDev.Client.Cars.Controls.ViewModels.Car
{
    public class CarDetailsViewModel : ViewModelBase
    {
        public CarDetailsViewModel()
        {
            MapCenter = new Location(55.75, 37.62);
        }

        private Location mapCenter;
        public Location MapCenter
        {
            get { return mapCenter; }
            set
            {
                if (mapCenter != value)
                {
                    mapCenter = value;
                    OnPropertyChanged("MapCenter");
                }
            }
        }

        private Location mapCenterUser;
        public Location MapCenterUser
        {
            get { return mapCenterUser; }
            set
            {
                if (mapCenterUser != value)
                {
                    mapCenterUser = value;
                    OnPropertyChanged("MapCenterUser");
                }
            }
        }

        DISP_Car _car;
        public DISP_Car Car
        {
            get { return _car; }
            set
            {
                if (_car == value) return;
                _car = value;
                OnPropertyChanged("Car");
            }
        }

    }
}
