using DTCDev.Client.Cars.Service.Engine.Handlers;
using DTCDev.Client.Cars.Service.Engine.Storage;
using DTCDev.Client.ViewModel;
using DTCDev.Models.CarBase.CarStatData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace DTCDev.Client.Cars.Service.Engine.Controls.ViewModels.Cars
{
    public class CarSettingsViewModel : ViewModelBase
    {
        public CarSettingsViewModel()
        {
            //Если марки не загружены - загружаем марки и только потом настройки
            //так как по факту завершения загрузки настроек начинается процесс выставления текущих значений
            if (SpecificationDataStorage.Instance.Marks.Count() < 1)
            {
                SpecificationDataStorage.Instance.Update();
                SpecificationDataStorage.Instance.Marks.CollectionChanged += Marks_CollectionChanged;
            }
            else
                LoadCarSettings();
        }

        void Marks_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            SpecificationDataStorage.Instance.Marks.CollectionChanged -= Marks_CollectionChanged;
            LoadCarSettings();
        }

        private void LoadCarSettings()
        {
            CarStorage.Instance.GetCarSettings();
            CarStorage.Instance.LoadCurrentCarSettingsComplete += Instance_LoadCurrentCarSettingsComplete;
        }

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
                this.OnPropertyChanged("Mark");
            }
        }

        public KVPBase Model
        {
            get { return SpecificationDataStorage.Instance.SelectedModel; }
            set
            {
                if (SpecificationDataStorage.Instance.SelectedModel == value) return;
                SpecificationDataStorage.Instance.SelectedModel = value;
                this.OnPropertyChanged("Model");
            }
        }

        public KVPBase Body
        {
            get { return SpecificationDataStorage.Instance.SelectedBody; }
            set
            {
                if (SpecificationDataStorage.Instance.SelectedBody == value) return;
                SpecificationDataStorage.Instance.SelectedBody = value;
                this.OnPropertyChanged("Body");
            }
        }

        public KVPBase EngineType
        {
            get { return SpecificationDataStorage.Instance.SelectedEngineType; }
            set
            {
                if (SpecificationDataStorage.Instance.SelectedEngineType == value) return;
                SpecificationDataStorage.Instance.SelectedEngineType = value;
                this.OnPropertyChanged("EngineType");
            }
        }

        public KVPBase EngineVolume
        {
            get { return SpecificationDataStorage.Instance.SelectedEngineVolume; }
            set
            {
                if (SpecificationDataStorage.Instance.SelectedEngineVolume == value) return;
                SpecificationDataStorage.Instance.SelectedEngineVolume = value;
                this.OnPropertyChanged("EngineVolume");
            }
        }
        private KVPBase _transmissionType;

        public KVPBase TransmissionType
        {
            get { return _transmissionType; }
            set
            {
                if (_transmissionType == value) return;
                _transmissionType = value;
                this.OnPropertyChanged("TransmissionType");
            }
        }


        void Instance_LoadCurrentCarSettingsComplete(object sender, EventArgs e)
        {
            Mark = Marks.Where(p => p.id == CarStorage.Instance.CarSettingsExemplar.IDMark).FirstOrDefault();
            SpecificationDataStorage.Instance.LoadModelsComplete += Instance_LoadModelsComplete;
        }

        void Instance_LoadModelsComplete(object sender, EventArgs e)
        {
            SpecificationDataStorage.Instance.LoadModelsComplete -= Instance_LoadModelsComplete;
            Model = Models.Where(p => p.id == CarStorage.Instance.CarSettingsExemplar.IDModel).FirstOrDefault();
            SpecificationDataStorage.Instance.LoadBodiesComplete += Instance_LoadBodiesComplete;
        }

        void Instance_LoadBodiesComplete(object sender, EventArgs e)
        {
            SpecificationDataStorage.Instance.LoadBodiesComplete -= Instance_LoadBodiesComplete;
            Body = Bodies.Where(p => p.id == CarStorage.Instance.CarSettingsExemplar.IDBody).FirstOrDefault();
            SpecificationDataStorage.Instance.LoadEngineTypesComplete += Instance_LoadEngineTypesComplete;
        }

        void Instance_LoadEngineTypesComplete(object sender, EventArgs e)
        {
            SpecificationDataStorage.Instance.LoadEngineTypesComplete -= Instance_LoadEngineTypesComplete;
            EngineType = EngineTypes.Where(p => p.id == CarStorage.Instance.CarSettingsExemplar.IDEngineType).FirstOrDefault();
            SpecificationDataStorage.Instance.LoadEnginsComplete += Instance_LoadEnginsComplete;
        }

        void Instance_LoadEnginsComplete(object sender, EventArgs e)
        {
            SpecificationDataStorage.Instance.LoadEnginsComplete -= Instance_LoadEnginsComplete;
            EngineVolume = EngineVolumes.Where(p => p.id == CarStorage.Instance.CarSettingsExemplar.IDEngine).FirstOrDefault();
            SpecificationDataStorage.Instance.LoadTransmissionsComplete += Instance_LoadTransmissionsComplete;
        }

        void Instance_LoadTransmissionsComplete(object sender, EventArgs e)
        {
            SpecificationDataStorage.Instance.LoadTransmissionsComplete -= Instance_LoadTransmissionsComplete;
            TransmissionType = TransmissionTypes.Where(p => p.id == CarStorage.Instance.CarSettingsExemplar.IDTransmission).FirstOrDefault();
        }
    }
}
