using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DTCDev.Client.Cars.Service.Engine.Controls.ViewModels;
using DTCDev.Client.Cars.Service.Engine.Controls.ViewModels.Settings;

namespace DTCDev.Client.Cars.Service.Controls.CalendarControls
{
    /// <summary>
    /// Логика взаимодействия для DayViewControl.xaml
    /// </summary>
    public partial class TestDriveDayWeekViewControl : UserControl
    {
        readonly TestDriveDayWeekViewModel _vm;
        private CarTestDrivesViewModel _selectedCar;
        public TestDriveDayWeekViewControl()
        {
            InitializeComponent();
            _vm = DataContext as TestDriveDayWeekViewModel;
            PostList.SelectedCellsChanged += PostList_SelectedCellsChanged;
            if (_vm != null) _vm.PropertyChanged += _vm_PropertyChanged;
            UpdateUI();
        }

        private void _vm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("WeekStyle") || e.PropertyName.Equals("Date")) UpdateUI();
        }

        private void UpdateUI()
        {
            if (_vm.WeekStyle)
                UpdateUIWeek();
            else
                UpdateUIDay();
        }

        private void UpdateUIWeek()
        {
            PostList.Columns.Clear();
            PostList.Columns.Add(new DataGridTextColumn { Header = "Автомобили", Binding = new Binding("Car.CurrentCar") });
            for (var i = 0; i < 7; i++)
            {
                var column = new DataGridTemplateColumn
                {
                    Header = (_vm.Date + new TimeSpan(i, 0, 0, 0)).ToShortDateString(),
                    CellStyle = Resources["StyleCell"] as Style
                };
                PostList.Columns.Add(column);
            }
        }

        private void UpdateUIDay()
        {
            PostList.Columns.Clear();
            PostList.Columns.Add(new DataGridTextColumn { Header = "Автомобили", Binding = new Binding("Car.CurrentCar") });
            for (var i = _vm.StartTime; i < _vm.EndTime; i++)
            {
                AddColumn(i, "{0}:00");
                AddColumn(i, "{0}:30");
            }
            AddColumn(_vm.EndTime, "{0}:00");
        }

        private void AddColumn(int indx, string name)
        {
            var column = new DataGridTemplateColumn
            {
                Header = string.Format(name, indx),
                CellStyle = Resources["StyleCell"] as Style
            };
            PostList.Columns.Add(column);
        }

        private void PostList_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (e.AddedCells == null || e.AddedCells.Count == 0) return;
            _selectedCar = e.AddedCells[0].Item as CarTestDrivesViewModel;
            if (_selectedCar == null) return;
            if (_vm.WeekStyle)
                ClickDay(e.AddedCells[0]);
            else
                ClickHour(e.AddedCells[0]);
        }

        private void ClickDay(DataGridCellInfo cell)
        {
            _vm.Date = DateTime.Parse(cell.Column.Header.ToString());
            _vm.WeekStyle = false;
            UpdateUI();
        }

        private void ClickHour(DataGridCellInfo cell)
        {
            if (cell.Column.DisplayIndex == 0 || _selectedCar == null) return;
            TimeSpan ts;
            if (!TimeSpan.TryParse(cell.Column.Header.ToString(), out ts)) return;
            var dateWork = _selectedCar.Date + ts;
            var td =
                _selectedCar.TestDrives.FirstOrDefault(
                    el => el.DateWork >= dateWork && el.DateWork < dateWork + new TimeSpan(0, 30, 0));
            var testdrive = td != null ? new TestDriveCarViewModel(td) :
                      new TestDriveCarViewModel(dateWork, _selectedCar.Car.CurrentCar, _selectedCar.TestDrives.Count, false);
            testdrive.IsCompleteSaved += TestDriveIsCompleteSaved;
            var detail = new TestDriveView() { DataContext = testdrive };
            ccDetail.Children.Clear();
            ccDetail.Children.Add(detail);
            tbCar.Text = _selectedCar.Car.CurrentCar.ToString();
            gDetail.Visibility = Visibility.Visible;
        }

        private void TestDriveIsCompleteSaved(object sender, EventArgs eventArgs)
        {
            var testdrive = sender as TestDriveCarViewModel;
            if (testdrive == null) return;
            _vm.Save(testdrive);
            _selectedCar.Update(testdrive);
            gDetail.Visibility = Visibility.Collapsed;
            UpdateUIDay();
        }

        private void imgLeft_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _vm.Date = _vm.WeekStyle ? _vm.Date.AddDays(-7) : _vm.Date.AddDays(-1);
        }

        private void imgRight_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _vm.Date = _vm.WeekStyle ? _vm.Date.AddDays(7) : _vm.Date.AddDays(1);
        }
    }



}
