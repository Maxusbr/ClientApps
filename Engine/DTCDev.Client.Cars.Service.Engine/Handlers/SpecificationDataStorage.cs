﻿using System;
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



        public delegate void GetInfoHandler(int idPearent);
        public delegate void GetSubInfoHandler(int idModel, int idPearent);
        public event EventHandler GetMarks;
        public event GetInfoHandler GetModels;
        public event GetInfoHandler GetBodies;
        public event GetSubInfoHandler GetEngines;
        public event GetSubInfoHandler GetEnginesByModel;
        public event GetSubInfoHandler GetEngineTypes;
        public event GetSubInfoHandler GetTransTypes;
        public event GetSubInfoHandler GetCarStatInfo;

        public delegate void GetCarWorksHandler(int idModel, int idEngine, int idBody, int idEngineType, int idTransmission);
        public event GetCarWorksHandler GetCarWorksEvent;

        public event EventHandler GetWorkTypes;
        public event EventHandler GetWorksList;

        public delegate void AddWorkNameHandler(int idType, string name);
        public event AddWorkNameHandler AddWorkNameEvent;

        public delegate void AddWorkToCarHandler(AddWorkTocarModel model);
        public event AddWorkToCarHandler AddWorkEvent;

        public event EventHandler LoadCarStatComplete;
        public event EventHandler LoadWorkListComplete;
        public event EventHandler LoadOtherWorkListComplete;
        public event EventHandler PartsWorksLoadComplete;

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
            if (GetMarks != null)
                GetMarks(this, new EventArgs());
        }

        private void CallLoadModels()
        {
            if (GetModels != null && SelectedMark != null)
                GetModels(SelectedMark.id);
        }

        private void CallLoadEngines()
        {
            if (GetEngines != null && SelectedModel != null && SelectedEngineType != null)
                GetEngines(SelectedModel.id, SelectedEngineType.id);
        }

        private void CallLoadBodies()
        {
            if (GetBodies != null && SelectedModel != null)
                GetBodies(_selectedModel.id);
        }

        private void CallLoadEngineTypes()
        {
            if (GetEngineTypes != null && SelectedModel != null && SelectedBody != null)
                GetEngineTypes(SelectedModel.id, SelectedBody.id);
        }

        private void CallLoadTrans()
        {
            if (GetTransTypes != null && SelectedModel != null && SelectedEngineVolume != null)
                GetTransTypes(SelectedModel.id, SelectedEngineVolume.id);
        }

        private void CallLoadCarWorks()
        {
            if (GetCarWorksEvent != null)
                GetCarWorksEvent(_selectedModel.id, _selectedEngine.id, _selectedBody.id, SelectedEngineType.id, SelectedTransmission.id);
        }

        public void Update()
        {
            if (_marks.Count() < 1)
                CallLoadMarks();
        }

        public void UpdateWorks()
        {
            if (GetWorksList != null)
                GetWorksList(this, new EventArgs());
        }

        public void UpdateOtherWorks()
        {
            CarBaseHandler.Instance.UpdateOtherWorks();
        }

        public void AddOtherWorkName(string name, int typeID)
        {
            CarBaseHandler.Instance.AddOtherWorkName(name, typeID);
        }

        public void UpdateWorkTypes()
        {
            if (GetWorkTypes != null)
                GetWorkTypes(this, new EventArgs());
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
        }

        public void SetModels(List<KVPBase> data)
        {
            data = data.OrderBy(p => p.Name).ToList();
            Models.Clear();
            data.ForEach(o => Models.Add(o));
        }

        public void SetEngineTypes(List<KVPBase> data)
        {
            data = data.OrderBy(p => p.Name).ToList();
            EngineTypes.Clear();
            data.ForEach(o => EngineTypes.Add(o));
        }

        public void SetEngineVolumes(List<KVPBase> data)
        {
            data = data.OrderBy(p => p.Name).ToList();
            EngineVolumes.Clear();
            data.ForEach(o => EngineVolumes.Add(o));
        }

        public void SetBodyTypes(List<KVPBase> data)
        {
            data = data.OrderBy(p => p.Name).ToList();
            BodyTypes.Clear();
            data.ForEach(o => BodyTypes.Add(o));
        }

        public void SetTransTypes(List<KVPBase> data)
        {
            data = data.OrderBy(p => p.Name).ToList();
            TransTypes.Clear();
            data.ForEach(o => TransTypes.Add(o));
        }

        public void SetWorkTypes(List<KVPBase> data)
        {
            data = data.OrderBy(p => p.Name).ToList();
            WorkTypes.Clear();
            data.ForEach(o => WorkTypes.Add(o));
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




        public void AddNewWorkName(string name, int typeID)
        {
            if (AddWorkNameEvent != null)
                AddWorkNameEvent(typeID, name);
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
            if (AddWorkEvent != null)
                AddWorkEvent(req);
        }



        public void GetEnginesDataByModel(KVPBase model)
        {
            if (GetEnginesByModel != null && model!=null)
                GetEnginesByModel(model.id, 0);
        }

        public void GetCarInfo(int idModel, int idEngine)
        {
            if (GetCarStatInfo != null)
                GetCarStatInfo(idModel, idEngine);
        }
    }
}
