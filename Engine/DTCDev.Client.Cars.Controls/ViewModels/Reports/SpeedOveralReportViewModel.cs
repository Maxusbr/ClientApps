using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DTCDev.Client.Cars.Controls.Models;
using DTCDev.Client.Cars.Engine.AppLogic;
using DTCDev.Client.Cars.Engine.Handlers.Cars;
using DTCDev.Client.ViewModel;
using DTCDev.Models.CarsSending.Report;
using RelayCommand = DTCDev.Client.ViewModel.RelayCommand;

namespace DTCDev.Client.Cars.Controls.ViewModels.Reports
{
    class SpeedOveralReportViewModel : ViewModelBase
    {
        public SpeedOveralReportViewModel()
        {
            CarSelector.OnCarChanged += CarSelector_OnCarChanged;
            ReportsHandler.Instance.SpeedOveralLoaded += Instance_MialageReportLoaded;
            ReportsHandler.Instance.SpeedOveralDataPreLoaded += Instance_SpeedOveralDataPreLoaded;
            StartLoadData();
        }

        void Instance_SpeedOveralDataPreLoaded(object sender, EventArgs e)
        {
            WaitText = "Обработка адресов";
        }

        void Instance_MialageReportLoaded(object sender, EventArgs e)
        {
            Result = ReportsHandler.Instance.SpeedOveral;
            VisWait = Visibility.Collapsed;
            var slowTask = new Task(delegate
            {
                foreach (var p in Result.Data.SelectMany(item => item.Points))
                {
                    p.StartAdress =
                        Helpers.GeoAdress.Instance.GetAdress(new Client.Controls.Map.Location
                        {
                            Latitude = p.LatitudeStart/10000.0,
                            Longitude = p.LongitudeStart/10000.0
                        });
                }
            });
            slowTask.Start();
        }



        void CarSelector_OnCarChanged(Engine.DisplayModels.DISP_Car car)
        {
            StartLoadData();
        }

        private DateTime _dateStart = DateTime.Now - new TimeSpan(7, 0, 0, 0);
        private DateTime _dateStop = DateTime.Now;

        private int _maxSpeed = 90;
        public int MaxSpeed
        {
            get { return _maxSpeed; }
            set
            {
                _maxSpeed = value;
                this.OnPropertyChanged("MaxSpeed");
            }
        }

        public DateTime DateStart
        {
            get { return _dateStart; }
            set
            {
                _dateStart = value;
                this.OnPropertyChanged("DateStart");
            }
        }

        public DateTime DateStop
        {
            get { return _dateStop; }
            set
            {
                _dateStop = value;
                this.OnPropertyChanged("DateStop");
            }
        }

        private int _totalDistance = 0;
        public int TotalDistance
        {
            get { return _totalDistance; }
            set
            {
                _totalDistance = value;
                this.OnPropertyChanged("TotalDistance");
            }
        }

        private string _carNumber = "";
        public string CarNumber
        {
            get { return _carNumber; }
            set
            {
                _carNumber = value;
                this.OnPropertyChanged("CarNumber");
            }
        }

        private Visibility _visViewHistory = Visibility.Collapsed;
        public Visibility VisViewHistory
        {
            get { return _visViewHistory; }
            set
            {
                _visViewHistory = value;
                this.OnPropertyChanged("VisViewHistory");
            }
        }


        private SpeedOveralReportModel _result;
        public SpeedOveralReportModel Result
        {
            get { return _result; }
            set
            {
                _result = value;
                this.OnPropertyChanged("Result");
            }
        }

        private string _WaitText = "Загрузка";
        public string WaitText
        {
            get { return _WaitText; }
            set
            {
                _WaitText = value;
                this.OnPropertyChanged("WaitText");
            }
        }

        private Visibility _visWait = Visibility.Collapsed;
        public Visibility VisWait
        {
            get { return _visWait; }
            set
            {
                _visWait = value;
                this.OnPropertyChanged("VisWait");
            }
        }




        private RelayCommand _getDataCommand;
        public RelayCommand GetDataCommand
        {
            get
            {
                if (_getDataCommand == null)
                    _getDataCommand = new RelayCommand(a => StartLoadData());
                return _getDataCommand;
            }
        }



        private void StartLoadData()
        {
            if (CarSelector.SelectedCar == null)
                return;
            CarNumber = CarSelector.SelectedCar.Car.CarNumber;
            ReportsHandler.Instance.GetSpeedOveralReport(CarSelector.SelectedCar.Car.Id, DateStart, DateStop, MaxSpeed);

            VisWait = Visibility.Visible;
            WaitText = "Загрузка";
        }

        private RelayCommand _exportDataCommand;
        public RelayCommand ExportDataCommand
        {
            get { return _exportDataCommand ?? (_exportDataCommand = new RelayCommand(a => ExportData())); }
        }

        private void ExportData()
        {
            var report = (from item in Result.Data from el in item.Points 
                          select new SpeedOveralReportConvertModel(item.Date,el)).ToList();
            var s = new ExportToExcel<SpeedOveralReportConvertModel>
            {
                DataToPrint = report
            };
            s.GenerateReport(GetHeader());
        }

        private object[] GetHeader()
        {
            var res = new List<object> { "Дата", "Скорость (км/ч)", "Время", "Расстояние (км)", "Адрес" };
            return res.ToArray();
        }

        private class SpeedOveralReportConvertModel
        {
            public SpeedOveralReportConvertModel(
                DTCDev.Models.Date.DateDataModel date, 
                SpeedOveralReportModel.DayReport.SpeedPoint model)
            {
                Date = date;
                Speed = model.Speed;
                Time = TimeSpan.FromMinutes(model.TimeMove);
                Distance = model.DistanceMove / 1000.0;
                Adress = model.StartAdress;
            }

            public DTCDev.Models.Date.DateDataModel Date{ get; set; }
            public int Speed { get; set; }
            public TimeSpan Time { get; set; }
            public double Distance { get; set; }
            public string Adress { get; set; }
        }
    }
}
