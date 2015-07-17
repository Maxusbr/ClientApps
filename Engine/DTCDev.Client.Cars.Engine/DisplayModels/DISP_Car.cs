using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using DTCDev.Client.Cars.Engine.AppLogic;
using DTCDev.Client.Controls.Map;
using DTCDev.Client.ViewModel;
using DTCDev.Models.CarsSending;
using DTCDev.Models.CarsSending.Car;
using DTCDev.Models.CarsSending.Navigation;
using DTCDev.Models.DeviceSender;
using DTCDev.Models.DeviceSender.DISP;
using DTCDev.Models.CarBase.CarStatData;

namespace DTCDev.Client.Cars.Engine.DisplayModels
{
    public class DISP_Car : ViewModelBase
    {
        private SCarModel _car = new SCarModel();

        public SCarModel Car
        {
            get { return _car; }
            set
            {
                _car = value;
                if (value == null)
                    return;
                this.ID = value.Id;
                this.Name = value.CarNumber;
                IsCreated = true;
            }
        }
        public bool IsCreated = false;

        private SCarData _data = new SCarData();

        public SCarData Data
        {
            get { return _data; }
            set
            {
                _data = value;
                this.OnPropertyChanged("Data");
                Update();
                if (value == null)
                    return;
                if(value.Navigation.Latitude != 0 && value.Navigation.Longitude != 0 )
                    Location = new Location(value.Navigation.Latitude / 10000.0d, value.Navigation.Longitude / 10000.0d);
                Current_Speed = value.Navigation.Speed / 10.0;
                CalculateFuelData();
            }
        }

        private DevicePresenter _device = new DevicePresenter();

        public DevicePresenter Device
        {
            get { return _device; }
            set
            {
                _device = value;
                this.OnPropertyChanged("Device");
            }
        }

        private DriverModel _driver;
        public DriverModel Driver
        {
            get { return _driver; }
            set { _driver = value; }
        }

        bool _isChanged = false;
        public bool IsChanged
        {
            get { return _isChanged; }
            set
            {
                if (this._isChanged != value)
                {
                    this._isChanged = value;
                    OnPropertyChanged("IsChanged");
                }
            }
        }

        private int _zoneId = -1;
        /// <summary>
        /// принадлежность автомобиля к зоне
        /// </summary>
        public int ZoneId
        {
            get { return _zoneId; }
            set
            {
                if (_zoneId != value)
                {
                    _zoneId = value;
                    OnPropertyChanged("ZoneId");
                }
            }
        }

        private string id;
        /// <summary>
        /// Идентификатор автомобиля
        /// </summary>
        public string ID
        {
            get { return id; }
            set
            {
                if (id != value)
                {
                    id = value;
                    OnPropertyChanged("ID");
                }
            }
        }

        private string name;
        /// <summary>
        /// Название автомобиля, в данном случае - госномер
        /// </summary>
        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        private Location location;
        /// <summary>
        /// Текущее местоположение автомобиля
        /// </summary>
        public Location Location
        {
            get { return location; }
            set
            {
                if (location == value) return;
                location = value;
                OnPropertyChanged("Location");
                if (location.Latitude > 0 && location.Longitude > 0 && VisOnMap == Visibility.Collapsed)
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
                return Location != null ? string.Format("{0}:{1}", Math.Ceiling(Location.Latitude), Math.Ceiling(Location.Longitude)) : "0:0";
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

        private bool inzone = true;
        /// <summary>
        /// Находится ли автомобиль в зоне
        /// </summary>
        public bool InZone
        {
            get { return inzone; }
            set
            {
                inzone = value;
                OnPropertyChanged("InZone");
            }
        }

        private string adress = string.Empty;
        /// <summary>
        /// Адрес автомобиля
        /// </summary>
        public string Adress
        {
            get { return adress; }
            set
            {
                if (adress != value)
                {
                    adress = value;
                    OnPropertyChanged("Adress");
                }
            }
        }

        private bool _isSelected = false;
        /// <summary>
        /// Указатель выбран ли автомобиль
        /// </summary>
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected == value) return;
                _isSelected = value;
                OnPropertyChanged("IsSelected");
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

        /// <summary>
        /// Дата последнего обновления навигационных данных автомобиля
        /// </summary>
        public string DateNavigation
        {
            get
            {
                return Data != null ? Data.DateUpdate.ToString() : "";
            }
        }

        public DateTime DateLastUpdate
        {
            get { return Data != null ? Data.DateUpdate.ToDateTime() : new DateTime(); }
        }

        public string HistroryTime
        {
            get
            {
                return string.Format("{0:00}:{1:00}:{2:00}", DateLastUpdate.Hour, DateLastUpdate.Minute, DateLastUpdate.Second);
            }
        }

        /// <summary>
        /// Текущий угол поворота автомобиля
        /// </summary>
        public double Angle
        {
            get { return Data != null ? Data.Navigation.Speed / 10 - 30 : -30; }
        }

        /// <summary>
        /// Количество спутников
        /// </summary>
        public string CountSatelite
        {
            get { return Data != null ? Data.Navigation.Sattelites.ToString() : ""; }
        }

        private string _vin = "";

        public string VIN
        {
            get { return _vin; }
            set
            {
                _vin = value;
                this.OnPropertyChanged("VIN");
            }
        }

        private bool _historyDetailView;
        public bool HistoryDetailView
        {
            get { return _historyDetailView; }
            set
            {
                _historyDetailView = value;
                OnPropertyChanged("HistoryDetailView");
            }
        }

        private List<EOBDData> _errors = new List<EOBDData>();

        public List<EOBDData> Errors
        {
            get { return _errors; }
            set { _errors = value; }
        }

        private List<EOBDData> _obd = new List<EOBDData>();
        public List<EOBDData> OBD
        {
            get { return _obd; }
            set
            {
                _obd = value;
            }
        }


        private readonly IndicatorPercentModel _levelGsmInd = new IndicatorPercentModel(100);
        public IndicatorPercentModel LevelGsmInd { get { return _levelGsmInd; } }

        private readonly IndicatorPercentModel _sateliteInd = new IndicatorPercentModel(15);
        public IndicatorPercentModel SateliteInd { get { return _sateliteInd; } }

        private RelayCommand _selectCommand;
        public RelayCommand SelectCommand
        {
            get
            {
                if (_selectCommand == null)
                    _selectCommand = new RelayCommand(a => CarSelected());
                return _selectCommand;
            }
        }


        private void CarSelected()
        {
            CarSelector.ViewCarDetailsCar = this;
        }

        public void Update()
        {
            LevelGsmInd.Update(Data != null ? Data.GSM_Level : 0);
            SateliteInd.Update(Data != null && Data.Navigation != null ? Data.Navigation.Sattelites : 0);
            OnPropertyChanged("DateNavigation");
            OnPropertyChanged("CountSatelite");
            OnPropertyChanged("Angle");
            OnPropertyChanged("FuelLevel");
            OnPropertyChanged("HistroryTime");
        }



        #region FuelData

        private void CalculateFuelData()
        {
            if (_fuelDataPosition < 0)
                return;
            if (this.Data.Sensors.Count() - 1 < _fuelDataPosition)
                return;
            if (_stepPerLiter < 0.0001m)
                return;
            int vol = this.Data.Sensors[_fuelDataPosition];
            vol = vol - _startValue;
            decimal fuel = ((decimal)vol) / _stepPerLiter;
            FuelLevelValue = (int)fuel;
        }

        /// <summary>
        /// уровень топлива в автомобиле
        /// </summary>
        public string FuelLevel
        {
            get { return Data != null ? Data.FuelLevel.ToString("D") : ""; }
        }

        private int _fuelLevelValue = 0;
        public int FuelLevelValue
        {
            get { return _fuelLevelValue; }
            set
            {
                _fuelLevelValue = value;
                this.OnPropertyChanged("FuelLevelValue");
            }
        }

        private int _fuelLevelPercents = 0;
        public int FuelLevelPercents
        {
            get { return _fuelLevelPercents; }
            set
            {
                _fuelLevelPercents = value;
                this.OnPropertyChanged("FuelLevelPercents");
            }
        }

        private int _fuelDataPosition = -1;
        public int FuelDataPosition
        {
            get { return _fuelDataPosition; }
            set
            {
                _fuelDataPosition = value;
                this.OnPropertyChanged("FuelDataPosition");
            }
        }

        private int _startValue = 0;
        public int StartFuelValue
        {
            get { return _startValue; }
            set
            {
                _startValue = value;
                this.OnPropertyChanged("StartFuelValue");
            }
        }

        private decimal _stepPerLiter = 0;
        public decimal StepPerLiter
        {
            get { return _stepPerLiter; }
            set
            {
                _stepPerLiter = value;
                this.OnPropertyChanged("StepPerLiter");
            }
        }

        #endregion




        public class EOBDData
        {
            public string Key { get; set; }

            public string Value { get; set; }
        }
    }
}
