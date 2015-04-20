using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using DTCDev.Client.Cars.Manager.Data;
using DTCDev.Client.ViewModel;
using DTCDev.Models.CarBase.CarStatData;

namespace DTCDev.Client.Cars.Manager.ViewModels
{
    class CarSpecificatorViewModel : ViewModelBase
    {
        public CarSpecificatorViewModel()
        {
            if (SpecificationDataStorage.Instance.Marks.Count() < 1)
                SpecificationDataStorage.Instance.Update();
            CarDataStorage.Instance.Update();
        }

        public ObservableCollection<KVPBase> Marks { get { return SpecificationDataStorage.Instance.Marks; } }
        public ObservableCollection<KVPBase> Models { get { return SpecificationDataStorage.Instance.Models; } }
        public ObservableCollection<KVPBase> EngineTypes { get { return SpecificationDataStorage.Instance.EngineTypes; } }
        public ObservableCollection<KVPBase> EngineVolumes { get { return SpecificationDataStorage.Instance.EngineVolumes; } }
        public ObservableCollection<KVPBase> BodyTypes { get { return SpecificationDataStorage.Instance.BodyTypes; } }
        public ObservableCollection<KVPBase> TransTypes { get { return SpecificationDataStorage.Instance.TransTypes; } }
        public ObservableCollection<CarPresenterDataModel> Cars { get { return CarDataStorage.Instance.CarsList; } }
        public ObservableCollection<CurrentDevicesDataModel> Devices { get { return CarDataStorage.Instance.Devices; } }





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
                this.OnPropertyChanged("SelectedMark");
                SpecificationDataStorage.Instance.SelectedMark = value;
            }
        }

        public KVPBase SelectedModel
        {
            get { return _selectedModel; }
            set
            {
                _selectedModel = value;
                this.OnPropertyChanged("SelectedModel");
                SpecificationDataStorage.Instance.SelectedModel = value;
            }
        }

        public KVPBase SelectedBody
        {
            get { return _selectedBody; }
            set
            {
                _selectedBody = value;
                this.OnPropertyChanged("SelectedBody");
                SpecificationDataStorage.Instance.SelectedBody = value;
            }
        }

        public KVPBase SelectedEngineType
        {
            get { return _selectedEngineType; }
            set
            {
                _selectedEngineType = value;
                this.OnPropertyChanged("SelectedEngineType");
                SpecificationDataStorage.Instance.SelectedEngineType = value;
            }
        }

        public KVPBase SelectedEngineVolume
        {
            get { return _selectedEngine; }
            set
            {
                _selectedEngine = value;
                this.OnPropertyChanged("SelectedEngineVolume");
                SpecificationDataStorage.Instance.SelectedEngineVolume = value;
            }
        }

        public KVPBase SelectedTransmission
        {
            get { return _selectedTransmission; }
            set
            {
                _selectedTransmission = value;
                this.OnPropertyChanged("SelectedTransmission");
            }
        }


        private CarPresenterDataModel _selectedCar;

        public CarPresenterDataModel SelectedCar
        {
            get { return _selectedCar; }
            set
            {
                _selectedCar = value;
                this.OnPropertyChanged("SelectedCar");
            }
        }




        private void UpdateSelectedCarData()
        {

        }

    }
}
