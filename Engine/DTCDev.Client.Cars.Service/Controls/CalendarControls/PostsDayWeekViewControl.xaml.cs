using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
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
            _vm = this.DataContext as PostsDayWeekViewModel;
            PostList.SelectedCellsChanged += PostList_SelectedCellsChanged;
            UpdateUI();
        }

        private void UpdateUI()
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

        private void PostList_SelectedCellsChanged(object sender, System.Windows.Controls.SelectedCellsChangedEventArgs e)
        {
            if (e.AddedCells == null || e.AddedCells.Count == 0 ) return;
            var vm = e.AddedCells[0].Item as PostOrdersViewModel;
            var time = TimeSpan.Parse(e.AddedCells[0].Column.Header.ToString());
            var dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            if(vm == null) return;
            var order = vm.Orders.FirstOrDefault(el => el.DateWork >= dt + time && el.DateWork < dt + time + new TimeSpan(0, 30, 0));
            if(order == null)
            {
                order =new OrderViewModel{ DateWork =  dt + time, PostID = vm.Post.ID};
                vm.Orders.Add(order);
            }
            order.PropertyChanged -= order_PropertyChanged;
            order.PropertyChanged += order_PropertyChanged;
            var detail = new CardOrder {DataContext = order};
            ccDetail.Content = detail;
            tbPost.Text = vm.Post.Name;
            gDetail.Visibility = Visibility.Visible;
        }

        void order_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            SaveButton.IsEnabled = true;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateUI();
            SaveButton.IsEnabled = false;
            gDetail.Visibility = Visibility.Collapsed;
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
            
            var time = TimeSpan.Parse(cell.Column.Header.ToString());
            var dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            if (time < new TimeSpan(vm.Post.StartWorkTime, 0, 0) || time > new TimeSpan(vm.Post.EndWorkTime, 0, 0))
            {
                cell.IsEnabled = false;
                return DesibleValue;
            }
            foreach (var el in vm.Orders.Where(el => el.DateWork >= dt + time && el.DateWork < dt + time + new TimeSpan(0, 30, 0)))
            {
                vm.SelectedOrder = el.ID;
                cell.IsEnabled = true;
                cell.ToolTip = string.Format("{0} ({1})", el.User != null ? el.User.Nm : "User", el.Car.MarkModelName);
                return TrueValue;
            }
            return FalseValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BoolToStyleConverter : BoolToValueMultyConverter<Brush> { }
}
