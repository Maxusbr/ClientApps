using DTCDev.Client.Cars.Service.Engine.Handlers;
using DTCDev.Client.ViewModel;
using DTCDev.Models.CarBase.CarStatData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;

namespace DTCDev.Client.Cars.Service.Engine.Controls.ViewModels.Settings
{
    public class SettingsWorkDicViewModel : ViewModelBase
    {
        public SettingsWorkDicViewModel()
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
            PartWorks = new ObservableCollection<PartWorkDataModel>();
            PartsWithWorks = new ObservableCollection<PartWorkWithHoursModel>();
            SpecificationDataStorage.Instance.UpdateWorkTypes();
            SpecificationDataStorage.Instance.UpdateWorks();
            SpecificationDataStorage.Instance.UpdateWorkParts();
            SpecificationDataStorage.Instance.LoadOtherWorkListComplete += Instance_LoadOtherWorkListComplete;
            SpecificationDataStorage.Instance.LoadWorkListComplete += Instance_LoadWorkListComplete;
            SpecificationDataStorage.Instance.PartsWorksLoadComplete += Instance_PartsWorksLoadComplete;
            SpecificationDataStorage.Instance.LoadWorkTypesComplete += Instance_LoadWorkTypesComplete;
            SpecificationDataStorage.Instance.LoadWorkPartsListComplete += Instance_LoadWorkPartsListComplete;
            FilterPartWorks();
        }

        void Instance_LoadWorkPartsListComplete(List<WorksInfoDataModel> data)
        {
            PartsWithWorks.Clear();
            foreach (var item in data)
            {
                PartWorkWithHoursModel model = new PartWorkWithHoursModel(item);
                PartsWithWorks.Add(model);
            }
        }

        void Instance_LoadWorkTypesComplete(object sender, EventArgs e)
        {
            foreach (var item in WorksTree)
            {
                item.Items.Clear();
                foreach (var wt in SpecificationDataStorage.Instance.WorkTypes)
                {
                    item.Items.Add(new WorkTreeModel
                        {
                            id = wt.id,
                            Name = wt.Name,
                            WGUID = "WT"
                        });
                }
            }
        }

        void Instance_PartsWorksLoadComplete(object sender, EventArgs e)
        {
            FilterPartWorks();
        }

        void Instance_LoadWorkListComplete(object sender, EventArgs e)
        {
            WorkTreeModel model = WorksTree.Where(p => p.id == -1).First();
            foreach (var item in model.Items)
            {
                item.Items.Clear();
                List<WorksInfoDataModel> temp = SpecificationDataStorage.Instance.WorkList.Where(p => p.id_Class == item.id).ToList();
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
            WorkTreeModel model = WorksTree.Where(p => p.id == -2).First();
            foreach (var item in model.Items)
            {
                item.Items.Clear();
                List<WorksInfoDataModel> temp = SpecificationDataStorage.Instance.OtherWorkList.Where(p => p.id_Class == item.id).ToList();
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


        public ObservableCollection<WorkTreeModel> WorksTree { get; set; }

        private WorkTreeModel _selectedWorkTree;

        public WorkTreeModel SelectedWorkTree
        {
            get { return _selectedWorkTree; }
            set
            {
                _selectedWorkTree = value;
                this.OnPropertyChanged("SelectedWorkTree");
                if (value != null)
                {
                    if (value.id_Class > 0)
                    {
                        SpecificationDataStorage.Instance.GetWorkParts(value.id);
                    }
                    else
                        PartWorks.Clear();
                }
                else
                    PartWorks.Clear();
            }
        }

        private bool _checkedPeriodic = true;
        
        /// <summary>
        /// Добавление работы. Периодическая работа или нет
        /// </summary>
        public bool CheckedPeriodic
        {
            get { return _checkedPeriodic; }
            set
            {
                _checkedPeriodic = value;
                this.OnPropertyChanged("CheckedPeriodic");
            }
        }

        public ObservableCollection<KVPBase> WorkTypes { get { return SpecificationDataStorage.Instance.WorkTypes; } }

        private KVPBase _selectedWorkType;
        /// <summary>
        /// выбранная категория работ для добавления
        /// </summary>
        public KVPBase SelectedWorkType
        {
            get { return _selectedWorkType; }
            set
            {
                _selectedWorkType = value;
                this.OnPropertyChanged("SelectedWorkType");
            }
        }


        private string _addWorkText = "";
        public string AddWorkText
        {
            get { return _addWorkText; }
            set
            {
                _addWorkText = value;
                this.OnPropertyChanged("AddWorkText");
            }
        }

        private RelayCommand _addWorkCommand;
        public RelayCommand AddWorkCommand { get { return _addWorkCommand ?? (_addWorkCommand = new RelayCommand(AddWork)); } }



        private void AddWork(object sender)
        {
            if (SelectedWorkType != null)
                if (AddWorkText.Length > 2)
                {
                    SpecificationDataStorage.Instance.AddNewWorkName(AddWorkText, SelectedWorkType.id, CheckedPeriodic);
                    AddWorkText = "";

                }
        }







        #region PARTWORKS


        public ObservableCollection<PartWorkDataModel> PartWorks { get; set; }
        public ObservableCollection<PartWorkWithHoursModel> PartsWithWorks { get; set; }

        private Visibility _visAddPartWorks = Visibility.Collapsed;
        public Visibility VisAddPartWorks
        {
            get { return _visAddPartWorks; }
            set
            {
                _visAddPartWorks = value;
                this.OnPropertyChanged("VisAddPartWorks");
            }
        }

        private bool _editPartWorksEnabled = false;
        public bool EditPartWorksEnabled
        {
            get { return _editPartWorksEnabled; }
            set
            {
                _editPartWorksEnabled = value;
                this.OnPropertyChanged("EditPartWorksEnabled");
            }
        }

        private Visibility _visSelectGroup = Visibility.Visible;
        public Visibility VisSelectGroup
        {
            get { return _visSelectGroup; }
            set
            {
                _visSelectGroup = value;
                this.OnPropertyChanged("VisSelectGroup");
            }
        }

        private KVPBase _selectedPartsWorkType;
        public KVPBase SelectedPartsWorkType
        {
            get { return _selectedPartsWorkType; }
            set
            {
                _selectedPartsWorkType = value;
                this.OnPropertyChanged("SelectedPartsWorkType");
                if (value != null)
                {
                    VisAllowPartsAddWork = Visibility.Visible;
                    VisSelectGroup = Visibility.Collapsed;
                }
                else
                {
                    VisAllowPartsAddWork = Visibility.Collapsed;
                    VisSelectGroup = Visibility.Visible;
                }
                FilterPartWorks();
            }
        }

        private Visibility _visAllowPartsAddWork = Visibility.Collapsed;
        public Visibility VisAllowPartsAddWork
        {
            get { return _visAllowPartsAddWork; }
            set
            {
                _visAllowPartsAddWork = value;
                this.OnPropertyChanged("VisAllowPartsAddWork");
            }
        }

        private string _addPartWorkText = "";
        public string AddPartWorkText
        {
            get { return _addPartWorkText; }
            set
            {
                _addPartWorkText = value;
                this.OnPropertyChanged("AddPartWorkText");
            }
        }

        private string _totalNHText = "Всего, н/ч - 0";
        public string TotalNHText
        {
            get { return _totalNHText; }
            set
            {
                _totalNHText = value;
                this.OnPropertyChanged("TotalNHText");
            }
        }

        private bool _deletePartEnabled = false;
        public bool DeletePartEnabled
        {
            get { return _deletePartEnabled; }
            set
            {
                _deletePartEnabled = value;
                this.OnPropertyChanged("DeletePartEnabled");
            }
        }

        private PartWorkWithHoursModel _selectedPart;
        public PartWorkWithHoursModel SelectedPArt
        {
            get { return _selectedPart; }
            set
            {
                _selectedPart = value;
                this.OnPropertyChanged("SelectedPArt");
                if (value == null)
                    DeletePartEnabled = false;
                else
                    DeletePartEnabled = true;
            }
        }


        private RelayCommand _addPartWorksCommand;
        public RelayCommand AddPartWorksCommand
        {
            get { return _addPartWorksCommand ?? (_addPartWorksCommand = new RelayCommand(AddPartWorks)); }
        }

        private RelayCommand _addPartWorkCommand;
        public RelayCommand AddPartWorkCommand { get { return _addPartWorkCommand ?? (_addPartWorkCommand = new RelayCommand(AddPartWork)); } }

        private RelayCommand _selectPartsCompleteCommand;
        public RelayCommand SelectPartsCompleteCommand
        {
            get { return _selectPartsCompleteCommand ?? (_selectPartsCompleteCommand = new RelayCommand(SelectPartsComplete)); }
        }

        private RelayCommand _deletePartCommand;
        public RelayCommand DeletePartCommand
        {
            get { return _deletePartCommand ?? (_deletePartCommand = new RelayCommand(DeletePart)); }
        }

        private void AddPartWorks(object sender)
        {
            VisAddPartWorks = Visibility.Visible;
        }

        private void AddPartWork(object sender)
        {
            if (SelectedPartsWorkType != null)
                if (AddPartWorkText.Length > 2)
                {
                    SpecificationDataStorage.Instance.AddPartWorkName(AddPartWorkText, SelectedPartsWorkType.id);
                    AddPartWorkText = "";
                }
        }

        private void SelectPartsComplete(object sender)
        {
            List<PartWorkDataModel> temp = PartWorks.ToList().Where(p => p.Checked == true).ToList();
            foreach (var item in PartsWithWorks)
            {
                item.HoursChanged -= model_HoursChanged;
            }
            PartsWithWorks.Clear();
            foreach (var item in temp)
            {
                PartWorkWithHoursModel model = new PartWorkWithHoursModel(item.Model);
                model.HoursChanged += model_HoursChanged;
                PartsWithWorks.Add(model);
            }
            VisAddPartWorks = Visibility.Collapsed;
        }

        void model_HoursChanged(object sender, EventArgs e)
        {
            PartWorkWithHoursModel model = (PartWorkWithHoursModel)sender;
            model.Hours = Math.Round(model.Hours, 1);
            TotalNHText = "Всего, н/ч - " + PartsWithWorks.ToList().Sum(p => p.Hours).ToString();
        }

        private void DeletePart(object sender)
        {
            if (SelectedPArt == null)
                return;
            PartsWithWorks.Remove(SelectedPArt);
            TotalNHText = "Всего, н/ч - " + PartsWithWorks.ToList().Sum(p => p.Hours).ToString();
        }



        private void FilterPartWorks()
        {
            PartWorks.Clear();
            if (SpecificationDataStorage.Instance.PartsWorkList.Count() < 1)
                return;
            new Thread(ThrSearchPartWorks).Start();
        }

        private void ThrSearchPartWorks()
        {
            if (SelectedPartsWorkType != null)
            {
                List<WorksInfoDataModel> temp = SpecificationDataStorage.Instance.PartsWorkList.ToList().Where(p => p.id_Class == SelectedPartsWorkType.id).ToList();
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        temp.ForEach(a => PartWorks.Add(new PartWorkDataModel(a)));
                    }));
            }
            else
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        SpecificationDataStorage.Instance.PartsWorkList.ToList().ForEach(a => PartWorks.Add(new PartWorkDataModel(a)));
                    }));
        }

        #endregion





        #region SELECTCAR

        public ObservableCollection<KVPBase> Marks { get { return SpecificationDataStorage.Instance.Marks; } }
        public ObservableCollection<KVPBase> Models { get { return SpecificationDataStorage.Instance.Models; } }
        public ObservableCollection<KVPBase> EngineTypes { get { return SpecificationDataStorage.Instance.EngineTypes; } }
        public ObservableCollection<KVPBase> EngineVolumes { get { return SpecificationDataStorage.Instance.EngineVolumes; } }
        public ObservableCollection<KVPBase> TransmissionTypes { get { return SpecificationDataStorage.Instance.TransTypes; } }
        public ObservableCollection<KVPBase> Bodies { get { return SpecificationDataStorage.Instance.BodyTypes; } }

        public KVPBase Mark
        {
            get { return SpecificationDataStorage.Instance.SelectedMark; }
            set
            {
                if (SpecificationDataStorage.Instance.SelectedMark == value) return;
                SpecificationDataStorage.Instance.SelectedMark = value;
                OnPropertyChanged("Mark");
                Model = null;
                Body = null;
                EngineType = null;
                EngineVolume = null;
                TransmissionType = null;
            }
        }

        public KVPBase Model
        {
            get { return SpecificationDataStorage.Instance.SelectedModel; }
            set
            {
                if (SpecificationDataStorage.Instance.SelectedModel == value) return;
                SpecificationDataStorage.Instance.SelectedModel = value;
                OnPropertyChanged("Model");
                Body = null;
                EngineType = null;
                EngineVolume = null;
                TransmissionType = null;
            }
        }

        public KVPBase Body
        {
            get { return SpecificationDataStorage.Instance.SelectedBody; }
            set
            {
                if (SpecificationDataStorage.Instance.SelectedBody == value) return;
                SpecificationDataStorage.Instance.SelectedBody = value;
                OnPropertyChanged("Body");
                EngineType = null;
                EngineVolume = null;
                TransmissionType = null;
            }
        }

        public KVPBase EngineType
        {
            get { return SpecificationDataStorage.Instance.SelectedEngineType; }
            set
            {
                if (SpecificationDataStorage.Instance.SelectedEngineType == value) return;
                SpecificationDataStorage.Instance.SelectedEngineType = value;
                OnPropertyChanged("EngineType");
                EngineVolume = null;
                TransmissionType = null;
            }
        }

        public KVPBase EngineVolume
        {
            get { return SpecificationDataStorage.Instance.SelectedEngineVolume; }
            set
            {
                if (SpecificationDataStorage.Instance.SelectedEngineVolume == value) return;
                SpecificationDataStorage.Instance.SelectedEngineVolume = value;
                OnPropertyChanged("EngineVolume");
                TransmissionType = null;
            }
        }

        public KVPBase TransmissionType
        {
            get { return SpecificationDataStorage.Instance.SelectedTransmission; }
            set
            {
                if (SpecificationDataStorage.Instance.SelectedTransmission == value) return;
                SpecificationDataStorage.Instance.SelectedTransmission = value;
                OnPropertyChanged("TransmissionType");
            }
        }

        private bool _selectedAllCars = true;
        public bool SelectedAllCars
        {
            get { return _selectedAllCars; }
            set
            {
                _selectedAllCars = value;
                this.OnPropertyChanged("SelectedAllCars");
                SetCarEnabled();
            }
        }

        private bool _selectedModelCars = false;
        public bool SelectedModelCars
        {
            get { return _selectedModelCars; }
            set
            {
                _selectedModelCars = value;
                this.OnPropertyChanged("SelectedModelCars");
                SetCarEnabled();
            }
        }

        private bool _selectedExemplarCars = false;
        public bool SelectedExemplarCars
        {
            get { return _selectedExemplarCars; }
            set
            {
                _selectedExemplarCars = value;
                this.OnPropertyChanged("SelectedExemplarCars");
                SetCarEnabled();
            }
        }

        private bool _enableAddModel = false;
        private bool _enableAddExemplar = false;
        public bool EnableAddModel
        {
            get { return _enableAddModel; }
            set
            {
                _enableAddModel = value;
                this.OnPropertyChanged("EnableAddModel");
            }
        }
        public bool EnableAddExemplar
        {
            get { return _enableAddExemplar; }
            set
            {
                _enableAddExemplar = value;
                this.OnPropertyChanged("EnableAddExemplar");
            }
        }

        private void SetCarEnabled()
        {
            if (SelectedAllCars)
            {
                EnableAddExemplar = false;
                EnableAddModel = false;
                return;
            }
            if (SelectedModelCars)
            {
                EnableAddExemplar = false;
                EnableAddModel = true;
                LoadMarks();
                return;
            }
            if (SelectedExemplarCars)
            {
                EnableAddExemplar = true;
                EnableAddModel = false;
                LoadMarks();
                return;
            }
        }

        private void LoadMarks()
        {
            SpecificationDataStorage.Instance.Update();
        }

        #endregion






        #region CompletePartsButton

        private bool _completeSaveEnabled = true;
        public bool CompleteSaveEnabled
        {
            get { return _completeSaveEnabled; }
            set
            {
                _completeSaveEnabled = value;
                this.OnPropertyChanged("CompleteSaveEnabled");
            }
        }

        private RelayCommand _completeSaveCommand;
        public RelayCommand CompleteSaveCommand { get { return _completeSaveCommand ?? (_completeSaveCommand = new RelayCommand(CompleteSave)); } }
        private void CompleteSave(object sender)
        {
            CarPartsWorkModel request = new CarPartsWorkModel();
            //if (SelectedWork != null)
            //{
            //    request.IDWork = SelectedWork.id;
            //    request.Periodic = 1;
            //}
            //else if (SelectedOtherWork != null)
            //{
            //    request.IDWork = SelectedOtherWork.id;
            //    request.Periodic = 0;
            //}
            //else
            //    return;
            foreach (var item in PartsWithWorks)
            {
                request.Parts.Add(new CarPartsWorkModel.PartsTime
                {
                    PartID = item.Model.id,
                    Time = (int)(item.Hours * 10)
                });
            }
            if (SelectedAllCars)
            {
                request.Exemplar.Saved = 0;
                request.ForAllCars = 1;
            }
            else if (SelectedModelCars)
            {
                request.ForAllCars = 0;
                request.Exemplar.Saved = 0;
                request.ModelID = Model.id;
            }
            else if(SelectedExemplarCars)
            {
                request.ForAllCars=0;
                request.ModelID=0;
                request.Exemplar.Saved=1;
                request.Exemplar.Model =Model.id;
                request.Exemplar.Body = Body.id;
                request.Exemplar.Engine = EngineVolume.id;
                request.Exemplar.EngineType = EngineType.id;
                request.Exemplar.Transmission = TransmissionType.id;
            }

            SpecificationDataStorage.Instance.AddPartsList(request);
        }


        #endregion CompletePartsButton






        public class PartWorkDataModel: ViewModelBase
        {
            public PartWorkDataModel()
            {
            }
            public PartWorkDataModel(WorksInfoDataModel model)
            {
                Model = model;
            }

            public WorksInfoDataModel Model { get; set; }

            private bool _checked = false;
            public bool Checked
            {
                get { return _checked; }
                set
                {
                    _checked = value;
                    this.OnPropertyChanged("Checked");
                }
            }
        }

        public class PartWorkWithHoursModel : ViewModelBase
        {
            public PartWorkWithHoursModel(WorksInfoDataModel model)
            {
                Model = model;
                Hours = model.NH / 10.0m;
            }

            public event EventHandler HoursChanged;

            public WorksInfoDataModel Model { get; set; }

            private decimal _hours = 0;

            public decimal Hours
            {
                get { return _hours; }
                set
                {
                    if (_hours == value)
                        return;
                    _hours = value;
                    this.OnPropertyChanged("Hours");
                    if (HoursChanged != null)
                        HoursChanged(this, new EventArgs());
                }
            }
        }

    }

    public class WorkTreeModel : WorksInfoDataModel
    {
        public WorkTreeModel()
        {
            this.Items = new ObservableCollection<WorkTreeModel>();
        }

        public ObservableCollection<WorkTreeModel> Items { get; set; }
    }
}
