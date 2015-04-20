using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using DTCDev.Client.Cars.Engine.DisplayModels;
using DTCDev.Client.Cars.Engine.Handlers.Cars;
using DTCDev.Client.Controls.Map;
using DTCDev.Client.ViewModel;

namespace DTCDev.Client.Cars.Controls.ViewModels.Map
{
    public class CarZoneMapViewModel : ViewModelBase
    {
        public CarZoneMapViewModel()
        {
            MapCenter = MapCenterUser = new Location(55.75, 37.62);
            _zoneHandler.LoadCompleted += zoneHandler_LoadCompleted;
            _zoneHandler.Update();
            _carHandler.CarsRefreshed += carHandler_CarsRefreshed;
            _carHandler.Update();
            if (Zones.Count != 0) return;
            //ObservableCollection<Location> Points = new AsyncObservableCollection<Location>();
            //Points.Add(new Location(55.758, 37.76));
            //Points.Add(new Location(55.75, 37.5));
            //Points.Add(new Location(55.8, 37.625));
            //var z = new VmPolyline(Zones.Count);
            //z.AddLocation(Points[0], false);
            //z.AddLocation(Points[1], false);
            //z.AddLocation(Points[2], false);
            //Zones.Add(z);

        }

        void zoneHandler_LoadCompleted(string resultOperation)
        {
            switch (resultOperation)
            {
                case "AddComplete":
                    break;
                case "UpdateComplete":
                    break;
                case "DeleteComplete":
                    break;
                case "GetComplete":
                    OnPropertyChanged("Zones");
                    break;
                case "Error":
                    break;
            }
        }

        void carHandler_CarsRefreshed(object sender, EventArgs e)
        {
            
        }

        readonly ZonesHandler _zoneHandler = ZonesHandler.Instance;
        readonly CarsHandler _carHandler = CarsHandler.Instance;

        public ObservableCollection<VmPolyline> Zones
        {
            get { return _zoneHandler.Zones; }
        }

        private VmPolyline _selectedZone;
        public VmPolyline SelectedZone
        {
            get { return this._selectedZone; }
            set
            {
                if (this._selectedZone != value)
                {
                    this._selectedZone = value;
                    OnPropertyChanged("SelectedZone");
                    ChangeZoneSelected();
                }
            }
        }

        public ObservableCollection<DISP_Car> Cars
        {
            get { return _carHandler.Cars; }
        }

        DISP_Car _selectedCar;
        public DISP_Car SelectedCar
        {
            get
            {
                return _selectedCar;
            }
            set
            {
                if (this._selectedCar != value)
                {
                    this._selectedCar = value;
                    OnPropertyChanged("SelectedCar");
                    ChangeCarSelected();
                }
            }
        }

        private void ClearSelect()
        {
            Zones.ToList().ForEach(x => x.IsSelected = false);
            Cars.ToList().ForEach(x => x.IsSelected = false);
            OnPropertyChanged("IsCarZoneCanLink");
            OnPropertyChanged("IsCarZoneCanUnLink");
        }

        private void ChangeCarSelected()
        {
            ClearSelect();
            if (_selectedCar == null) return;
            var zone = Zones.FirstOrDefault(o => o.ID == _selectedCar.ZoneId);
            if (zone != null)
                zone.IsSelected = true;
        }

        private void ChangeZoneSelected()
        {
            ClearSelect();
            if (_selectedZone == null) return;
            Cars.Where(o => o.ZoneId == _selectedZone.ID).ToList().ForEach(x => x.IsSelected = true);
            MapCenterUser = _selectedZone.MovedLocations.GetCenter();
        }

        private Location _mapCenter;
        public Location MapCenter
        {
            get { return _mapCenter; }
            set
            {
                if (_mapCenter != value)
                {
                    _mapCenter = value;
                    OnPropertyChanged("MapCenter");
                }
            }
        }

        private Location _mapCenterUser;
        public Location MapCenterUser
        {
            get { return _mapCenterUser; }
            set
            {
                if (_mapCenterUser != value)
                {
                    _mapCenterUser = value;
                    OnPropertyChanged("MapCenterUser");
                }
            }
        }


        public bool IsCarZoneCanLink
        {
            get { return SelectedCar != null && SelectedZone != null; }
        }

        public bool IsCarZoneCanUnLink
        {
            get { return SelectedCar != null && SelectedCar.ZoneId != -1; }
        }

        public bool IsEnabledSaveBtn
        {
            get { return Cars.Any(o => o.IsChanged); }
        }

        private ICommand _linkCarToZoneCommand;
        public ICommand LinkCarToZoneCommand
        {
            get { return _linkCarToZoneCommand ?? (_linkCarToZoneCommand = new RelayCommand(LinkCarToZone)); }
        }

        private void LinkCarToZone(object obj)
        {
            if(SelectedZone == null || SelectedCar == null) return;
            SelectedCar.IsSelected = true;
            var indx = Cars.IndexOf(SelectedCar);
            Cars[indx].ZoneId = SelectedCar.ZoneId = SelectedZone.ID;
            Cars[indx].IsChanged = true;
            OnPropertyChanged("IsEnabledSaveBtn");
        }

        private ICommand _unLinCarToZoneCommand;
        public ICommand UnLinCarToZoneCommand
        {
            get { return _unLinCarToZoneCommand ?? (_unLinCarToZoneCommand = new RelayCommand(UnLinCarToZone)); }
        }

        private void UnLinCarToZone(object obj)
        {
            if(SelectedCar == null) return;
            SelectedCar.IsSelected = false;
            var indx = Cars.IndexOf(SelectedCar);
            Cars[indx].ZoneId = SelectedCar.ZoneId = -1;
            Cars[indx].IsChanged = true;
            OnPropertyChanged("IsEnabledSaveBtn");
        }

        private ICommand _saveCommand;
        public ICommand SaveCommand
        {
            get { return _saveCommand ?? (_saveCommand = new RelayCommand(SaveHandler)); }
        }

        private void SaveHandler(object obj)
        {
            //TODO Сохранить изменения
            Cars.Where(o => o.IsChanged).ToList().ForEach(x => x.IsChanged = false);
            OnPropertyChanged("IsEnabledSaveBtn");
        }
    }
}
