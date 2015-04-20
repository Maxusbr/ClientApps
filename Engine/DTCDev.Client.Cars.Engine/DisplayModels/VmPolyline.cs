using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DTCDev.Client.Controls.Map;

namespace DTCDev.Client.Cars.Engine.DisplayModels
{
    public class VmPolyline : System.ComponentModel.INotifyPropertyChanged
    {
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

        public VmPolyline() { }
        public VmPolyline(int count) {
            this.Name = string.Format("Zone - {0}", count);
        }
        public VmPolyline(string name) {
            this.Name = name;
        }
        public VmPolyline(int id, string name, string coordinate) {
            this.ID = id;
            this.Name = name;
            if (string.IsNullOrEmpty(coordinate)) return;
            MovedLocationCollection coord = MovedLocationCollection.Parse(coordinate);
            if (coord.Count <= 0) return;
            foreach (var el in coord)
                AddLocation(el, false);
        }

        public LocationCollection Locations { get; set; }

        private MovedLocationCollection movedlocations = new MovedLocationCollection();
        public MovedLocationCollection MovedLocations { get { return movedlocations; } }

        public void AddLocation(Location point, bool useradded)
        {
            MovedLocation el = new MovedLocation(point);
            el.PropertyChanged += Location_PropertyChanged;
            el.EnableEdit = true;
            MovedLocations.Add(el);
            if(MovedLocations.Count > 1)
                Locations = new LocationCollection(MovedLocations);
            OnUpdate(useradded);
        }



        void Location_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            MovedLocation el = sender as MovedLocation;
            if (e.PropertyName.Equals("Current"))
            {
                Locations = new LocationCollection(MovedLocations);
                OnUpdate(true);
            }
            if (e.PropertyName.Equals("Remove"))
            {
                if (el != null)
                {
                    MovedLocations.Remove(el);
                    Locations = new LocationCollection(MovedLocations);
                    OnUpdate(true);
                }
            }
        }

        public void ClearLocation()
        {
            foreach (MovedLocation el in MovedLocations)
                el.PropertyChanged -= Location_PropertyChanged;
            if (MovedLocations != null)
                MovedLocations.Clear();
            if(Locations != null)
                Locations.Clear();
        }

        private void OnUpdate(bool ischaged)
        {
            OnPropertyChanged("Locations");
            IsChanged = ischaged;
        }

        private void ChangeEnabled()
        {
            foreach (var el in MovedLocations)
                el.EnableEdit = this.enableEdit;
        }

        bool _isChanged = false;
        public bool IsChanged
        {
            get { return _isChanged; }
            set
            {
                if (this._isChanged != value)
                {
                    this._isChanged = this._isChanged || value;
                    OnPropertyChanged("IsChanged");
                }
            }
        }

        public int ID { get; set; }

        private string name = string.Empty;
        public string Name
        {
            get { return name; }
            set
            {
                if (this.name != value)
                {
                    this.name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        private bool enableEdit = false;
        public bool EnableEdit
        {
            get { return enableEdit; }
            set
            {
                if (this.enableEdit != value)
                {
                    this.enableEdit = value;
                    OnPropertyChanged("EnableEdit");
                }
                ChangeEnabled();
            }
        }

        private bool _isSelected = false;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (this._isSelected != value)
                {
                    this._isSelected = value;
                    OnPropertyChanged("IsSelected");
                }
            }
        }

        public override string ToString()
        {
            return this.Name;
        }

        public bool IsPublic { get; set; }
    }
}
