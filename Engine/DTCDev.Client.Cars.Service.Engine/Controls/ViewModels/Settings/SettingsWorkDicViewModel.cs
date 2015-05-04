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
            Works = new ObservableCollection<WorksInfoDataModel>();
            PartWorks = new ObservableCollection<PartWorkDataModel>();
            OtherWorks = new ObservableCollection<WorksInfoDataModel>();
            PartsWithWorks = new ObservableCollection<PartWorkWithHoursModel>();
            SpecificationDataStorage.Instance.UpdateWorkTypes();
            SpecificationDataStorage.Instance.UpdateWorks();
            SpecificationDataStorage.Instance.UpdateWorkParts();
            SpecificationDataStorage.Instance.LoadOtherWorkListComplete += Instance_LoadOtherWorkListComplete;
            SpecificationDataStorage.Instance.LoadWorkListComplete += Instance_LoadWorkListComplete;
            SpecificationDataStorage.Instance.PartsWorksLoadComplete += Instance_PartsWorksLoadComplete;
            SpecificationDataStorage.Instance.LoadWorkTypesComplete += Instance_LoadWorkTypesComplete;
            FilterWorks();
            OtherFilterWorks();
            FilterPartWorks();
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
            FilterWorks();
        }

        void Instance_LoadOtherWorkListComplete(object sender, EventArgs e)
        {
            OtherFilterWorks();
        }


        public ObservableCollection<WorkTreeModel> WorksTree { get; set; }



        #region PERIODIC

        public ObservableCollection<KVPBase> WorkTypes { get { return SpecificationDataStorage.Instance.WorkTypes; } }

        private KVPBase _selectedWorkType;
        public KVPBase SelectedWorkType
        {
            get { return _selectedWorkType; }
            set
            {
                _selectedWorkType = value;
                this.OnPropertyChanged("SelectedWorkType");
                FilterWorks();
                CheckAllowAddWork();
            }
        }

        public ObservableCollection<WorksInfoDataModel> Works { get; set; }

        private WorksInfoDataModel _selectedWork;
        public WorksInfoDataModel SelectedWork
        {
            get { return _selectedWork; }
            set
            {
                _selectedWork = value;
                this.OnPropertyChanged("SelectedWork");
                if (value != null)
                {
                    SelectedOtherWork = null;
                    CheckPartsEnabled();
                }
            }
        }

        private Visibility _visAllowAddWork = Visibility.Collapsed;
        public Visibility VisAllowAddWork
        {
            get { return _visAllowAddWork; }
            set
            {
                _visAllowAddWork = value;
                this.OnPropertyChanged("VisAllowAddWork");
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




        private void FilterWorks()
        {
            Works.Clear();
            if (SelectedWorkType != null)
                foreach (var item in SpecificationDataStorage.Instance.WorkList)
                {
                    if (item.id_Class == SelectedWorkType.id)
                        Works.Add(item);
                }
            else
                SpecificationDataStorage.Instance.WorkList.ToList().ForEach(a => Works.Add(a));
        }

        private void CheckAllowAddWork()
        {
            if (SelectedWorkType == null)
                VisAllowAddWork = Visibility.Collapsed;
            else
                VisAllowAddWork = Visibility.Visible;
        }

        private void AddWork(object sender)
        {
            if(SelectedWorkType!=null)
                if (AddWorkText.Length > 2)
                {
                    SpecificationDataStorage.Instance.AddNewWorkName(AddWorkText, SelectedWorkType.id);
                    AddWorkText = "";

                }
        }


        #endregion PERIODIC





        #region APERIODIC

        public ObservableCollection<KVPBase> OtherWorkTypes { get { return SpecificationDataStorage.Instance.WorkTypes; } }

        private KVPBase _otherSelectedWorkType;
        public KVPBase OtherSelectedWorkType
        {
            get { return _otherSelectedWorkType; }
            set
            {
                _otherSelectedWorkType = value;
                this.OnPropertyChanged("OtherSelectedWorkType");
                OtherFilterWorks();
                OtherCheckAllowAddWork();
            }
        }

        public ObservableCollection<WorksInfoDataModel> OtherWorks { get; set; }

        private WorksInfoDataModel _selectedOtherWork;
        public WorksInfoDataModel SelectedOtherWork
        {
            get { return _selectedOtherWork; }
            set
            {
                _selectedOtherWork = value;
                this.OnPropertyChanged("SelectedOtherWork");
                if (value != null)
                {
                    SelectedWork = null;
                    CheckPartsEnabled();
                }
            }
        }

        private Visibility _visOtherAllowAddWork = Visibility.Collapsed;
        public Visibility VisOtherAllowAddWork
        {
            get { return _visOtherAllowAddWork; }
            set
            {
                _visOtherAllowAddWork = value;
                this.OnPropertyChanged("VisOtherAllowAddWork");
            }
        }

        private string _addOtherWorkText = "";
        public string AddOtherWorkText
        {
            get { return _addOtherWorkText; }
            set
            {
                _addOtherWorkText = value;
                this.OnPropertyChanged("AddOtherWorkText");
            }
        }





        private RelayCommand _addOtherWorkCommand;
        public RelayCommand AddOtherWorkCommand { get { return _addOtherWorkCommand ?? (_addOtherWorkCommand = new RelayCommand(AddOtherWork)); } }




        private void OtherFilterWorks()
        {
            OtherWorks.Clear();
            if (SpecificationDataStorage.Instance.OtherWorkList.Count() < 1)
                return;
            if (OtherSelectedWorkType != null)
                foreach (var item in SpecificationDataStorage.Instance.OtherWorkList)
                {
                    if (item.id_Class == OtherSelectedWorkType.id)
                        OtherWorks.Add(item);
                }
            else
                SpecificationDataStorage.Instance.OtherWorkList.ToList().ForEach(a => OtherWorks.Add(a));
        }

        private void OtherCheckAllowAddWork()
        {
            if (OtherSelectedWorkType == null)
                VisOtherAllowAddWork = Visibility.Collapsed;
            else
                VisOtherAllowAddWork = Visibility.Visible;
        }

        private void AddOtherWork(object sender)
        {
            if (OtherSelectedWorkType != null)
                if (AddOtherWorkText.Length > 2)
                {
                    SpecificationDataStorage.Instance.AddOtherWorkName(AddOtherWorkText, OtherSelectedWorkType.id);
                    AddOtherWorkText = "";

                }
        }


        #endregion APERIODIC





        #region PARTWORKS

        private void CheckPartsEnabled()
        {
            if (SelectedWork == null && SelectedOtherWork == null)
                EditPartWorksEnabled = false;
            else
            {
                EditPartWorksEnabled = true;
                SpecificationDataStorage.Instance.GetWorkParts((SelectedWork ?? SelectedOtherWork).idWork);
            }
        }

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
            if (SelectedWork != null)
            {
                request.IDWork = SelectedWork.id;
                request.Periodic = 1;
            }
            else if (SelectedOtherWork != null)
            {
                request.IDWork = SelectedOtherWork.id;
                request.Periodic = 0;
            }
            else
                return;
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
