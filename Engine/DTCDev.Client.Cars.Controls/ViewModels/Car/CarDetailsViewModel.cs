using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using DTCDev.Client.Cars.Engine.DisplayModels;
using DTCDev.Client.Controls.Map;
using DTCDev.Client.ViewModel;
using DTCDev.Models.CarBase.CarStatData;
using DTCDev.Client.Cars.Engine.Handlers.Cars;

namespace DTCDev.Client.Cars.Controls.ViewModels.Car
{
    public class CarDetailsViewModel : ViewModelBase
    {
        public CarDetailsViewModel()
        {
            
        }

        private DISP_Car _car;
        public DISP_Car CAR
        {
            get { return _car; }
            set
            {
                _car = value;
                this.OnPropertyChanged("CAR");
                UpdateData();
            }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                this.OnPropertyChanged("Name");
            }
        }

        private string _vin;

        public string VIN
        {
            get { return _vin; }
            set
            {
                _vin = value;
                this.OnPropertyChanged("VIN");
            }
        }

        private void UpdateData()
        {
            if (CAR == null)
                return;
            Name = CAR.Name;
            VIN = CAR.VIN;
        }

        private RelayCommand _saveSettingsCommand;

        public RelayCommand SaveSettingsCommand
        {
            get
            {
                if (_saveSettingsCommand == null)
                    _saveSettingsCommand = new RelayCommand(a=>SaveSettings());
                return _saveSettingsCommand;
            }
        }

        private void SaveSettings()
        {
            CarSettings settings = new CarSettings();
            settings.DID = CAR.ID;
            settings.CarName = Name;
            settings.VIN = VIN;
            CAR.Name = Name;
            CAR.VIN = VIN;
            CarsHandler.Instance.SaveCarSettings(settings);
        }
    }
}
