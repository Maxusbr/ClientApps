using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using DTCDev.Client.Cars.Controls.Models;
using DTCDev.Client.Cars.Engine.AppLogic;
using DTCDev.Client.Cars.Engine.Handlers.Cars;
using DTCDev.Client.ViewModel;
using DTCDev.Models.CarsSending.Report;
using RelayCommand = DTCDev.Client.ViewModel.RelayCommand;

namespace DTCDev.Client.Cars.Controls.ViewModels.Reports
{
    class DistanceReportViewModel : ViewModelBase
    {
        public DistanceReportViewModel()
        {
            CarSelector.OnCarChanged += CarSelector_OnCarChanged;
            ReportsHandler.Instance.MialageReportLoaded += Instance_MialageReportLoaded;
            StartLoadData();
        }

        void Instance_MialageReportLoaded(object sender, EventArgs e)
        {
            Result = ReportsHandler.Instance.MialageReport;
            TotalDistance = 0;
            foreach (var item in Result.Data)
            {
                TotalDistance += item.Dist;
            }
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


        private MialageReportModel _result;
        public MialageReportModel Result
        {
            get { return _result; }
            set
            {
                _result = value;
                this.OnPropertyChanged("Result");
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
            ReportsHandler.Instance.GetDistanceReport(CarSelector.SelectedCar.Car.Id, DateStart, DateStop);
        }


        private RelayCommand _exportDataCommand;
        public RelayCommand ExportDataCommand
        {
            get { return _exportDataCommand ?? (_exportDataCommand = new RelayCommand(a => ExportData())); }
        }



        private void ExportData()
        {
            var s = new ExportToExcel<MialageReportModel.DR_Point>
            {
                DataToPrint = Result.Data
            };
            s.GenerateReport(GetHeader());
        }

        private object[] GetHeader()
        {
            var res = new List<object> { "Дата", "Пробег", "Max скорость", "Начало", "Окончание" };
            return res.ToArray();
        }

    }
}
