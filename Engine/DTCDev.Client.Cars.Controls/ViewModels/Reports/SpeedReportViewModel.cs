using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using DTCDev.Client.Cars.Controls.Models;
using DTCDev.Client.Cars.Engine.AppLogic;
using DTCDev.Client.Cars.Engine.Handlers.Cars;
using DTCDev.Client.ViewModel;
using DTCDev.Models.CarsSending.Report;
using DTCDev.Models.Date;
using RelayCommand = DTCDev.Client.ViewModel.RelayCommand;

namespace DTCDev.Client.Cars.Controls.ViewModels.Reports
{
    public class SpeedReportViewModel : ViewModelBase
    {
        public SpeedReportViewModel()
        {
            CarSelector.OnCarChanged += CarSelector_OnCarChanged;
            ReportsHandler.Instance.SpeedReportLoaded += Instance_MialageReportLoaded;
            StartLoadData();
        }

        private ObservableCollection<DISP_SpeedReport> _data = new ObservableCollection<DISP_SpeedReport>();
        public ObservableCollection<DISP_SpeedReport> Data
        {
            get { return _data; }
            set
            {
                _data = value;
            }
        }

        void Instance_MialageReportLoaded(object sender, EventArgs e)
        {
            CalculateData();
        }

        void CarSelector_OnCarChanged(Engine.DisplayModels.DISP_Car car)
        {
            StartLoadData();
        }

        private DateTime _dateStart = DateTime.Now - new TimeSpan(7, 0, 0, 0);
        private DateTime _dateStop = DateTime.Now;

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
            ReportsHandler.Instance.GetSpeedReport(CarSelector.SelectedCar.Car.Id, DateStart, DateStop);
        }

        private void CalculateData()
        {
            Data.Clear();
            foreach (var item in ReportsHandler.Instance.SpeedReport.Data)
            {
                DISP_SpeedReport model = new DISP_SpeedReport();
                model.Date = item.Date;
                int maxSpeed = 0;
                int centerSpeed = 0;
                TimeSpan tsOveral = new TimeSpan(0, 0, 0);
                TimeSpan tsStart = new TimeSpan();
                for (int i = 0; i < item.Points.Count(); i++)
                {
                    if (item.Points[i].Speed > maxSpeed)
                        maxSpeed = item.Points[i].Speed;
                    centerSpeed = (centerSpeed + item.Points[i].Speed) / 2;
                    if (item.Points[i].Speed > 900)
                    {
                        tsStart = new TimeSpan(item.Points[i].Time.H, item.Points[i].Time.M, item.Points[i].Time.S);
                    }
                    else
                    {
                        if (tsStart.TotalMinutes>0)
                        {
                            TimeSpan tsDist = new TimeSpan(item.Points[i].Time.H, item.Points[i].Time.M, item.Points[i].Time.S) - tsStart;
                            tsOveral+=tsDist;
                            tsStart = new TimeSpan();
                        }
                    }
                }
                model.TimeOveral = (int)tsOveral.TotalMinutes;
                model.MaxSpeed = maxSpeed / 10;
                model.CenterSpeed = centerSpeed / 10;
                Data.Add(model);
            }
        }

        public class DISP_SpeedReport
        {
            public DateDataModel Date { get; set; }

            public int MaxSpeed { get; set; }

            public int CenterSpeed { get; set; }

            public int TimeOveral { get; set; }
        }

        private RelayCommand _exportDataCommand;
        public RelayCommand ExportDataCommand
        {
            get { return _exportDataCommand ?? (_exportDataCommand = new RelayCommand(a => ExportData())); }
        }

        private void ExportData()
        {
            var s = new ExportToExcel<DISP_SpeedReport>
            {
                DataToPrint = Data.ToList()
            };
            s.GenerateReport(GetHeader());
        }

        private object[] GetHeader()
        {
            var res = new List<object> { "Дата", "Max скорость", "Средняя скорость", "Более 90 км/ч, минут" };
            return res.ToArray();
        }
    }
}
