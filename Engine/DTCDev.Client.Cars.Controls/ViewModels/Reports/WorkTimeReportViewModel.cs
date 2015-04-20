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
    class WorkTimeReportViewModel : ViewModelBase
    {
        public WorkTimeReportViewModel()
        {
            if (TimeFrames == null)
                TimeFrames = new ObservableCollection<string>();
            for (int i = 0; i < 48; i++)
            {
                TimeSpan ts = TimeSpan.FromMinutes(30 * i);
                TimeFrames.Add(ts.Hours.ToString() + ":" + ts.Minutes.ToString());
            }
            SelectedIndexStart = 15;
            SelectedIndexStop = 31;
            CarSelector.OnCarChanged += CarSelector_OnCarChanged;
            StartLoadData();
        }

        public ObservableCollection<WorkTimeReportModel> Data
        {
            get { return ReportsHandler.Instance.WorktimeReport; }
        }

        public ObservableCollection<string> TimeFrames { get; set; }

        private int _selectedIndexStart = -1;
        private int _selectedIndexStop = -1;

        public int SelectedIndexStart
        {
            get { return _selectedIndexStart; }
            set
            {
                _selectedIndexStart = value;
                this.OnPropertyChanged("SelectedIndexStart");
            }
        }

        public int SelectedIndexStop
        {
            get { return _selectedIndexStop; }
            set
            {
                _selectedIndexStop = value;
                this.OnPropertyChanged("SelectedIndexStop");
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
            TimeSpan tsStartWork = TimeSpan.FromMinutes(30 * SelectedIndexStart);
            TimeSpan tsStopWork = TimeSpan.FromMinutes(30 * SelectedIndexStop);
            ReportsHandler.Instance.GetWorkTimeReport(CarSelector.SelectedCar.Car.Id, DateStart, DateStop, tsStartWork, tsStopWork);
        }

        private RelayCommand _exportDataCommand;
        public RelayCommand ExportDataCommand
        {
            get { return _exportDataCommand ?? (_exportDataCommand = new RelayCommand(a => ExportData())); }
        }

        private void ExportData()
        {
            var s = new ExportToExcel<WorkTimeReportConvertModel>
            {
                DataToPrint = Data.Select(o => new WorkTimeReportConvertModel(o)).ToList()
            };
            s.GenerateReport(GetHeader());
        }

        private object[] GetHeader()
        {
            var res = new List<object> { "Дата", "Пробег в раб.", "Время в раб.", "Пробег в нераб.", "Время в нераб." };
            return res.ToArray();
        }

        public class WorkTimeReportConvertModel
        {
            public WorkTimeReportConvertModel(WorkTimeReportModel model)
            {
                Date = model.Date;
                UsedInworkTime = TimeSpan.FromMinutes(model.UsedInworkTime);
                UsedInUnworkTime = TimeSpan.FromMinutes(model.UsedInUnworkTime);
                DistWorkTime = model.DistWorkTime;
                DistUnworkTime = model.DistUnworkTime;
            }

            public DateDataModel Date { get; set; }
            public int DistWorkTime { get; set; }
            public TimeSpan UsedInworkTime { get; set; }
            public int DistUnworkTime { get; set; }
            public TimeSpan UsedInUnworkTime { get; set; }
        }
    }
}
