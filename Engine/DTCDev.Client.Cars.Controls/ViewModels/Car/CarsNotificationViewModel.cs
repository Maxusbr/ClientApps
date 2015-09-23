using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Data;
using DTCDev.Client.Cars.Controls.ViewModels.History;
using DTCDev.Client.Cars.Engine.AppLogic;
using DTCDev.Client.Cars.Engine.DisplayModels;
using DTCDev.Client.ViewModel;
using DTCDev.Models.DeviceSender.DISP;
using Microsoft.Win32;

namespace DTCDev.Client.Cars.Controls.ViewModels.Car
{
    public class CarsNotificationViewModel : BaseViewModel, IDataErrorInfo
    {
        private readonly List<PhoneCarsLink> _phonesCarsList = new List<PhoneCarsLink>();
        private string _selectedPhone;

        public CarsNotificationViewModel()
        {
            _selectedCar = CarSelector.SelectedCar;
            CarSelector.OnCarChanged += CarSelector_OnCarChanged;
        }

        private void CarSelector_OnCarChanged(DISP_Car car)
        {
            _selectedCar = car;
            var link = PhonesCarsList.FirstOrDefault(o => o.Did.Equals(_selectedCar.ID));
            SelectedPhone = link != null ? link.PhoneNumber : null;
            FillControls();
            OnPropertyChanged("EnableButtons");
        }

        private DISP_Car _selectedCar;

        public List<PhoneCarsLink> PhonesCarsList
        {
            get { return _phonesCarsList; }
        }

        private readonly ObservableCollection<string> _listphone = new ObservableCollection<string>();
        public ObservableCollection<string> ListPhone
        {
            get
            {
                return _listphone;
            }
        }

        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set
            {
                if (_phoneNumber == value) return;
                _phoneNumber = value;
                OnPropertyChanged("PhoneNumber");
            }
        }

        public bool IsZoneControl
        {
            get { return _isZoneControl; }
            set
            {
                _isZoneControl = value;
                OnPropertyChanged("IsZoneControl");
            }
        }

        public int SpeedWarning
        {
            get { return _speedWarning; }
            set
            {
                _speedWarning = value;
                OnPropertyChanged("SpeedWarning");
            }
        }

        public string SelectedPhone
        {
            get { return _selectedPhone; }
            set
            {
                if (_selectedPhone == value) return;
                _selectedPhone = value;
                OnPropertyChanged("SelectedPhone");
                if (value != null) PhoneNumber = value;
                SelectCar(value);
                FillControls();
            }
        }

        private void SelectCar(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                CarSelector.OnClearSelection();
                return;
            }
            var list = PhonesCarsList.Where(o => o.PhoneNumber.Equals(value));
            if (list.Any())
                CarSelector.CreateCarSelected(list.Select(o => o.Did));
            if (_selectedCar == null) return;
        }

        void FillControls()
        {
            var link = PhonesCarsList.FirstOrDefault(o => o.Did.Equals(_selectedCar.ID));
            if (link == null) return;
            IsZoneControl = link.IsZoneControl;
            SpeedWarning = link.SpeedWarning;
        }

        public bool EnableButtons { get { return !string.IsNullOrEmpty(PhoneNumber) && _selectedCar != null && ValidatePhone(); } }

        

        private RelayCommand _addPhoneLinkCommand;
        private string _phoneNumber;
        private bool _isZoneControl = true;
        private int _speedWarning = 0;

        public RelayCommand AddPhoneLinkCommand
        {
            get { return _addPhoneLinkCommand ?? (_addPhoneLinkCommand = new RelayCommand(a => AddPhoneLink())); }
        }

        private bool ValidatePhone()
        {
            var rg = new Regex(@"^\+?\d?(\(\d{3,5}\))?[\d- ]+\d$");
            return PhoneNumber != null && rg.IsMatch(PhoneNumber);
        }

        private void AddPhoneLink()
        {
            if (string.IsNullOrEmpty(PhoneNumber) || _selectedCar == null) return;
            var link = PhonesCarsList.FirstOrDefault(o => o.Did.Equals(_selectedCar.ID));
            if (link == null)
            {
                link = new PhoneCarsLink
                {
                    Did = _selectedCar.ID,
                    PhoneNumber = PhoneNumber,
                    SpeedWarning = SpeedWarning,
                    IsZoneControl = IsZoneControl
                };
                PhonesCarsList.Add(link);
                if (!ListPhone.Contains(PhoneNumber))
                    ListPhone.Add(PhoneNumber);
            }
            else
            {
                link.PhoneNumber = PhoneNumber;
                link.IsZoneControl = IsZoneControl;
                link.SpeedWarning = SpeedWarning;
            }
            Save(link);
            var reuse = SelectedPhone == PhoneNumber;
            SelectedPhone = PhoneNumber;
            if (reuse) SelectCar(PhoneNumber);
        }

        private void Save(PhoneCarsLink link)
        {
            //TODO Сохранить привязку в базе
        }

        public string this[string columnName]
        {
            get
            {
                error = string.Empty;
                OnPropertyChanged("EnableButtons");
                if (columnName == "PhoneNumber" && !ValidatePhone())
                {
                    error = PhoneNumber != null ? "Неверный формат телефона!": string.Empty;
                }
                return error;
            }
        }
        private string error = string.Empty;
        public string Error { get { return error; } }
    }

    public class PhoneCarsLink : BaseViewModel
    {
        public string PhoneNumber { get; set; }

        public string Did { get; set; }

        public bool IsZoneControl { get; set; }

        public int SpeedWarning { get; set; }

        public override string ToString()
        {
            return PhoneNumber;
        }
    }
}
