using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCDev.Client.ViewModel;
using DTCDev.Models.CarBase.CarList;
using DTCDev.Client.Cars.Service.Engine.Storage;
using System.Windows;

namespace DTCDev.Client.Cars.Service.Engine.Controls.ViewModels
{
    public class CarViewModel : ViewModelBase
    {
        private string _carNumber;
        private string _mark;
        private string _model;
        private int _currentDistance;
        private int _incomeDistance;
        private string _serviceName;
        private int _daysToService;
        private Visibility _visErrors = Visibility.Collapsed;
        private int _errors = 0;

        public CarViewModel(CarListBaseDataModel model, int errors)
        {
            CarNumber = model.CarNumber;
            Mark = model.Mark;
            Model = model.Model;
            CurrentDistance = model.CurrentDistance;
            IncomeDistance = model.IncomeDistance;
            ServiceName = model.ServiceName;
            DaysToService = model.DaysToService;
            ErrorsCount = errors;
            if (errors > 0)
                VisErrors = Visibility.Visible;
            else
                VisErrors = Visibility.Collapsed;
        }

        public CarViewModel()
        {
        }

        public string CarNumber
        {
            get { return _carNumber; }
            set
            {
                if (_carNumber == value) return;
                _carNumber = value;
                OnPropertyChanged("CarNumber");
            }
        }

        public string Mark
        {
            get { return _mark; }
            set {
                if (_mark == value) return;
                _mark = value;
                OnPropertyChanged("Mark");
            }
        }

        public string Model
        {
            get { return _model; }
            set
            {
                if (_model == value) return;
                _model = value;
                OnPropertyChanged("Model");
            }
        }

        public int CurrentDistance
        {
            get { return _currentDistance; }
            set
            {
                if (_currentDistance == value) return;
                _currentDistance = value;
                OnPropertyChanged("CurrentDistance");
            }
        }

        public int IncomeDistance
        {
            get { return _incomeDistance; }
            set
            {
                if (_incomeDistance == value) return;
                _incomeDistance = value;
                OnPropertyChanged("IncomeDistance");
            }
        }

        public string ServiceName
        {
            get { return _serviceName; }
            set
            {
                if (_serviceName == value) return;
                _serviceName = value;
                OnPropertyChanged("ServiceName");
            }
        }

        public int DaysToService
        {
            get { return _daysToService; }
            set
            {
                if (_daysToService == value) return;
                _daysToService = value;
                OnPropertyChanged("DaysToService");
            }
        }

        public int ErrorsCount
        {
            get { return _errors; }
            set
            {
                _errors = value;
                this.OnPropertyChanged("ErrorsCount");
            }
        }

        public Visibility VisErrors
        {
            get { return _visErrors; }
            set
            {
                _visErrors = value;
                this.OnPropertyChanged("VisErrors");
            }
        }
    }
}
