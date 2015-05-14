using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using DTCDev.Client.Cars.Service.Engine.Controls.ViewModels.Settings;
using DTCDev.Client.Cars.Service.Engine.Handlers;
using DTCDev.Client.Cars.Service.Engine.Network;
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
        private UserLightModel _user;
        private DISP_Car _car;
        private string _comment;
        private readonly ObservableCollection<WorksInfoDataModel> _selectedWorks = new ObservableCollection<WorksInfoDataModel>();
        private double _nh;

        public OrderViewModel(OrderViewModel model)
        {
            UpdateOrder(model);
            model.WorksList.ToList().ForEach(o => WorksList.Add(o));
        }

        internal void UpdateOrder(OrderViewModel model)
        {
            ID = model.ID;
            PostID = model.PostID;
            _car = model.Car;
            _comment = model.Comment;
            _dateWork = model.DateWork;
            _isChanged = model.IsChanged;
            _nh = model.NH;
            _user = model.User;

            _selectedWorks.Clear();
            model.SelectedWorks.ToList().ForEach(o => _selectedWorks.Add(o));
        }

        public OrderViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                ID = 1;
                User = new UserLightModel { Nm = "User 1" };
                Car = new DISP_Car{ CarModel = { CarNumber = "Demo2", Mark = "Audio", Model = "A5" }};
                DateWork = DateTime.Now;
                IsChanged = false;
            }
        }

        public ObservableCollection<WorksInfoDataModel> WorksList { get { return _storage.WorkList; } }

        public ObservableCollection<WorksInfoDataModel> SelectedWorks { get { return _selectedWorks; } }

        public List<DISP_Car> Cars { get { return _carStorage.Cars; } }


        public int ID { get; set; }

        public int PostID { get; set; }

        public DateTime DateWork
        {
            get { return _dateWork; }
            set
            {
                IsChanged = _dateWork != value;
                _dateWork = value;
                OnPropertyChanged("DateWork");
            }
        }

        public UserLightModel User
        {
            get { return _user; }
            set
            {
                IsChanged = _user != value;
                _user = value;
                OnPropertyChanged("User");
            }
        }

        public string UserName
        {
            get { return _user != null ? _user.Nm : ""; }
            set
            {
                IsChanged = (_user ?? (_user = new UserLightModel())).Nm != value;
                _user.Nm = value;
                OnPropertyChanged("UserName");
            }
        }

        public DISP_Car Car
        {
            get { return _car; }
            set
            {
                IsChanged = _car != value;
                _car = value;
                OnPropertyChanged("Car");
            }
        }

        public string Comment
        {
            get { return _comment; }
            set
            {
                IsChanged = _comment != value;
                _comment = value;
                OnPropertyChanged("Comment");
            }
        }

        public double NH
        {
            get { return _nh; }
            set
            {
                IsChanged = _nh != value;
                _nh = value;
                OnPropertyChanged("NH");
            }
        }

        private RelayCommand _completeSaveCommand;
        private bool _isChanged;

        public RelayCommand SaveCommand
        {
            get { return _completeSaveCommand ?? (_completeSaveCommand = new RelayCommand(CompleteSave)); }
        }

        public event EventHandler IsCompleteSaved;
        protected virtual void CompleteSave(object sender)
        {
            if (IsCompleteSaved != null) IsCompleteSaved(this, EventArgs.Empty);
        }

        public bool IsChanged
        {
            get { return _isChanged; }
            set
            {
                _isChanged = value;
                OnPropertyChanged("IsChanged");
            }
        }

        public override bool Equals(object obj)
        {
            var order = obj as OrderViewModel;
            return order != null && order.PostID == PostID && order.ID == ID;
        }
    }
}
