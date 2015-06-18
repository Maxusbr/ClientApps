using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using DTCDev.Client.Cars.Service.Engine.Controls.ViewModels.Settings;
using DTCDev.Client.Cars.Service.Engine.Handlers;
using DTCDev.Client.ViewModel;
using DTCDev.Models.CarsSending.Car;

namespace DTCDev.Client.Cars.Service.Engine.Controls.ViewModels
{
    public class CarTestDrivesViewModel : ViewModelBase, IDateList
    {
        private readonly TestDriveCarViewModel _car;
        private readonly ObservableCollection<TestDriveCarViewModel> _testDrives = new ObservableCollection<TestDriveCarViewModel>();
        private int _selectedOrder = -1;
        private readonly List<DateListModel> _datesList = new List<DateListModel>();
        public bool WeekStyle { get { return PostsHandler.Instance.WeekStyle; } }
        public DateTime Date { get { return PostsHandler.Instance.Date; } }

        public CarTestDrivesViewModel(TestDriveCarViewModel car)
        {
            _car = car;
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {

            }
        }

        public TestDriveCarViewModel Car { get { return _car; } }

        public ObservableCollection<TestDriveCarViewModel> TestDrives { get { return _testDrives; } }

        public int SelectedOrder
        {
            get { return _selectedOrder; }
            set { _selectedOrder = value; }
        }

        public void Update(TestDriveCarViewModel tst)
        {
            var test = TestDrives.FirstOrDefault(o => o.ID == tst.ID);
            if (test == null)
            {
                //ord.IsCompleteSaved += Order_IsCompleteSaved;
                TestDrives.Add(tst);
            }
            else
            {
                test.Update(tst.Model);
            }
            UpdateDatesList();
        }

        private void UpdateDatesList()
        {
            _datesList.Clear();
            foreach (var el in TestDrives)
            {
                _datesList.Add(new DateListModel
                {
                    DateWork = el.DateWork,
                    ToolTip = string.Format("{0} - {1}",
                        el.User != null ? el.User.Name : "User",
                        el.CurrentCar != null ? el.CurrentCar.ToString() : "")
                });
            }
        }

        public int StartWorkTime { get { return Car.StartWorkTime; } }

        public int EndWorkTime { get { return Car.EndWorkTime; } }

        public List<DateListModel> DatesList
        {
            get { return _datesList; }
        }

        internal void Update(TestDriveModel test)
        {
            throw new NotImplementedException();
        }

        internal void Update(Models.CarBase.CarList.CarListBaseDataModel carListBaseDataModel)
        {
            throw new NotImplementedException();
        }
    }
}
