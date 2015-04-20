using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using DTCDev.Models.CarBase.CarStatData;

namespace DTCDev.Client.Cars.Manager.Data
{
    public class SpecificationDataStorage
    {
        private static SpecificationDataStorage _instance;
        public static SpecificationDataStorage Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new SpecificationDataStorage();
                return _instance;
            }
        }

        public SpecificationDataStorage()
        {
            _instance = this;
        }

        private ObservableCollection<KVPBase> _marks = new ObservableCollection<KVPBase>();
        private ObservableCollection<KVPBase> _models = new ObservableCollection<KVPBase>();
        private ObservableCollection<KVPBase> _engineTypes = new ObservableCollection<KVPBase>();
        private ObservableCollection<KVPBase> _engineVolumes = new ObservableCollection<KVPBase>();
        private ObservableCollection<KVPBase> _bodyTypes = new ObservableCollection<KVPBase>();
        private ObservableCollection<KVPBase> _transTypes = new ObservableCollection<KVPBase>();

        public ObservableCollection<KVPBase> Marks { get { return _marks; } }
        public ObservableCollection<KVPBase> Models { get { return _models; } }
        public ObservableCollection<KVPBase> EngineTypes { get { return _engineTypes; } }
        public ObservableCollection<KVPBase> EngineVolumes { get { return _engineVolumes; } }
        public ObservableCollection<KVPBase> BodyTypes { get { return _bodyTypes; } }
        public ObservableCollection<KVPBase> TransTypes { get { return _transTypes; } }


        public delegate void GetInfoHandler(int idPearent);
        public delegate void GetSubInfoHandler(int idModel, int idPearent);
        public event EventHandler GetMarks;
        public event GetInfoHandler GetModels;
        public event GetInfoHandler GetBodies;
        public event GetSubInfoHandler GetEngines;
        public event GetSubInfoHandler GetEngineTypes;
        public event GetSubInfoHandler GetTransTypes;

        private KVPBase _selectedMark;
        private KVPBase _selectedModel;
        private KVPBase _selectedEngine;
        private KVPBase _selectedEngineType;
        private KVPBase _selectedBody;

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
            set { _selectedEngine = value; }
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

        public void Update()
        {
            CallLoadMarks();
        }


        public void SetMarks(List<KVPBase> data)
        {
            data = data.OrderBy(p => p.Name).ToList();
            Marks.Clear();
            foreach (var item in data)
            {
                Marks.Add(item);
            }
        }

        public void SetModels(List<KVPBase> data)
        {
            data = data.OrderBy(p => p.Name).ToList();
            Models.Clear();
            foreach (var item in data)
            {
                Models.Add(item);
            }
        }

        public void SetEngineTypes(List<KVPBase> data)
        {
            EngineTypes.Clear();
            foreach (var item in data)
            {
                EngineTypes.Add(item);
            }
        }

        public void SetEngineVolumes(List<KVPBase> data)
        {
            data = data.OrderBy(p => p.Name).ToList();
            EngineVolumes.Clear();
            foreach (var item in data)
            {
                EngineVolumes.Add(item);
            }
        }

        public void SetBodyTypes(List<KVPBase> data)
        {
            data = data.OrderBy(p => p.Name).ToList();
            BodyTypes.Clear();
            foreach (var item in data)
            {
                BodyTypes.Add(item);
            }
        }

        public void SetTransTypes(List<KVPBase> data)
        {
            data = data.OrderBy(p => p.Name).ToList();
            TransTypes.Clear();
            foreach (var item in data)
            {
                TransTypes.Add(item);
            }
        }
    }
}