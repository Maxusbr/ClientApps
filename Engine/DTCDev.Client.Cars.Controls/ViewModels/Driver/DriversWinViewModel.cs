using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using DTCDev.Client.Cars.Engine.Handlers.Cars;
using DTCDev.Client.ViewModel;
using DTCDev.Models.CarsSending;

namespace DTCDev.Client.Cars.Controls.ViewModels.Driver
{
    public class DriversWinViewModel : ViewModelBase
    {
        public DriversWinViewModel()
        {
            DriverHandler.Instance.DriverRefreshed -= Instance_DriverRefreshed;
            DriverHandler.Instance.DriverRefreshed += Instance_DriverRefreshed;
            DriverHandler.Instance.LoadDriver();
        }

        void Instance_DriverRefreshed(object sender, EventArgs e)
        {
            Drivers.Clear();
            foreach (var item in DriverHandler.Instance.ListDriver)
            {
                Drivers.Add(item);
            }
        }





        #region Properties

        private ObservableCollection<DriverModel> _drivers = new ObservableCollection<DriverModel>();
        public ObservableCollection<DriverModel> Drivers
        {
            get { return _drivers; }
            set
            {
                _drivers = value;
                this.OnPropertyChanged("Drivers");
            }
        }

        #endregion





        private RelayCommand _saveNewCommand;
        public RelayCommand SaveNewCommand
        {
            get
            {
                if (_saveNewCommand == null)
                {
                    _saveNewCommand = new RelayCommand((e) =>
                    {
                        DriverHandler.Instance.EditDriver((DriverModel)e);
                    });
                }
                return _saveNewCommand;
            }
        }

        private RelayCommand _deleteDriverCommand;
        public RelayCommand DeleteDriverCommand
        {
            get
            {
                if (_deleteDriverCommand == null)
                {
                    _deleteDriverCommand = new RelayCommand((e) =>
                    {
                        DriverHandler.Instance.DeleteDriver((DriverModel)e);
                    });
                }
                return _deleteDriverCommand;
            }
        }
    }
}
