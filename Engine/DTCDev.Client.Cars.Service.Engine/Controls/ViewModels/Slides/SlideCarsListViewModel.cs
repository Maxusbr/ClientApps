﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using DTCDev.Client.Cars.Service.Engine.Handlers;
using DTCDev.Client.ViewModel;
using DTCDev.Models.CarBase.CarList;
using DTCDev.Models.CarBase.CarStatData;
using DTCDev.Client.Cars.Service.Engine.Storage;

namespace DTCDev.Client.Cars.Service.Engine.Controls.ViewModels.Slides
{
    public class SlideCarsListViewModel : ViewModelBase
    {
        public SlideCarsListViewModel()
        {
            CarStorage.Instance.LoadComplete += Instance_LoadComplete;
            UpdateCar();
        }

        void Instance_LoadComplete(object sender, EventArgs e)
        {
            UpdateCar();
            SettingsLoader.Instance.Save();
        }

        public event EventHandler OnSelectedCarChange;
        private void SelectedCarChange(DISP_Car car)
        {
            CarStorage.Instance.GetCarDetails(car.CarModel.CarNumber);
            if (OnSelectedCarChange != null) OnSelectedCarChange(car, EventArgs.Empty);
        }

        public event EventHandler OnClose;
        private void CloseHandler()
        {
            if (OnClose != null) OnClose(this, EventArgs.Empty);
        }

        private bool _visableCloseCommand = false;
        private DISP_Car _selectedCar;
        private string _searchText;

        private bool _loaded = true;
        public bool Loading
        {
            get { return _loaded; }
            set
            {
                _loaded = value;
                this.OnPropertyChanged("Loading");
            }
        }

        public bool VisableCloseCommand
        {
            get { return _visableCloseCommand; }
            protected set
            {
                if (_visableCloseCommand == value) return;
                _visableCloseCommand = value;
                OnPropertyChanged("VisableCloseCommand");
            }
        }


        private ObservableCollection<DISP_Car> _cars = new ObservableCollection<DISP_Car>();
        public ObservableCollection<DISP_Car> Cars
        {
            get { return _cars; }
        }

        public DISP_Car SelectedCar
        {
            get { return _selectedCar; }
            set
            {
                if (_selectedCar == value) return;
                _selectedCar = value;
                OnPropertyChanged("SelectedCar");
                if (value != null)
                    SelectedCarChange(value);
            }
        }

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                if (_searchText == value) return;
                _searchText = value;
                OnPropertyChanged("SearchText");
                FilterCars();
            }
        }

        void UpdateCar()
        {
            Cars.Clear();
            foreach (var item in CarStorage.Instance.Cars)
            {
                Cars.Add(item);
            }
            if (Cars.Count() > 0)
                Loading = false;
            else
                Loading = true;
        }

        public void FilterCars()
        {
            if (string.IsNullOrEmpty(SearchText))
                UpdateCar();
            else
            {
                Cars.Clear();
                foreach (var item in CarStorage.Instance.Cars)
                {
                    if (item.CarModel.CarNumber.IndexOf(SearchText) != -1)
                        Cars.Add(item);
                }
            }
        }


        private RelayCommand _closeCommand;
        public RelayCommand CloseCommand
        {
            get { return _closeCommand ?? (_closeCommand = new RelayCommand(Close)); }
        }

        private void Close(object parameter)
        {
            CloseHandler();
        }
    }
}