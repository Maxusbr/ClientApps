using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using DTCDev.Client.Cars.Service.Engine.Controls.ViewModels;

namespace DTCDev.Client.Cars.Service.Convertors
{
    public class BoolToValueMultyConverter<T> : IMultiValueConverter
    {
        public T FalseValue { get; set; }
        public T TrueValue { get; set; }
        public T СonditionValue { get; set; }
        public T DesibleValue { get; set; }

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var cell = values[0] as DataGridCell;
            var vm = values[1] as IDateList;
            if (cell == null || vm == null) return FalseValue;
            return vm.WeekStyle ? WeekStyle(vm, cell) : DayStyle(vm, cell);
        }

        private T DayStyle(IDateList vm, DataGridCell cell)
        {
            TimeSpan time;
            if (!TimeSpan.TryParse(cell.Column.Header.ToString(), out time)) return FalseValue;
            var dt = vm.Date;
            if (time < new TimeSpan(vm.StartWorkTime, 0, 0) || time > new TimeSpan(vm.EndWorkTime, 0, 0))
            {
                cell.IsEnabled = false;
                return DesibleValue;
            }
            foreach (var el in vm.DatesList.Where(el => el.DateWork >= dt + time && el.DateWork < dt + time + new TimeSpan(0, 30, 0)))
            {
                //vm.SelectedOrder = el.ID;
                cell.IsEnabled = true;
                cell.ToolTip = el.ToolTip;
                return el.Сondition ? СonditionValue: TrueValue;
            }
            return FalseValue;
        }

        private T WeekStyle(IDateList vm, DataGridCell cell)
        {
            DateTime dt;
            if (!DateTime.TryParse(cell.Column.Header.ToString(), out dt)) return FalseValue;
            if (!vm.DatesList.Any(el => el.DateWork >= dt && el.DateWork < dt + new TimeSpan(23, 59, 59)))
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
