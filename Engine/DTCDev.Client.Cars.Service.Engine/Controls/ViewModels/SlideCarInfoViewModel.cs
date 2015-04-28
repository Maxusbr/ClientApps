using DTCDev.Client.Cars.Service.Engine.Handlers;
using DTCDev.Client.ViewModel;
using DTCDev.Models.CarBase.CarStatData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace DTCDev.Client.Cars.Service.Engine.Controls.ViewModels
{
    public class SlideCarInfoViewModel : ViewModelBase
    {

        public SlideCarInfoViewModel()
        {
            SpecificationDataStorage.Instance.Update();
            SpecificationDataStorage.Instance.LoadCarStatComplete += Instance_LoadCarStatComplete;
        }

        void Instance_LoadCarStatComplete(object sender, EventArgs e)
        {
            ProtocolName = SpecificationDataStorage.Instance.CarProtocolType;
        }

        public ObservableCollection<KVPBase> Marks { get { return SpecificationDataStorage.Instance.Marks; } }
        public ObservableCollection<KVPBase> Models { get { return SpecificationDataStorage.Instance.Models; } }
        public ObservableCollection<KVPBase> Engins { get { return SpecificationDataStorage.Instance.EngineVolumes; } }
        public ObservableCollection<CarStatInfoModel.PIDInfo> PIDS { get { return SpecificationDataStorage.Instance.PIDS; } }

        public KVPBase Mark
        {
            get { return SpecificationDataStorage.Instance.SelectedMark; }
            set
            {
                if (SpecificationDataStorage.Instance.SelectedMark == value) return;
                SpecificationDataStorage.Instance.SelectedMark = value;
                OnPropertyChanged("Mark");
                PIDS.Clear();
                Model = null;
                Engine = null;
                Models.Clear();
                Engins.Clear();
                ProtocolName = "";
            }
        }

        KVPBase _model;

        public KVPBase Model
        {
            get { return _model; }
            set
            {
                if (SpecificationDataStorage.Instance.SelectedModel == value) return;
                _model = value;
                SpecificationDataStorage.Instance.GetEnginesDataByModel(value);
                OnPropertyChanged("Model");
                PIDS.Clear();
                Engine = null;
                Engins.Clear();
                ProtocolName = "";
            }
        }

        private KVPBase _engine;

        public KVPBase Engine
        {
            get { return _engine; }
            set
            {
                if (_engine == value) return;
                _engine = value;
                OnPropertyChanged("Engine");
                PIDS.Clear();
                ProtocolName = "";
                if (value != null && Model != null)
                    SpecificationDataStorage.Instance.GetCarInfo(Model.id, value.id);
            }
        }

        private string _protocolName;
        public string ProtocolName
        {
            get { return _protocolName; }
            set
            {
                _protocolName = value;
                this.OnPropertyChanged("ProtocolName");
            }
        }
    }
}
