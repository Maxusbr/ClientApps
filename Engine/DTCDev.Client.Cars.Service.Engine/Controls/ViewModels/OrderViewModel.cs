using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using DTCDev.Client.Cars.Service.Engine.Controls.ViewModels.Settings;
using DTCDev.Client.Cars.Service.Engine.Handlers;
using DTCDev.Client.ViewModel;
using DTCDev.Models.CarBase.CarList;
using DTCDev.Models.CarBase.CarStatData;
using DTCDev.Models.User;

namespace DTCDev.Client.Cars.Service.Engine.Controls.ViewModels
{
    public class OrderViewModel : ViewModelBase
    {
        private readonly SpecificationDataStorage _storage = SpecificationDataStorage.Instance;
        private DateTime _dateWork;
        private UserLightModel _user;
        private CarListBaseDataModel _car;
        private string _comment;
        private readonly ObservableCollection<WorksInfoDataModel> _selectedWorks = new ObservableCollection<WorksInfoDataModel>();
        private double _nh;

        public OrderViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                ID = 1;
                User = new UserLightModel {Nm = "User 1"};
                Car = new CarListBaseDataModel {CarNumber = "Demo2", Mark = "Audio", Model = "A5"};
                DateWork = DateTime.Now;
            }
        }

        public ObservableCollection<WorksInfoDataModel> WorksList { get { return _storage.WorkList; } }

        public ObservableCollection<WorksInfoDataModel> SelectedWorks { get { return _selectedWorks; } }

        public int ID { get; set; }

        public DateTime DateWork
        {
            get { return _dateWork; }
            set
            {
                _dateWork = value;
                OnPropertyChanged("DateWork");
            }
        }

        public UserLightModel User
        {
            get { return _user; }
            set
            {
                _user = value;
                OnPropertyChanged("User");
            }
        }

        public CarListBaseDataModel Car
        {
            get { return _car; }
            set
            {
                _car = value;
                OnPropertyChanged("Car");
            }
        }

        public string Comment
        {
            get { return _comment; }
            set
            {
                _comment = value;
                OnPropertyChanged("Comment");
            }
        }

        public double NH
        {
            get { return _nh; }
            set
            {
                _nh = value;
                OnPropertyChanged("NH");
            }
        }
    }
}
