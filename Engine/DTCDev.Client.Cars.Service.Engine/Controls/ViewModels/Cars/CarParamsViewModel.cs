using DTCDev.Client.Cars.Service.Engine.Handlers;
using DTCDev.Client.Cars.Service.Engine.Storage;
using DTCDev.Client.ViewModel;
using DTCDev.Models.CarBase.CarStatData;
using DTCDev.Models.CarsSending.Car;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace DTCDev.Client.Cars.Service.Engine.Controls.ViewModels.Cars
{
    public class CarParamsViewModel : ViewModelBase
    {

        public CarParamsViewModel()
        {
            _currentCar = CarStorage.Instance.SelectedCar;
            CarStorage.Instance.OBDHistoryLoaded += Instance_OBDHistoryLoaded;
            CallLoadHistory();
            CallLoadPids();
            CarsHandler.Instance.PIDSLoaded += Instance_PIDSLoaded;
        }

        void Instance_OBDHistoryLoaded(object sender, EventArgs e)
        {
            History = _currentCar.OBDHistory;
        }

        private DateTime _displayedDate = DateTime.Now;
        private OBDHistoryDataModel _history;
        private DISP_Car _currentCar;
        private ObservableCollection<CarEnabledPIDs> _pids = new ObservableCollection<CarEnabledPIDs>();
        public ObservableCollection<CarEnabledPIDs> PIDS
        {
            get { return _pids; }
        }

        public OBDHistoryDataModel History
        {
            get { return _history; }
            set
            {
                _history = value;
                this.OnPropertyChanged("History");
            }
        }

        public DateTime DisplayedDate
        {
            get { return _displayedDate; }
            set
            {
                _displayedDate = value;
                this.OnPropertyChanged("DisplayedDate");
                CallLoadHistory();
            }
        }


        private void CallLoadHistory()
        {
            if (_currentCar == null)
                return;
            if (_currentCar.CalledOBDHistory != null)
                if (_currentCar.CalledOBDHistory == _displayedDate)
                {
                    History = _currentCar.OBDHistory;
                    return;
                }
            CarsHandler.Instance.GetOBDHistory(_currentCar.CarModel.DID, _displayedDate.Year, _displayedDate.Month, _displayedDate.Day);
            _currentCar.CalledOBDHistory = _displayedDate;
        }



        #region PIDS

        private void CallLoadPids()
        {
            PIDS.Clear();
            if (_currentCar == null)
                return;
            CarsHandler.Instance.GetCarPidList(_currentCar.CarModel.CarNumber);
        }

        void Instance_PIDSLoaded(List<CarEnabledPIDs> pids)
        {
            List<CarEnabledPIDs> temp = pids.OrderByDescending(p => p.Enabled).ToList();
            foreach (var item in temp)
            {
                PIDS.Add(item);
            }
        }

        private RelayCommand _savePidsCommand;
        public RelayCommand SavePidsCommand
        {
            get
            {
                if (_savePidsCommand == null)
                    _savePidsCommand = new RelayCommand(SaveNewPids);
                return _savePidsCommand;
            }
        }

        private void SaveNewPids(object obj)
        {
            SetCarPidsModel model = new SetCarPidsModel();
            model.CarNumber = _currentCar.CarModel.CarNumber;
            List<CarEnabledPIDs> temp = PIDS.Where(p => p.PIDEnabled == true).ToList();
            for (int i = 0; i < temp.Count(); i++)
            {
                model.PIDS.Add(temp[i].PID);
            }
            CarsHandler.Instance.SendCarNewPids(model);
        }

        #endregion
    }
}
