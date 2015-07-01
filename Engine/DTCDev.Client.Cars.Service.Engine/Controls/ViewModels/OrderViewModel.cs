using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        private UserLightModel _selectedUser;
        private bool _isChanged;
        private bool _visableUserList = false;
        private bool _canDeleted = true;
        private DateTime _dateProduce = DateTime.Now;

        public OrderViewModel(OrderViewModel model)
        {
            WorksTree = new ObservableCollection<WorkTreeModel>();
            WorksTree.Add(new WorkTreeModel
            {
                Name = "Периодические",
                id = -1
            });
            WorksTree.Add(new WorkTreeModel
            {
                Name = "Остальные",
                id = -2
            });

            UpdateOrder(model);
            model.WorksList.ToList().ForEach(o => WorksList.Add(o));
            SelectedWorks.CollectionChanged += SelectedWorks_CollectionChanged;
            _storage.LoadOtherWorkListComplete += Instance_LoadOtherWorkListComplete;
            _storage.LoadWorkListComplete += Instance_LoadWorkListComplete;
            _storage.LoadWorkTypesComplete += Instance_LoadWorkTypesComplete;

            _storage.LoadMarksComplete += _storage_LoadMarksComplete;
            _storage.LoadModelsComplete += _storage_LoadModelsComplete;
            _storage.LoadBodiesComplete += _storage_LoadBodiesComplete;
            _storage.LoadEngineTypesComplete += _storage_LoadEngineTypesComplete;
            _storage.LoadEnginsComplete += _storage_LoadEnginsComplete;
            _storage.LoadTransmissionsComplete += _storage_LoadTransmissionsComplete;

            _storage.Update();
            _storage.UpdateWorkTypes();
            
        }

        void _storage_LoadBodiesComplete(object sender, EventArgs e)
        {

        }

        void _storage_LoadMarksComplete(object sender, EventArgs e)
        {

        }

        private void _storage_LoadTransmissionsComplete(object sender, EventArgs e)
        {
            ;
        }

        private void _storage_LoadEnginsComplete(object sender, EventArgs e)
        {

        }

        void _storage_LoadEngineTypesComplete(object sender, EventArgs e)
        {

        }

        private void _storage_LoadModelsComplete(object sender, EventArgs e)
        {

        }

        void Instance_LoadWorkTypesComplete(object sender, EventArgs e)
        {
            foreach (var item in WorksTree)
            {
                item.Items.Clear();
                foreach (var wt in _storage.WorkTypes)
                {
                    item.Items.Add(new WorkTreeModel
                    {
                        id = wt.id,
                        Name = wt.Name,
                        WGUID = "WT"
                    });
                }
            }
            _storage.UpdateWorks();
        }

        void Instance_LoadWorkListComplete(object sender, EventArgs e)
        {
            var model = WorksTree.First(p => p.id == -1);
            foreach (var item in model.Items)
            {
                item.Items.Clear();
                var temp = _storage.WorkList.Where(p => p.id_Class == item.id).ToList();
                foreach (var w in temp)
                {
                    item.Items.Add(new WorkTreeModel
                    {
                        Cost = w.Cost,
                        id = w.id,
                        id_Class = w.id_Class,
                        idWork = w.idWork,
                        Name = w.Name,
                        NH = w.NH,
                        NHD = w.NHD,
                        WGUID = w.WGUID
                    });
                }
            }
        }

        void Instance_LoadOtherWorkListComplete(object sender, EventArgs e)
        {
            var model = WorksTree.First(p => p.id == -2);
            foreach (var item in model.Items)
            {
                item.Items.Clear();
                var temp = _storage.OtherWorkList.Where(p => p.id_Class == item.id).ToList();
                foreach (var w in temp)
                {
                    item.Items.Add(new WorkTreeModel
                    {
                        Cost = w.Cost,
                        id = w.id,
                        id_Class = w.id_Class,
                        idWork = w.idWork,
                        Name = w.Name,
                        NH = w.NH,
                        NHD = w.NHD,
                        WGUID = w.WGUID
                    });
                }
            }
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
            
            _user = model.User ?? new UserLightModel();
            VisableUserList = model.User == null;
            _foundString = model.User != null ? _user.Nm : "";

            _selectedWorks.Clear();
            CanDeleted = model.CanDeleted;
            model.SelectedWorks.ToList().ForEach(o => _selectedWorks.Add(o));
        }

        public OrderViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                ID = 1;
                User = new UserLightModel { Nm = "Иванов Петр Иванович" };
                Car = new DISP_Car { CarModel = { CarNumber = "Demo2", Mark = "Audio", Model = "A5" } };
                DateWork = DateTime.Now;
                IsChanged = false;
                _foundString = "User";
            }
        }

        #region CarDetail

        public ObservableCollection<KVPBase> Marks { get { return _storage.Marks; } }
        public ObservableCollection<KVPBase> Models { get { return _storage.Models; } }
        public ObservableCollection<KVPBase> EngineTypes { get { return _storage.EngineTypes; } }
        public ObservableCollection<KVPBase> EngineVolumes { get { return _storage.EngineVolumes; } }
        public ObservableCollection<KVPBase> TransmissionTypes { get { return _storage.TransTypes; } }
        public ObservableCollection<KVPBase> Bodies { get { return _storage.BodyTypes; } }

        private void IsValidateCarNumber()
        {
            var rg = new Regex(@"[A-ZА-Я]\d{3}[A-ZА-Я]{2}\d{2,3}");
            ValidateCarNumber = string.IsNullOrEmpty(CarNumber) || !rg.IsMatch(CarNumber);
        }

        private string _carNumber;
        public string CarNumber
        {
            get { return _carNumber; }
            set
            {
                if (_carNumber == value) return;
                _carNumber = value;
                OnPropertyChanged("CarNumber");
                IsValidateCarNumber();
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

        private bool _validateCarNumber;
        public bool ValidateCarNumber
        {
            get { return _validateCarNumber; }
            set
            {
                if (_validateCarNumber == value) return;
                _validateCarNumber = value;
                OnPropertyChanged("ValidateCarNumber");
            }
        }

        public KVPBase Mark
        {
            get { return _storage.SelectedMark; }
            set
            {
                _storage.SelectedMark = value;
                OnPropertyChanged("Mark");
            }
        }

        public KVPBase Model
        {
            get { return _storage.SelectedModel; }
            set
            {
                _storage.SelectedModel = value;
                OnPropertyChanged("Model");
            }
        }

        public KVPBase Body
        {
            get { return _storage.SelectedBody; }
            set
            {
                _storage.SelectedBody = value;
                OnPropertyChanged("Body");
            }
        }

        public KVPBase EngineType
        {
            get { return _storage.SelectedEngineType; }
            set
            {
                _storage.SelectedEngineType = value;
                OnPropertyChanged("EngineType");
            }
        }

        public KVPBase EngineVolume
        {
            get { return _storage.SelectedEngineVolume; }
            set
            {
                _storage.SelectedEngineVolume = value;
                OnPropertyChanged("EngineVolume");
            }
        }

        private int intEngineVolume
        {
            get
            {
                var res = 0;
                if (_storage.SelectedEngineVolume != null)
                    int.TryParse(_storage.SelectedEngineVolume.Name, out res);
                return res;
            }
        }

        public KVPBase TransmissionType
        {
            get { return _storage.SelectedTransmission; }
            set
            {
                if (_storage.SelectedTransmission == value) return;
                _storage.SelectedTransmission = value;
                OnPropertyChanged("TransmissionType");
            }
        }

        public DateTime DateProduce
        {
            get { return _dateProduce; }
            set
            {
                if (_dateProduce == value) return;
                _dateProduce = value;
                OnPropertyChanged("DateProduce");
            }
        }

        #endregion

        public ObservableCollection<WorkTreeModel> WorksTree { get; set; }

        private WorkTreeModel _selectedWorkTree;
        public WorkTreeModel SelectedWorkTree
        {
            get { return _selectedWorkTree; }
            set
            {
                _selectedWorkTree = value;
                OnPropertyChanged("SelectedWorkTree");
                OnPropertyChanged("EnableAddWorkButton");
            }
        }
        public ObservableCollection<WorksInfoDataModel> OtherWorksList { get { return _storage.OtherWorkList; } }
        public ObservableCollection<WorksInfoDataModel> WorksList { get { return _storage.WorkList; } }

        private WorksInfoDataModel _selectedWork;
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
            get { return _user != null && !string.IsNullOrEmpty(_user.Nm) ? _user.Nm : ""; }
        }

        public string Phone
        {
            get { return _user.Ph; }
            set
            {
                _user.Ph = value;
                OnPropertyChanged("Phone");
            }
        }

        public string Email
        {
            get { return _user.Em; }
            set
            {
                _user.Em = value;
                OnPropertyChanged("Email");
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

        public bool IsChanged
        {
            get { return _isChanged; }
            set
            {
                _isChanged = value;
                OnPropertyChanged("IsChanged");
            }
        }

        public bool VisableUserList
        {
            get { return _visableUserList; }
            set
            {
                _visableUserList = value;
                OnPropertyChanged("VisableAddUser");
                OnPropertyChanged("VisableUserList");
            }
        }

        private bool _visableAddUser = false;
        public bool VisableAddUser
        {
            get { return !string.IsNullOrEmpty(_foundString) && 
                !_handler.Users.Any(o => o.Nm.ToLower().Contains(_foundString.ToLower())); }
            set
            {
                _visableAddUser = value;
                OnPropertyChanged("VisableAddUser");
            }
        }

        public bool EnableAddWorkButton
        {
            get
            {
                return SelectedWorkTree != null && SelectedWorkTree.id_Class > 0;
                //_selectedWork != null && SelectedWorks.FirstOrDefault(o => o.Equals(_selectedWork)) == null;
            }
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
                    e.Accepted = string.IsNullOrEmpty(_foundString) ||
                        ((UserLightModel)e.Item).Nm.ToLower().Contains(_foundString.ToLower());
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

        public bool IsSaved { get; set; }

        public bool CanDeleted
        {
            get { return _canDeleted; }
            set
            {
                _canDeleted = value;
                OnPropertyChanged("CanDeleted");
            }
        }

        public override bool Equals(object obj)
        {
            var order = obj as OrderViewModel;
            return order != null && order.PostID == PostID && order.ID == ID;
        }



        #region RelayCommands

        private RelayCommand _textChangedCommand;
        public RelayCommand TextChangedCommand
        {
            get { return _textChangedCommand ?? (_textChangedCommand = new RelayCommand(TextChanged)); }
        }

        private void TextChanged(object obj)
        {
            SelectedUser = null;
            VisableUserList = !UserName.Equals(_foundString = obj.ToString()) || string.IsNullOrEmpty(_foundString);
            if (VisableUserList)
                ListUsers.Refresh();
            else
                OnPropertyChanged("UserName");
            VisableUserList = VisableUserList && _listUsers.View.Cast<object>().Any();
        }

        private RelayCommand _cancelCommand;
        public RelayCommand CancelCommand
        {
            get { return _cancelCommand ?? (_cancelCommand = new RelayCommand(Cancel)); }
        }

        private void Cancel(object obj)
        {
            IsSaved = false;
            if (IsCompleteSaved != null) IsCompleteSaved(this, EventArgs.Empty);
        }

        private RelayCommand _completeSaveCommand;
        public RelayCommand SaveCommand
        {
            get { return _completeSaveCommand ?? (_completeSaveCommand = new RelayCommand(CompleteSave)); }
        }

        public event EventHandler IsCompleteSaved;
        protected virtual void CompleteSave(object sender)
        {
            IsSaved = true;
            if (IsCompleteSaved != null) IsCompleteSaved(this, EventArgs.Empty);
        }

        private RelayCommand _addWorkCommand;
        public object AddWorkCommand
        {
            get { return _addWorkCommand ?? (_addWorkCommand = new RelayCommand(AddWork)); }
        }

        private void AddWork(object obj)
        {
            //if (_selectedWork == null) return;
            //SelectedWorks.Add(_selectedWork);

            if (SelectedWorkTree == null) return;
            var work = WorksList.FirstOrDefault(o => o.Name.Equals(SelectedWorkTree.Name) && o.id_Class == SelectedWorkTree.id_Class) 
                ?? OtherWorksList.FirstOrDefault(o => o.Name.Equals(SelectedWorkTree.Name) && o.id_Class == SelectedWorkTree.id_Class);
            if(work == null || SelectedWorks.IndexOf(work) >= 0) return;
            SelectedWorks.Add(work);
        }

        private RelayCommand _deleteCommand;
        public RelayCommand DeleteCommand
        {
            get { return _deleteCommand ?? (_deleteCommand = new RelayCommand(DeleteOrder)); }
        }

        private void DeleteOrder(object obj)
        {
            if (CanDeleted)
                OnDeleted(this);
        }

        public event EventHandler Deleted;
        protected virtual void OnDeleted(object sender)
        {
            if (Deleted != null) Deleted(sender, EventArgs.Empty);
        }


        private RelayCommand _deleteWorkCommand;
        

        public RelayCommand DeleteWorkCommand
        {
            get { return _deleteWorkCommand ?? (_deleteWorkCommand = new RelayCommand(DeleteWork)); }
        }

        private void DeleteWork(object obj)
        {
            var item = obj as System.Windows.Controls.ListBoxItem;
            if (item == null) return;
            var work = item.Content as WorksInfoDataModel;
            if(work == null) return;
            SelectedWorks.Remove(work);
        }
        #endregion
    }
}
