using DTCDev.Client.Controls.Map;
using DTCDev.Client.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace DTCDev.Client.Cars.Engine.DisplayModels
{
    public class NavigationData : ViewModelBase
    {
        private Location _location;
        /// <summary>
        /// Текущее местоположение автомобиля
        /// </summary>
        public Location LocationPoint
        {
            get { return _location; }
            set
            {
                if (_location == value) return;
                _location = value;
                OnPropertyChanged("LocationPoint");
                if (_location.Latitude > 0 && _location.Longitude > 0 && VisOnMap == Visibility.Collapsed)
                    VisOnMap = Visibility.Visible;
            }
        }

        /// <summary>
        /// Строка с координатами автомобиля
        /// </summary>
        public string strLocation
        {
            get
            {
                return LocationPoint != null ? string.Format("{0}:{1}", Math.Ceiling(LocationPoint.Latitude), Math.Ceiling(LocationPoint.Longitude)) : "0:0";
            }
        }

        private Visibility _visOnMap = Visibility.Collapsed;
        /// <summary>
        /// Отображается ли автомобиль на карте
        /// </summary>
        public Visibility VisOnMap
        {
            get { return _visOnMap; }
            set
            {
                _visOnMap = value;
                this.OnPropertyChanged("VisOnMap");
            }
        }

        private double current_speed;
        /// <summary>
        /// Текущая скорость автомобиля
        /// </summary>
        public double Current_Speed
        {
            get { return current_speed; }
            set
            {
                if (current_speed != value)
                {
                    current_speed = value;
                    OnPropertyChanged("Current_Speed");
                    OnPropertyChanged("strSpeed");
                }
            }
        }


        /// <summary>
        /// Строка скорости автомобиля
        /// </summary>
        public string strSpeed
        {
            get
            {
                return string.Format("{0} km/h", Current_Speed);
            }
        }


        private string _dateNavigation = "";
        /// <summary>
        /// Дата последнего обновления навигационных данных автомобиля
        /// </summary>
        public string DateNavigation
        {
            get { return _dateNavigation; }
            set
            {
                _dateNavigation = value;
                this.OnPropertyChanged("DateNavigation");
            }
        }

        private DateTime _dateLastUpdate = new DateTime();
        public DateTime DateLastUpdate
        {
            get { return _dateLastUpdate; }
            set
            {
                _dateLastUpdate = value;
                this.OnPropertyChanged("DateLastUpdate");
            }
        }

        public string HistroryTime
        {
            get
            {
                return string.Format("{0:00}:{1:00}:{2:00}", DateLastUpdate.Hour, DateLastUpdate.Minute, DateLastUpdate.Second);
            }
        }

        private double _angle = 0;

        /// <summary>
        /// Текущий угол поворота автомобиля
        /// </summary>
        public double Angle
        {
            get { return _angle; }
            set
            {
                _angle = value;
                this.OnPropertyChanged("Angle");
            }
        }

        private string _countSattelites = "";

        /// <summary>
        /// Количество спутников
        /// </summary>
        public string CountSatelite
        {
            get { return _countSattelites; }
            set
            {
                _countSattelites = value;
                this.OnPropertyChanged("CountSatelite");
            }
        }
    }
}
