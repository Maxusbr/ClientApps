using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace DTCDev.Models.DeviceSender.DISP
{
    public class DevicePresenter :BaseViewModel
    {
        public int id { get; set; }

        public string DID { get; set; }

        private string _name = "";

        /// <summary>
        /// DeviceName
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                this.OnPropertyChanged("Name");
            }
        }

        private bool _isOnline = false;
        /// <summary>
        /// Is device online
        /// </summary>
        public bool IsOnline
        {
            get { return _isOnline; }
            set
            {
                _isOnline = value;
                this.OnPropertyChanged("IsOnline");
                if (IsOnline == false)
                {
                    LastUpdateError = LastUpdateString;
                }
                else
                    LastUpdateError = "";
            }
        }

        private bool _isGuard = false;
        public bool IsGuard
        {
            get { return _isGuard; }
            set
            {
                _isGuard = value;
                this.OnPropertyChanged("IsGuard");
            }
        }

        private List<Sensor> _sensors = new List<Sensor>();

        public List<Sensor> Sensors
        {
            get { return _sensors; }
            set
            {
                _sensors = value;
                this.OnPropertyChanged("Sensors");
            }
        }

        private List<int> _outStates = new List<int>();
        public List<int> OutStates
        {
            get { return _outStates; }
            set
            {
                _outStates = value;
                this.OnPropertyChanged("OutStates");
            }
        }

        private int _controllerType = 1;

        /// <summary>
        /// Controller type
        /// </summary>
        public int CT
        {
            get { return _controllerType; }
            set
            {
                _controllerType = value;
            }
        }

        private DateTime ?_lastUpdate = DateTime.Now;
        public DateTime ?LastUpdate
        {
            get { return _lastUpdate; }
            set
            {
                _lastUpdate = value;
                this.OnPropertyChanged("LastUpdate");
                if (LastUpdate == null)
                {
                    LastUpdateString = "Очень давно";
                }
                else
                {
                    LastUpdateString = ((DateTime)LastUpdate).ToString("dd.MM.yyyy hh:mm");
                }
            }
        }

        private string _lastUpdateString="";
        public string LastUpdateString
        {
            get { return _lastUpdateString; }
            set
            {
                _lastUpdateString = value;
                this.OnPropertyChanged("LastUpdateString");
            }
        }

        private string _lastUpdaeError = "Очень давно";
        public string LastUpdateError
        {
            get { return _lastUpdaeError; }
            set
            {
                _lastUpdaeError = value;
                this.OnPropertyChanged("LastUpdateError");
            }
        }



        public class Sensor : BaseViewModel
        {
            public event EventHandler Updated;

            private DeviceSensorsModel _model;
            public DeviceSensorsModel Model
            {
                get { return _model; }
                set
                {
                    _model = value;
                    this.OnPropertyChanged("Model");
                }
            }

            private SensorState _state = null;

            public SensorState State
            {
                get { return _state; }
                set
                {
                    _state = value;
                    this.OnPropertyChanged("State");
                    if (Updated != null)
                        Updated(this, new EventArgs());
                }
            }
        }
    }
}
