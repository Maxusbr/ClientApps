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
    public class ZoneMapViewModel : ViewModelBase
    {
        public ZoneMapViewModel()
        {
            MapCenter = MapCenterUser = new Location(55.75, 37.62);
            _zoneHandler.LoadCompleted += zoneHandler_LoadCompleted;
            _zoneHandler.Update();
            if (Zones.Count != 0) return;
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {
                ObservableCollection<Location> Points = new AsyncObservableCollection<Location>();
                Points.Add(new Location(55.758, 37.76));
                Points.Add(new Location(55.75, 37.5));
                Points.Add(new Location(55.8, 37.625));
                var z = new VmPolyline(Zones.Count);
                z.PropertyChanged += Zone_PropertyChanged;
                Zones.Add(z);
                Zones[0].AddLocation(Points[0], false);
                Zones[0].AddLocation(Points[1], false);
                Zones[0].AddLocation(Points[2], false);
            }
        }

        private void Zone_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != null && e.PropertyName == "IsChanged")
            {
                var cntrl = sender as VmPolyline;
                if (cntrl != null) IsEnabledSaveBtn = _isEnabledSaveBtn || cntrl.IsChanged;
            }
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

        private Location currentLocation;
        public Location CurrentLocation
        {
            get { return currentLocation; }
            set
            {
                if (currentLocation != value)
                {
                    currentLocation = value;
                    OnPropertyChanged("CurrentLocation");
                }
            }
        }

        public bool IsSelectedZone
        {
            get { return SelectedZone != null && !SelectedZone.IsPublic; }
        }

        private bool _isEnabledSaveBtn = false;
        public bool IsEnabledSaveBtn
        {
            get { return _isEnabledSaveBtn; }
            set
            {
                if (this._isEnabledSaveBtn != value)
                {
                    this._isEnabledSaveBtn = value;
                    OnPropertyChanged("IsEnabledSaveBtn");
                }
            }
        }

        private bool _enableEditZone = false;
        public bool EnableEditZone
        {
            get { return _enableEditZone; }
            set
            {
                if (this._enableEditZone != value)
                {
                    this._enableEditZone = value;
                    OnPropertyChanged("EnableEditZone");
                }
            }
        }

        readonly ZonesHandler _zoneHandler = ZonesHandler.Instance;
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
                    if (this._selectedZone != null)
                        ChangeEnableEdit(this._selectedZone, false);
                    this._selectedZone = value;
                    if (this._selectedZone != null)
                        ChangeEnableEdit(this._selectedZone, true);
                    OnPropertyChanged("SelectedZone");
                    OnPropertyChanged("IsSelectedZone");
                }
            }
        }

        private void ChangeEnableEdit(VmPolyline obj, bool Enable)
        {
            int indx = Zones.IndexOf(obj);
            if (indx >= 0 && indx < Zones.Count)
                Zones[indx].EnableEdit = Enable;
            if (obj.MovedLocations != null && obj.MovedLocations.Count >= 0 && Enable)
            {
                var centerLocation = obj.MovedLocations.GetCenter();
                if (centerLocation.Latitude != 0 && centerLocation.Longitude != 0)
                    MapCenterUser = centerLocation;
            }
            EnableEditZone = !obj.IsPublic;
        }

        void zoneHandler_LoadCompleted(string ResultOperation)
        {
            switch (ResultOperation)
            {
                case "AddComplete":
                    break;
                case "UpdateComplete":
                    break;
                case "DeleteComplete":
                    break;
                case "GetComplete":
                    OnPropertyChanged("Zones");
                    /*if (Zones.Count > 0)
                        SelectedZone = Zones[0];*/
                    break;
                case "Error":
                    break;
            }
        }

        private ICommand _leftMouseButtonClickCommand;
        public ICommand LeftMouseButtonClickCommand
        {
            get
            {
                if (_leftMouseButtonClickCommand == null)
                {
                    _leftMouseButtonClickCommand = new RelayCommand(LeftMouseButtonClick);
                }
                return _leftMouseButtonClickCommand;
            }
        }

        private void LeftMouseButtonClick(object obj)
        {
            if (SelectedZone != null && EnableEditZone)
            {
                MouseButtonEventArgs e = obj as MouseButtonEventArgs;
                var sender = e.Source as Client.Controls.Map.Map;
                if (sender != null)
                {
                    System.Windows.Point mousePosition = e.GetPosition(sender);
                    CurrentLocation = sender.ViewportPointToLocation(mousePosition);
                    SelectedZone.AddLocation(CurrentLocation, true);
                }
            }
        }

        private ICommand _addZoneCommand;
        public ICommand AddZoneCommand
        {
            get
            {
                if (_addZoneCommand == null)
                {
                    _addZoneCommand = new RelayCommand(AddZone);
                }
                return _addZoneCommand;
            }
        }

        private void AddZone(object obj)
        {
            Zones.Add(new VmPolyline(Zones.Count){IsChanged = true});
            IsEnabledSaveBtn = true;
        }

        private ICommand _RemoveZoneCommand;
        public ICommand RemoveZoneCommand
        {
            get
            {
                if (_RemoveZoneCommand == null)
                {
                    _RemoveZoneCommand = new RelayCommand(RemoveZone);
                }
                return _RemoveZoneCommand;
            }
        }

        private void RemoveZone(object obj)
        {
            if (SelectedZone != null && Zones.IndexOf(SelectedZone) >= 0)
            {
                _zoneHandler.DeleteZone(SelectedZone);
                Zones.Remove(SelectedZone);
                IsEnabledSaveBtn = true;
            }
        }

        private ICommand _clearCurentZoneCommand;
        public ICommand ClearCurentZoneCommand
        {
            get
            {
                if (_clearCurentZoneCommand == null)
                {
                    _clearCurentZoneCommand = new RelayCommand(ClearCurentZone);
                }
                return _clearCurentZoneCommand;
            }
        }

        private void ClearCurentZone(object obj)
        {
            if (SelectedZone != null)
            {
                SelectedZone.ClearLocation();
            }
        }

        private ICommand _saveZoneCommand;
        public ICommand SaveZoneCommand
        {
            get
            {
                if (_saveZoneCommand == null)
                {
                    _saveZoneCommand = new RelayCommand(this.SaveZone);
                }
                return _saveZoneCommand;
            }
        }

        private void SaveZone(object obj)
        {
            foreach (var el in Zones.Where(w => w.IsChanged))
            {
                if (el.ID != 0)
                    _zoneHandler.EditZone(el);
                else
                    _zoneHandler.AddZone(el);
                IsEnabledSaveBtn = false;
            }
            Zones.Where(w => w.IsChanged).ToList().ForEach(x => x.IsChanged = false);
        }

        private ICommand _editCurentZoneCommand;
        public ICommand EditCurentZoneCommand
        {
            get
            {
                if (_editCurentZoneCommand == null)
                {
                    _editCurentZoneCommand = new RelayCommand(StartEditCurentZone);
                }
                return _editCurentZoneCommand;
            }
        }

        private void StartEditCurentZone(object obj)
        {
            if (SelectedZone != null)
            {
                ChangeEnableEdit(SelectedZone, true);
                
            }
            
        }
    }
}
