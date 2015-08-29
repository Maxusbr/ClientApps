using DTCDev.Client.Cars.Engine.AppLogic;
using DTCDev.Client.Cars.Engine.Handlers.Cars;
using DTCDev.Client.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using DTCDev.Client.Cars.Controls.Models;
using DTCDev.Client.Cars.Engine.DisplayModels;
using DTCDev.Models.CarsSending.Car;

namespace DTCDev.Client.Cars.Controls.ViewModels.Reports
{
    public class CompilateReportViewModel : ViewModelBase
    {
        #region Fields

        private DateTime _dateStart = DateTime.Now - new TimeSpan(7, 0, 0, 0);
        private DateTime _dateStop = DateTime.Now;
        private readonly HistoryHandler _handler;
        private RelayCommand _getDataCommand;
        private bool _vaiting = false;
        private string _carNumber = "";
        private readonly List<HistoryCarStateViewModel> _historyRows = new List<HistoryCarStateViewModel>();
        private readonly List<OBDHistoryDataModel> _loadedObdData = new List<OBDHistoryDataModel>();
        private readonly Dispatcher _dispatcher;

        #endregion

        public CompilateReportViewModel(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
            _handler = HistoryHandler.Instance;
            _handler.DayRefreshed += Instance_DayRefreshed;
            //_handler.OBDLoaded += Instance_OBDLoaded;
            CarSelector.OnCarChanged += CarSelector_OnCarChanged;
            if (CarSelector.SelectedCar != null) CarNumber = CarSelector.SelectedCar.Car.CarNumber;
            if (!DesignerProperties.GetIsInDesignMode(new DependencyObject())) return;
            //HistoryRows.Add(new HistoryCarStateViewModel {Date = DateTime.Now});
        }

        #region Events

        public delegate void AddHistoryRowEvent(HistoryCarStateViewModel item);
        public event AddHistoryRowEvent AddHistoryRow;
        protected virtual void OnAddHistoryRow(HistoryCarStateViewModel item)
        {
            if (AddHistoryRow != null) AddHistoryRow(item);
        }


        public event EventHandler ClearHistory;
        protected virtual void OnClearHistory()
        {
            if (ClearHistory != null) ClearHistory.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Handlers
        private void CarSelector_OnCarChanged(DISP_Car car)
        {
            CarNumber = car != null ? car.Car.CarNumber: "";
            OnPropertyChanged("EnableButton");
            OnClearHistory();
            HistoryRows.Clear();
            _loadedObdData.Clear();
        }


        private void Instance_OBDLoaded(OBDHistoryDataModel model)
        {
            //if (!_loadedObdData.Any(o => o.DT.Equals(model.DT)))
            _loadedObdData.Clear();
                _loadedObdData.Add(model);
        }

        /// <summary>
        /// Формирование массива данных для дня day
        /// </summary>
        /// <param name="data"></param>
        /// <param name="day"></param>
        private void Instance_DayRefreshed(DateTime day, List<CarStateModel> data)
        {
            var curentdata = data ?? new List<CarStateModel>();
            var first = curentdata.FirstOrDefault();
            if(first == null || !first.DevID.Equals(CarSelector.SelectedCar.Car.Id) || !Vaiting || day < DateStart || day > DateStop) return;
            var el = HistoryRows.FirstOrDefault(o => o.Date.Equals(day));
            if (el == null)
                DispatherThreadRun(() =>
                {
                    var item = new HistoryCarStateViewModel(curentdata, day, _dispatcher);
                    OnAddHistoryRow(item);
                    HistoryRows.Add(item);
                });
            else
                el.SortDataByDate(curentdata, day);
            //if(!_loadedObdData.Any(o => o.DT.ToDate.Equals(day)))
                
        }

        #endregion


        #region Properties

        public DateTime DateStart
        {
            get { return _dateStart; }
            set
            {
                _dateStart = value;
                this.OnPropertyChanged("DateStart");
            }
        }
        
        public bool Vaiting
        {
            get { return _vaiting; }
            set
            {
                _vaiting = value;
                this.OnPropertyChanged("Vaiting");
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


        public string CarNumber
        {
            get { return _carNumber; }
            set
            {
                _carNumber = value;
                this.OnPropertyChanged("CarNumber");
            }
        }

        public List<HistoryCarStateViewModel> HistoryRows
        {
            get { return _historyRows; }
        }

        #endregion


        #region RelayCommands

        public RelayCommand GetDataCommand
        {
            get { return _getDataCommand ?? (_getDataCommand = new RelayCommand(a => StartLoadData())); }
        }

        public bool EnableButton
        {
            get { return CarSelector.SelectedCar != null; }
        }

        #endregion


        #region Metods

        private void StartLoadData()
        {
            HistoryRows.Clear();
            Vaiting = true;
            if (CarSelector.SelectedCar == null)
                return;
            CarNumber = CarSelector.SelectedCar.Car.CarNumber;
            //ReportsHandler.Instance.GetCompilateReport(CarSelector.SelectedCar.Car.Id, DateStart, DateStop);
            _handler.StartLoadHistory(CarSelector.SelectedCar.Car.Id, DateStart, DateStop);
        }

        /// <summary>
        /// Выполнение метода action в потоке приложения
        /// </summary>
        /// <param name="action"></param>
        private void DispatherThreadRun(Action action)
        {
            if (_dispatcher != null)
                _dispatcher.BeginInvoke(action);

        }

        #endregion

        
    }

    
}
