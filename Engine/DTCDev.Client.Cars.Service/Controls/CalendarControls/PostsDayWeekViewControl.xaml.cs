﻿using System;
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

namespace DTCDev.Client.Cars.Service.Controls.CalendarControls
{
    /// <summary>
    /// Логика взаимодействия для DayViewControl.xaml
    /// </summary>
    public partial class PostsDayWeekViewControl : UserControl
    {
        readonly PostsDayWeekViewModel _vm;

        public PostsDayWeekViewControl()
        {
            InitializeComponent();
            _vm = DataContext as PostsDayWeekViewModel;
            PostList.SelectedCellsChanged += PostList_SelectedCellsChanged;
            _vm.PropertyChanged += _vm_PropertyChanged;
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
            PostList.Columns.Add(new DataGridTextColumn { Header = "Название поста", Binding = new Binding("Post.Name") });
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
            PostList.Columns.Add(new DataGridTextColumn { Header = "Название поста", Binding = new Binding("Post.Name") });
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
            var vm = e.AddedCells[0].Item as PostOrdersViewModel;
            if (vm == null) return;
            if (_vm.WeekStyle)
                ClickDay(e.AddedCells[0]);
            else
                ClickHour(vm, e.AddedCells[0]);
        }

        private void ClickDay(DataGridCellInfo cell)
        {
            _vm.Date = DateTime.Parse(cell.Column.Header.ToString());
            _vm.WeekStyle = false;
            UpdateUI();
        }

        private void ClickHour(PostOrdersViewModel vm, DataGridCellInfo cell)
        {
            var dateWork = vm.Date +
                TimeSpan.Parse(cell.Column.Header.ToString());
            var order = new OrderViewModel(vm.Orders.FirstOrDefault(el => el.DateWork >= dateWork && el.DateWork < dateWork + new TimeSpan(0, 30, 0)) ??
                      new OrderViewModel { DateWork = dateWork, PostID = vm.Post.ID, IsChanged = false, ID = vm.Orders.Count });
            order.IsCompleteSaved += OrderOnIsCompleteSaved;
            var detail = new CardOrder { DataContext = order };
            ccDetail.Children.Add(detail);
            tbPost.Text = vm.Post.Name;
            gDetail.Visibility = Visibility.Visible;
        }

        private void OrderOnIsCompleteSaved(object sender, EventArgs eventArgs)
        {
            var order = sender as OrderViewModel;
            if (order == null) return;
            _vm.UpdateOrders(order);
            gDetail.Visibility = Visibility.Collapsed;
            UpdateUIDay();
        }

        private void imgLeft_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _vm.Date = _vm.WeekStyle ? _vm.Date.AddDays(-7) : _vm.Date.AddDays(-1);
        }

        private void imgRight_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _vm.Date =_vm.WeekStyle ?  _vm.Date.AddDays(7) : _vm.Date.AddDays(1);
        }
    }


    public class BoolToValueMultyConverter<T> : IMultiValueConverter
    {

        // geekswithblogs.net/codingbloke/archive/2010/05/28/a-generic-boolean-value-converter.aspx

        public T FalseValue { get; set; }
        public T TrueValue { get; set; }
        public T DesibleValue { get; set; }

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var cell = values[0] as DataGridCell;
            var vm = values[1] as PostOrdersViewModel;
            if (cell == null || vm == null) return FalseValue;
            return vm.WeekStyle ? WeekStyle(vm, cell) : DayStyle(vm, cell);
        }

        private T DayStyle(PostOrdersViewModel vm, DataGridCell cell)
        {
            var time = TimeSpan.Parse(cell.Column.Header.ToString());
            var dt = vm.Date;
            if (time < new TimeSpan(vm.Post.StartWorkTime, 0, 0) || time > new TimeSpan(vm.Post.EndWorkTime, 0, 0))
            {
                cell.IsEnabled = false;
                return DesibleValue;
            }
            foreach (var el in vm.Orders.Where(el => el.DateWork >= dt + time && el.DateWork < dt + time + new TimeSpan(0, 30, 0)))
            {
                vm.SelectedOrder = el.ID;
                cell.IsEnabled = true;
                cell.ToolTip = string.Format("{0} ({1})",
                    el.User != null ? el.User.Nm : "User",
                    el.Car != null ? el.Car.MarkModelName : "");
                return TrueValue;
            }
            return FalseValue;
        }

        private T WeekStyle(PostOrdersViewModel vm, DataGridCell cell)
        {
            var dt = DateTime.Parse(cell.Column.Header.ToString());
            if (!vm.Orders.Any(el => el.DateWork >= dt && el.DateWork < dt + new TimeSpan(23, 59, 59)))
                return FalseValue;
            cell.IsEnabled = true;
            return TrueValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BoolToStyleConverter : BoolToValueMultyConverter<Brush> { }
}
