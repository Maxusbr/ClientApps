using DTCDev.Client.Cars.Engine.Handlers.Cars;
using DTCDev.Client.ViewModel;
using DTCDev.Models.CarBase.CarStatData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace DTCDev.Client.Cars.Controls.ViewModels.Settings
{
    public class SettingsCarsViewModel : ViewModelBase
    {
        public SettingsCarsViewModel()
        {
            CarsHandler.Instance.SettingsLoaded += Instance_SettingsLoaded;
            CarsHandler.Instance.GetCarSettings();
        }

        void Instance_SettingsLoaded(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(delegate()
            {
                Items.Clear();
                foreach (var item in CarsHandler.Instance.Settings)
                {
                    Items.Add(new SettingsItem(item));
                }
            }));
        }

        private ObservableCollection<SettingsItem> _items = new ObservableCollection<SettingsItem>();

        public ObservableCollection<SettingsItem> Items { get { return _items; } }



        public class SettingsItem: ViewModelBase
        {
            public SettingsItem(CarSettings model)
            {
                _settings = model;
                Name = model.CarName;
            }

            private CarSettings _settings = new CarSettings();

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

            private RelayCommand _saveCommand;

            public RelayCommand SaveCommand
            {
                get
                {
                    if (_saveCommand == null)
                        _saveCommand = new RelayCommand(a => OnSaveData());
                    return _saveCommand;
                }
            }

            private void OnSaveData()
            {
                _settings.VIN = VIN;
                _settings.CarName = Name;

                CarsHandler.Instance.SaveCarSettings(_settings);
            }
        }
    }
}
