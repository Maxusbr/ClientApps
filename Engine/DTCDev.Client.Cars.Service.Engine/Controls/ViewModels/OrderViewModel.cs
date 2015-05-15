using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
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
        private readonly PostsHandler _handler = PostsHandler.Instance;
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
            SelectedWorks.CollectionChanged += SelectedWorks_CollectionChanged;
            _storage.LoadWorkListComplete += _storage_LoadWorkListComplete;
            _storage.UpdateWorks();
        }

        void _storage_LoadWorkListComplete(object sender, EventArgs e)
        {
            
        }

        void SelectedWorks_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            IsChanged = true;
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
                Car = new DISP_Car { CarModel = { CarNumber = "Demo2", Mark = "Audio", Model = "A5" } };
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
                if (value != null && value.Equals(_user)) return;
                IsChanged = _user != value;
                _user = value;
                OnPropertyChanged("User");
                OnPropertyChanged("UserName");
            }
        }

        private UserLightModel _selectedUser;
        public UserLightModel SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                if (value == null) return;
                User = value;
                TextChanged(value.Nm);
            }
        }

        public string UserName
        {
            get { return _user != null ? _user.Nm : ""; }
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

        private bool _isChanged;
        public bool IsChanged
        {
            get { return _isChanged; }
            set
            {
                _isChanged = value;
                OnPropertyChanged("IsChanged");
            }
        }

        private bool _visableUserList = false;
        public bool VisableUserList
        {
            get { return _visableUserList; }
            set
            {
                _visableUserList = value;
                OnPropertyChanged("VisableUserList");
            }
        }

        public bool EnableAddWorkButton
        {
            get { return _selectedWork != null && SelectedWorks.FirstOrDefault(o => o.Equals(_selectedWork)) == null; }
        }

        private string _foundString = string.Empty;
        private CollectionViewSource _listUsers;
        public ICollectionView ListUsers
        {
            get
            {
                if (_listUsers != null) return _listUsers.View;
                _listUsers = new CollectionViewSource { Source = _handler.Users };
                _listUsers.Filter += (o, e) =>
                {
                    e.Accepted = string.IsNullOrEmpty(_foundString) || ((UserLightModel)e.Item).Nm.Contains(_foundString);
                };
                return _listUsers.View;
            }
        }

        public WorksInfoDataModel SelectedWork
        {
            get { return _selectedWork; }
            set
            {
                _selectedWork = value;
                OnPropertyChanged("EnableAddWorkButton");                
            }
        }


        private RelayCommand _completeSaveCommand;
        public RelayCommand SaveCommand
        {
            get { return _completeSaveCommand ?? (_completeSaveCommand = new RelayCommand(CompleteSave)); }
        }

        private RelayCommand _textChangedCommand;
        public RelayCommand TextChangedCommand
        {
            get { return _textChangedCommand ?? (_textChangedCommand = new RelayCommand(TextChanged)); }
        }

        private RelayCommand _addWorkCommand;
        private WorksInfoDataModel _selectedWork;

        public object AddWorkCommand
        {
            get { return _addWorkCommand ?? (_addWorkCommand = new RelayCommand(AddWork)); }
        }

        private void AddWork(object obj)
        {
            if(_selectedWork == null) return;
            SelectedWorks.Add(_selectedWork);
        }

        private void TextChanged(object obj)
        {
            SelectedUser = null;
            VisableUserList = !UserName.Equals(_foundString = obj.ToString());
            if (!VisableUserList) return;
            ListUsers.Refresh();
        }

        public event EventHandler IsCompleteSaved;

        protected virtual void CompleteSave(object sender)
        {
            if (IsCompleteSaved != null) IsCompleteSaved(this, EventArgs.Empty);
        }

        public override bool Equals(object obj)
        {
            var order = obj as OrderViewModel;
            return order != null && order.PostID == PostID && order.ID == ID;
        }
    }
}
