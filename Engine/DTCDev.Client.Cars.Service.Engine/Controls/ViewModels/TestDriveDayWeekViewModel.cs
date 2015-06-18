using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using DTCDev.Client.Cars.Service.Engine.Controls.ViewModels.Settings;
using DTCDev.Client.Cars.Service.Engine.Handlers;
using DTCDev.Client.Cars.Service.Engine.Storage;
using DTCDev.Client.ViewModel;
using DTCDev.Models.CarBase.CarList;
using DTCDev.Models.User;

namespace DTCDev.Client.Cars.Service.Engine.Controls.ViewModels
{
    public class TestDriveDayWeekViewModel : ViewModelBase
    {
        private readonly TestDriveCarStorage _handler = TestDriveCarStorage.Instance;
        public TestDriveDayWeekViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {

            }
            AddCarTests();
        }

        private void AddCarTests()
        {
            foreach (var car in _handler.Cars)
            {
                var el = ListCarTest.FirstOrDefault(o => o.Car != null && o.Car.CarNumber == car.CarModel.DID);
                var added = el == null;
                if (added) el = new CarTestDrivesViewModel(new TestDriveCarViewModel(car.CarModel, ListCarTest.Count));
                foreach (var test in _handler.TestDrives.Where(o => o.Car.CarNumber == el.Car.CarNumber))
                {
                    el.Update(test);
                }
                if (added)
                    ListCarTest.Add(el);
                else
                    el.Update(car.CarModel);
            }
        }

        public DateTime Date
        {
            get { return _handler.Date; }
            set
            {
                if(_handler.Date == value) return;
                _handler.Date = value;
                OnPropertyChanged("Date");
                OnPropertyChanged("Period");
            }
        }

        public bool WeekStyle
        {
            get { return _handler.WeekStyle; }
            set
            {
                if (_handler.WeekStyle == value) return;
                _handler.WeekStyle = value;
                if(value)
                    UpdateDate();
                OnPropertyChanged("WeekStyle");
                OnPropertyChanged("Period");
                OnPropertyChanged("ToggleButtonName");
            }
        }

        private void UpdateDate()
        {
            _handler.Date = _handler.Date.AddDays(DayOfWeek.Monday - _handler.Date.DayOfWeek);
        }

        public string Period
        {
            get { return WeekStyle ? string.Format("{0} - {1}", Date.ToShortDateString(), 
                (Date + new TimeSpan(7,0,0,0)).ToShortDateString()): Date.ToShortDateString(); }
        }

        readonly ObservableCollection<CarTestDrivesViewModel> _listCarTest = new ObservableCollection<CarTestDrivesViewModel>();
        private CarTestDrivesViewModel _selectedItem;
        public ObservableCollection<CarTestDrivesViewModel> ListCarTest { get { return _listCarTest; } }

        public int StartTime
        {
            get
            {
                return ListCarTest.Count == 0 ? 8 : ListCarTest.Min(o => o.StartWorkTime);
            }
        }

        public int EndTime
        {
            get
            {
                return ListCarTest.Count == 0 ? 18 : ListCarTest.Max(o => o.EndWorkTime);
            }
        }

        public CarTestDrivesViewModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                OnPropertyChanged("SelectedItem");
                OnPropertyChanged("PostName");
            }
        }

        public string PostName
        {
            get { return SelectedItem != null && SelectedItem.Car != null ? SelectedItem.Car.Name : ""; }
        }

        public string ToggleButtonName
        {
            get { return WeekStyle ? "Неделя" : "День"; }
        }

        public void Save(TestDriveCarViewModel testdrive)
        {
            var car = ListCarTest.FirstOrDefault(o => o.Car.ID == testdrive.ID);
            if (car == null) return;
            var tst = car.TestDrives.FirstOrDefault(o => o.Equals(testdrive));
            testdrive.IsChanged = false;
            if (tst == null)
                car.TestDrives.Add(testdrive);
            else
                tst.Update(testdrive.Model);

            _handler.Save(testdrive.Model);
        }
    }
}
