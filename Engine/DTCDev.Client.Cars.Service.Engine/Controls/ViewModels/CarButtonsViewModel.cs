using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCDev.Client.ViewModel;

namespace DTCDev.Client.Cars.Service.Engine.Controls.ViewModels
{
    public class CarButtonsViewModel : ViewModelBase
    {
        private RelayCommand _addCarCommand;
        private RelayCommand _addWorkCommand;
        private RelayCommand _showHistoryCommand;
        private bool _isSelectedCar = true;

        public CarButtonsViewModel() { }

        public event EventHandler AddCarHandler;
        protected virtual void OnAddCarHandler()
        {
            if (AddCarHandler != null) AddCarHandler(this, EventArgs.Empty);
        }

        public event EventHandler AddWorkHandler;
        protected virtual void OnAddWorkHandler()
        {
            if (AddWorkHandler != null) AddWorkHandler(this, EventArgs.Empty);
        }

        public event EventHandler ShowHistoryHandler;
        protected virtual void OnShowHistoryHandler()
        {
            if (ShowHistoryHandler != null) ShowHistoryHandler(this, EventArgs.Empty);
        }

        public RelayCommand AddCarCommand
        {
            get { return _addCarCommand ?? (_addCarCommand = new RelayCommand(AddCar)); }
        }

        private void AddCar(object obj)
        {
            OnAddCarHandler();
        }

        public RelayCommand AddWorkCommand
        {
            get { return _addWorkCommand ?? (_addWorkCommand = new RelayCommand(AddWork)); }
        }

        private void AddWork(object obj)
        {
            OnAddWorkHandler();
        }

        public RelayCommand ShowHistoryCommand
        {
            get { return _showHistoryCommand ?? (_showHistoryCommand = new RelayCommand(ShowHistory)); }
        }

        private void ShowHistory(object obj)
        {
            OnShowHistoryHandler();
        }

        public bool IsSelectedCar
        {
            get { return _isSelectedCar; }
            set
            {
                if (_isSelectedCar == value) return;
                _isSelectedCar = value;
                OnPropertyChanged("IsSelectedCar");
            }
        }

    }
}
