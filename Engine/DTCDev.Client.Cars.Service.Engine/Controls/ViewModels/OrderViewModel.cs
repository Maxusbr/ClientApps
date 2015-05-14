using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using DTCDev.Client.Cars.Service.Engine.Controls.ViewModels.Settings;
using DTCDev.Client.Cars.Service.Engine.Handlers;
using DTCDev.Client.Cars.Service.Engine.Storage;
using DTCDev.Client.ViewModel;
using DTCDev.Models.CarBase.CarList;
using DTCDev.Models.CarBase.CarStatData;
using DTCDev.Models.User;

namespace DTCDev.Client.Cars.Service.Engine.Controls.ViewModels
{
    public class OrderViewModel : ViewModelBase
    {
        private readonly SpecificationDataStorage _storage = SpecificationDataStorage.Instance;
        private readonly CarStorage _carStorage = CarStorage.Instance;
        private DateTime _dateWork;
        private UserLightModel _user = new UserLightModel{ Nm = ""};
        private DISP_Car _car;
        private string _comment;
        private readonly ObservableCollection<WorksInfoDataModel> _selectedWorks = new ObservableCollection<WorksInfoDataModel>();
        private ObservableCollection<DISP_Car> _cars;
        private double _nh;


        public OrderViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                ID = 1;
                User = new UserLightModel { Nm = "User 1" };
                Car = new DISP_Car{ CarModel = { CarNumber = "Demo2", Mark = "Audio", Model = "A5" }};
                DateWork = DateTime.Now;
            }
            _carStorage.LoadComplete += _carStorage_LoadComplete;

            ReloadCarList();
        }

        private void _carStorage_LoadComplete(object sender, EventArgs e)
        {
            ReloadCarList();
        }

        private void ReloadCarList()
        {
            Cars.Clear();
            _carStorage.Cars.ForEach(o => Cars.Add(o));
        }

        public ObservableCollection<WorksInfoDataModel> WorksList { get { return _storage.WorkList; } }

        public ObservableCollection<WorksInfoDataModel> SelectedWorks { get { return _selectedWorks; } }

        public ObservableCollection<DISP_Car> Cars { get { return _cars ?? (_cars = new ObservableCollection<DISP_Car>()); } }


        public int ID { get; set; }

        public int PostID { get; set; }

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

        public string UserName
        {
            get { return _user.Nm; }
            set
            {
                _user.Nm = value;
                OnPropertyChanged("UserName");
            }
        }

        public DISP_Car Car
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

        //private RelayCommand _completeSaveCommand;
        //public RelayCommand SaveCommand
        //{
        //    get { return _completeSaveCommand ?? (_completeSaveCommand = new RelayCommand(CompleteSave)); }
        //}

        //public event EventHandler IsCompleteSaved;
        //protected virtual void CompleteSave(object sender)
        //{
        //    if (IsCompleteSaved != null) IsCompleteSaved(this, EventArgs.Empty);
        //}

        internal void Update(OrderViewModel ord)
        {
            NH = ord.NH;
            User = ord.User;
            Car = ord.Car;
            Comment = ord.Comment;
            DateWork = ord.DateWork;
            foreach (var el in ord.SelectedWorks)
            {
                var work = SelectedWorks.FirstOrDefault(o => o.idWork == el.idWork);
                if (work == null)
                    SelectedWorks.Add(el);
                else
                {
                    work.Cost = el.Cost;
                    work.NH = el.NH;
                    work.NHD = el.NHD;
                    work.Name = el.Name;
                    work.WGUID = el.WGUID;
                    work.id_Class = el.id_Class;
                    work.isPeriodic = el.isPeriodic;
                }
            }
        }
    }
}
