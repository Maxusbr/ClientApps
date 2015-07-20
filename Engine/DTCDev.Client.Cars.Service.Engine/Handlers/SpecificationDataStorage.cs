using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using DTCDev.Models.CarBase.CarStatData;

namespace DTCDev.Client.Cars.Service.Engine.Handlers
{
    public class SpecificationDataStorage
    {
        private static SpecificationDataStorage _instance;
        public static SpecificationDataStorage Instance
        {
            get { return _instance ?? (_instance = new SpecificationDataStorage()); }
        }

        public SpecificationDataStorage()
        {
            _instance = this;
        }

        private readonly ObservableCollection<KVPBase> _marks = new ObservableCollection<KVPBase>();
        private readonly ObservableCollection<KVPBase> _models = new ObservableCollection<KVPBase>();
        private readonly ObservableCollection<KVPBase> _engineTypes = new ObservableCollection<KVPBase>();
        private readonly ObservableCollection<KVPBase> _engineVolumes = new ObservableCollection<KVPBase>();
        private readonly ObservableCollection<KVPBase> _bodyTypes = new ObservableCollection<KVPBase>();
        private readonly ObservableCollection<KVPBase> _transTypes = new ObservableCollection<KVPBase>();
        private readonly ObservableCollection<KVPBase> _worksTypes = new ObservableCollection<KVPBase>();
        private readonly ObservableCollection<WorksInfoDataModel> _worksList = new ObservableCollection<WorksInfoDataModel>();
        private readonly ObservableCollection<WorksInfoDataModel> _otherWorksList = new ObservableCollection<WorksInfoDataModel>();
        private readonly ObservableCollection<WorksInfoDataModel> _partsWorksList = new ObservableCollection<WorksInfoDataModel>();
        private readonly ObservableCollection<WorksInfoExemplarDataModel> _carWorkList = new ObservableCollection<WorksInfoExemplarDataModel>();
        private readonly ObservableCollection<CarStatInfoModel.PIDInfo> _pids = new ObservableCollection<CarStatInfoModel.PIDInfo>();
        

        public ObservableCollection<KVPBase> Marks { get { return _marks; } }
        public ObservableCollection<KVPBase> Models { get { return _models; } }
        public ObservableCollection<KVPBase> EngineTypes { get { return _engineTypes; } }
        public ObservableCollection<KVPBase> EngineVolumes { get { return _engineVolumes; } }
        public ObservableCollection<KVPBase> BodyTypes { get { return _bodyTypes; } }
        public ObservableCollection<KVPBase> TransTypes { get { return _transTypes; } }
        public ObservableCollection<KVPBase> WorkTypes { get { return _worksTypes; } }
        public ObservableCollection<WorksInfoDataModel> WorkList { get { return _worksList; } }
        public ObservableCollection<WorksInfoDataModel> OtherWorkList { get { return _otherWorksList; } }
        public ObservableCollection<WorksInfoDataModel> PartsWorkList { get { return _partsWorksList; } }
        public ObservableCollection<WorksInfoExemplarDataModel> CarWorkList{get{return _carWorkList;}}
        public ObservableCollection<CarStatInfoModel.PIDInfo> PIDS { get { return _pids; } }
        public string CarProtocolType { get; set; }



        public event EventHandler LoadCarStatComplete;
        public event EventHandler LoadWorkListComplete;
        public event EventHandler LoadOtherWorkListComplete;
        public event EventHandler PartsWorksLoadComplete;
        public event EventHandler LoadWorkTypesComplete;
        public event EventHandler LoadModelsComplete;
        public event EventHandler LoadMarksComplete;
        public event EventHandler LoadBodiesComplete;
        public event EventHandler LoadEngineTypesComplete;
        public event EventHandler LoadEnginsComplete;
        public event EventHandler LoadTransmissionsComplete;

        public delegate void LoadWorkPartsListHandler(List<WorksInfoDataModel> data);
        public event LoadWorkPartsListHandler LoadWorkPartsListComplete;



        private KVPBase _selectedMark;
        private KVPBase _selectedModel;
        private KVPBase _selectedEngine;
        private KVPBase _selectedEngineType;
        private KVPBase _selectedBody;
        private KVPBase _selectedTransmission;

        public KVPBase SelectedMark
        {
            get { return _selectedMark; }
            set
            {
                _selectedMark = value;
                CallLoadModels();
            }
        }

        public KVPBase SelectedModel
        {
            get { return _selectedModel; }
            set
            {
                _selectedModel = value;
                CallLoadBodies();
            }
        }

        public KVPBase SelectedBody
        {
            get { return _selectedBody; }
            set
            {
                _selectedBody = value;
                CallLoadEngineTypes();
            }
        }

        public KVPBase SelectedEngineType
        {
            get { return _selectedEngineType; }
            set
            {
                _selectedEngineType = value;
                CallLoadEngines();
            }
        }

        public KVPBase SelectedEngineVolume
        {
            get { return _selectedEngine; }
            set
            {
                _selectedEngine = value;
                CallLoadTrans();
            }
        }

        public KVPBase SelectedTransmission
        {
            get { return _selectedTransmission; }
            set
            {
                _selectedTransmission = value;
                if (value != null)
                    CallLoadCarWorks();
            }
        }


        private void CallLoadMarks()
        {
            CarBaseHandler.Instance.GetMarks();
        }

        private void CallLoadModels()
        {
            if (SelectedMark != null)
                CarBaseHandler.Instance.GetModels(SelectedMark.id);
        }

        private void CallLoadEngines()
        {
            if (SelectedModel != null && SelectedEngineType != null)
                CarBaseHandler.Instance.GetEngines(SelectedModel.id, SelectedEngineType.id);
        }

        private void CallLoadBodies()
        {
            if (SelectedModel != null)
                CarBaseHandler.Instance.GetBodies(_selectedModel.id);
        }

        private void CallLoadEngineTypes()
        {
            if (SelectedModel != null && SelectedBody != null)
                CarBaseHandler.Instance.GetEngineTypes(SelectedModel.id, SelectedBody.id);
        }

        private void CallLoadTrans()
        {
            if (SelectedModel != null && SelectedEngineVolume != null)
                CarBaseHandler.Instance.GetTransTypes(SelectedModel.id, SelectedEngineVolume.id);
        }

        private void CallLoadCarWorks()
        {
            CarBaseHandler.Instance.GetCarWorksEvent(_selectedModel.id, _selectedEngine.id, _selectedBody.id, SelectedEngineType.id, SelectedTransmission.id);
        }

        public void Update()
        {
            if (!_marks.Any())
                CallLoadMarks();
        }

        public void UpdateWorks()
        {
            CarBaseHandler.Instance.GetWorksList(this, new EventArgs());
        }

        public void UpdateWorkTypes()
        {
                CarBaseHandler.Instance.GetWorkTypes(this, new EventArgs());
        }

        public void UpdateWorkParts()
        {
            CarBaseHandler.Instance.UpdatePartsWorks();
        }

        public void AddPartWorkName(string name, int typeID)
        {
            CarBaseHandler.Instance.AddPartWorkName(name, typeID);
        }

        public void AddPartsList(CarPartsWorkModel model)
        {
            CarBaseHandler.Instance.AddPartsList(model);
        }

        public void GetWorkParts(int workID, string carNumber, bool periodic)
        {
            CarBaseHandler.Instance.GetWorkParts(workID, carNumber, periodic);
        }

        public void GetWorkParts(int workID, string carNumber)
        {
            CarBaseHandler.Instance.GetWorkParts(workID, carNumber);
        }

        public void GetWorkParts(int workID)
        {
            CarBaseHandler.Instance.GetWorkParts(workID);
        }


        public void SetMarks(List<KVPBase> data)
        {
            data = data.OrderBy(p => p.Name).ToList();
            Marks.Clear();
            data.ForEach(o => Marks.Add(o));
            if (LoadMarksComplete != null)
                LoadMarksComplete(this, new EventArgs());
        }

        public void SetModels(List<KVPBase> data)
        {
            data = data.OrderBy(p => p.Name).ToList();
            Models.Clear();
            data.ForEach(o => Models.Add(o));
            if(LoadModelsComplete != null)
                LoadModelsComplete(this, new EventArgs());
        }

        public void SetEngineTypes(List<KVPBase> data)
        {
            data = data.OrderBy(p => p.Name).ToList();
            EngineTypes.Clear();
            data.ForEach(o => EngineTypes.Add(o));
            if (LoadEngineTypesComplete != null)
                LoadEngineTypesComplete(this, new EventArgs());
        }

        public void SetEngineVolumes(List<KVPBase> data)
        {
            data = data.OrderBy(p => p.Name).ToList();
            EngineVolumes.Clear();
            data.ForEach(o => EngineVolumes.Add(o));
            if (LoadEnginsComplete != null)
                LoadEnginsComplete(this, new EventArgs());

        }

        public void SetBodyTypes(List<KVPBase> data)
        {
            data = data.OrderBy(p => p.Name).ToList();
            BodyTypes.Clear();
            data.ForEach(o => BodyTypes.Add(o));
            if (LoadBodiesComplete != null)
                LoadBodiesComplete(this, new EventArgs());
        }

        public void SetTransTypes(List<KVPBase> data)
        {
            data = data.OrderBy(p => p.Name).ToList();
            TransTypes.Clear();
            data.ForEach(o => TransTypes.Add(o));
            if (LoadTransmissionsComplete != null)
                LoadTransmissionsComplete(this, new EventArgs());
        }

        public void SetWorkTypes(List<KVPBase> data)
        {
            data = data.OrderBy(p => p.Name).ToList();
            WorkTypes.Clear();
            data.ForEach(o => WorkTypes.Add(o));
            if (LoadWorkTypesComplete != null)
                LoadWorkTypesComplete(this, new EventArgs());
        }

        public void SetWorksList(List<WorksInfoDataModel> data)
        {
            data = data.OrderBy(p => p.Name).ToList();
            WorkList.Clear();
            data.ForEach(o => WorkList.Add(o));
            if (LoadWorkListComplete != null)
                LoadWorkListComplete(this, new EventArgs());
        }

        public void SetCarWorks(List<WorksInfoExemplarDataModel> data)
        {
            data = data.OrderBy(p => p.Name).ToList();
            CarWorkList.Clear();
            data.ForEach(o => CarWorkList.Add(o));
        }

        public void SetCarStatInfo(CarStatInfoModel model)
        {
            CarProtocolType = model.InterfaceType;
            PIDS.Clear();
            foreach (var item in model.PIDs)
            {
                PIDS.Add(item);
            }
            if (LoadCarStatComplete != null)
                LoadCarStatComplete(this, new EventArgs());
        }

        public void SetOtherCarWorks(List<WorksInfoDataModel> data)
        {
            data = data.OrderBy(p => p.Name).ToList();
            OtherWorkList.Clear();
            data.ForEach(o => OtherWorkList.Add(o));
            if (LoadOtherWorkListComplete != null)
                LoadOtherWorkListComplete(this, new EventArgs());
        }

        public void SetPartsWorks(List<WorksInfoDataModel> data)
        {
            data = data.OrderBy(p => p.Name).ToList();
            PartsWorkList.Clear();
            data.ForEach(o => PartsWorkList.Add(o));
            if (PartsWorksLoadComplete != null)
                PartsWorksLoadComplete(this, new EventArgs());
        }

        public void SetWorkPartsList(List<WorksInfoDataModel> data)
        {
            if (LoadWorkPartsListComplete != null)
                LoadWorkPartsListComplete(data);
        }




        public void AddNewWorkName(string name, int typeID, bool periodic)
        {
                CarBaseHandler.Instance.AddWorkNameEvent(typeID, name, periodic);
        }

        public void AddWorkToCar(int model, int transmission, int body, int engine, int engineType, string WorkName, int periodic, int distance)
        {
            AddWorkTocarModel req = new AddWorkTocarModel
            {
                Body = body,
                Distance = distance,
                Engine = engine,
                EngineType = engineType,
                Model = model,
                Periodic = periodic,
                Transmission = transmission,
                WorkName = WorkName
            };
                CarBaseHandler.Instance.AddWorkEvent(req);
        }



        public void GetEnginesDataByModel(KVPBase model)
        {
            if (model!=null)
                CarBaseHandler.Instance.GetEnginesByModel(model.id, 0);
        }

        public void GetCarInfo(int idModel, int idEngine)
        {
            CarBaseHandler.Instance.GetCarStatInfo(idModel, idEngine);
        }
    }
}
