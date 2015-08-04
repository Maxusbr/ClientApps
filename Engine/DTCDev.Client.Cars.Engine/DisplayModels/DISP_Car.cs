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
                OnPropertyChanged("Data");
                Update();
                if (value == null) return;

                if (value.Navigation.Latitude != 0 && value.Navigation.Longitude != 0)
                {
                    Navigation.LocationPoint = new Location(value.Navigation.Latitude / 10000.0d, value.Navigation.Longitude / 10000.0d);
                    Navigation.DateNavigation = Data.DateUpdate.ToString();
                    Navigation.DateLastUpdate = Data.DateUpdate.ToDateTime();
                    Navigation.Angle = Data.Navigation.Speed / 10 - 30;
                    Navigation.CountSatelite = Data.Navigation.Sattelites.ToString();
                }
                Navigation.Current_Speed = value.Navigation.Speed/10.0;
                FuelData.CalculateFuelData(value);
            }
        }

        private NavigationData _navigation = new NavigationData();
        
        /// <summary>
        /// Данные о навигации автомобиля
        /// </summary>
        public NavigationData Navigation
        {
            get { return _navigation; }
        }
        /// <summary>
        /// Обновить данные навигации для модели
        /// </summary>
        /// <param name="navigationData"></param>
        /// <param name="updateLocation"></param>
        public void UpdateNavigation(NavigationData navigationData)
        {
            Navigation.Update(navigationData);
            OnPropertyChanged("Navigation");
        }

        /// <summary>
        /// Показать Модель на карте
        /// </summary>
        /// <param name="visable"></param>
        public void UpdateVisableMap(bool visable)
        {
            HistoryDetailView = visable;
            Navigation.VisOnMap = visable ? Visibility.Visible : Visibility.Collapsed;
        }

        private DevicePresenter _device = new DevicePresenter();

        /// <summary>
        /// Класс обработки данных о линиях устройства
        /// </summary>
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

        /// <summary>
        /// Водитель автомобиля
        /// </summary>
        public DriverModel Driver
        {
            get { return _driver; }
            set { _driver = value; }
        }

        private ZoneModel _zoneData = new ZoneModel();

        /// <summary>
        /// Данные для работы с зонами автомобиля
        /// </summary>
        public ZoneModel ZoneData
        {
            get { return _zoneData; }
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

        public void UpdateZone(ZoneModel model)
        {
            ZoneData.Update(model);
            OnPropertyChanged("ZoneData");
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

        private string _mark = "";
        public string Mark
        {
            get { return _mark; }
            set
            {
                _mark = value;
                this.OnPropertyChanged("Mark");
            }
        }

        private string _model = "";
        public string Model
        {
            get { return _model; }
            set
            {
                _model = value;
                this.OnPropertyChanged("Model");
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
            //OnPropertyChanged("DateNavigation");
            //OnPropertyChanged("CountSatelite");
            //OnPropertyChanged("Angle");
            //OnPropertyChanged("FuelLevel");
            //OnPropertyChanged("HistroryTime");
        }



        private FuelDataModel _fuelData = new FuelDataModel();

        /// <summary>
        /// Данные о топливе в автомобиле
        /// </summary>
        public FuelDataModel FuelData
        {
            get { return _fuelData; }
            set
            {
                _fuelData = value;
            }
        }




        public class EOBDData
        {
            public string Key { get; set; }

            public string Value { get; set; }
        }

        
    }
}
