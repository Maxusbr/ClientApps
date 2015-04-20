using DTCDev.Client.Cars.Controls.Models;
using DTCDev.Client.Cars.Engine.AppLogic;
using DTCDev.Client.Cars.Engine.Handlers.Cars;
using DTCDev.Client.ViewModel;
using DTCDev.Models.CarsSending.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Client.Cars.Controls.ViewModels.Reports
{
    public class FuelReportViewModel : ViewModelBase
    {
        public FuelReportViewModel()
        {
            ReportsHandler.Instance.FuelReportLoaded += Instance_FuelReportLoaded;
        }

        public event EventHandler FuelLoaded;

        void Instance_FuelReportLoaded(object sender, EventArgs e)
        {
            Result = ReportsHandler.Instance.Fuel;
            if (FuelLoaded != null)
                FuelLoaded(this, new EventArgs());
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

        private ReportFuelModel _result;
        public ReportFuelModel Result
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
            ReportsHandler.Instance.GetFuelReport(CarSelector.SelectedCar.Car.Id, DateStart, DateStop);
        }


        private RelayCommand _exportDataCommand;
        public RelayCommand ExportDataCommand
        {
            get { return _exportDataCommand ?? (_exportDataCommand = new RelayCommand(a => ExportData())); }
        }



        private void ExportData()
        {
            var s = new ExportToExcel<ReportFuelModel.ItemModel>
            {
                DataToPrint = Result.Report
            };
            s.GenerateReport(GetHeader());
        }

        private object[] GetHeader()
        {
            var res = new List<object> { "Дата", "Уровень топлива"};
            return res.ToArray();
        }
    }
}
