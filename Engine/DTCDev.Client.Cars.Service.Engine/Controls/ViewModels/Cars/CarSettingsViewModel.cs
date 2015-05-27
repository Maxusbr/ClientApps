using DTCDev.Client.Cars.Service.Engine.Handlers;
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

        }

        public ObservableCollection<KVPBase> Marks { get { return SpecificationDataStorage.Instance.Marks; } }
        public ObservableCollection<KVPBase> Models { get { return SpecificationDataStorage.Instance.Models; } }
        public ObservableCollection<KVPBase> EngineTypes { get { return SpecificationDataStorage.Instance.EngineTypes; } }
        public ObservableCollection<KVPBase> EngineVolumes { get { return SpecificationDataStorage.Instance.EngineVolumes; } }
        public ObservableCollection<KVPBase> TransmissionTypes { get { return SpecificationDataStorage.Instance.TransTypes; } }
        public ObservableCollection<KVPBase> Bodies { get { return SpecificationDataStorage.Instance.BodyTypes; } }
    }
}
