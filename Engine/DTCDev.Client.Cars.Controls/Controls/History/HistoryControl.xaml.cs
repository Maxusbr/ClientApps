using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DTCDev.Client.Cars.Controls.ViewModels.History;
using System.Windows.Media.Animation;
using System.Windows.Controls.Primitives;

namespace DTCDev.Client.Cars.Controls.Controls.History
{
    /// <summary>
    /// Interaction logic for HistoryControl.xaml
    /// </summary>
    public partial class HistoryControl : UserControl
    {
        readonly HistoryViewModel _hvm;

        public HistoryControl()
        {
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this))
                return;
            DataContext  = _hvm = new HistoryViewModel(Dispatcher);
            Properties.Settings.Default.Reload();
        }

        public HistoryControl(DateTime date)
        {
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this))
                return;
            this.DataContext = _hvm;
            _hvm.StartDate = date;
            _hvm.StopDate = date + new TimeSpan(1, 0, 0, 0);
            Properties.Settings.Default.Reload();
        }

        void hvm_HideSelectDate(object sender, EventArgs e)
        {
            //stbHideDate.Begin();
        }

        void hvm_DisplaySelectDate(object sender, EventArgs e)
        {
            //stbShowDate.Begin();
        }


        private void Image_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {

        }

        private bool _playerVisible = false;

        private void Image_MouseLeftButtonUp_2(object sender, MouseButtonEventArgs e)
        {
            //if (_playerVisible)
            //{
            //    grdMove.Visibility = Visibility.Visible;
            //    grdPlayer.Visibility = System.Windows.Visibility.Collapsed;
            //}
            //else
            //{
            //    grdMove.Visibility = Visibility.Collapsed;
            //    grdPlayer.Visibility = System.Windows.Visibility.Visible;
            //}
            //_playerVisible = !_playerVisible;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            MultiValueSlider.ValueChanged += MultiValueSlider_ValueChanged;
            //_stDisplayHistory = Resources["stbDisplayWorkHistory"] as Storyboard;
            //if (_stDisplayHistory != null)
            //    _stDisplayHistory.Completed += (o, args) => _isAnimation = false;
            //_stbHideHistory = Resources["stbHideWorkHistory"] as Storyboard;
            //if (_stbHideHistory != null)
            //    _stbHideHistory.Completed += (o, args) =>
            //    {
            //        btnShowDetail.Visibility = Visibility.Visible;
            //        _isAnimation = false;
            //    };
        }



        void MultiValueSlider_ValueChanged(object sender, EventArgs e)
        {
            _hvm.UpdateRoutes(false);
        }

        bool _displayHistory = false;

        public void HistoryButton_Click()
        {
            if (_displayHistory == false)
            {
                CarPoints.Opacity = .5;
                _hvm.EnableHistory = true;
                _hvm.LoadData();
                _hvm.PropertyChanged += _hvm_PropertyChanged;
                _hvm.CenterUpdates += _hvm_CenterUpdates;
                //grdHistoryWork.Visibility = Visibility.Visible;
            }
            else
            {
                CarPoints.Opacity = 1;
                _hvm.EnableHistory = false;
                _hvm.PropertyChanged -= _hvm_PropertyChanged;
                _hvm.CenterUpdates -= _hvm_CenterUpdates;
                //grdHistoryWork.Visibility = Visibility.Collapsed;
            }
            _displayHistory = !_displayHistory;
            CarPin.Visibility = ParkingsPin.Visibility =
            RouteLine.Visibility = WarningLine.Visibility = ErrorLine.Visibility = OfflineLine.Visibility =
            _carZonesError.Visibility = _displayHistory ? Visibility.Visible : Visibility.Collapsed;
        }

        void _hvm_CenterUpdates(Client.Controls.Map.Location southWest, Client.Controls.Map.Location northEast)
        {
            Dispatcher.BeginInvoke(new Action(() =>
                {
                    Map.ZoomToBounds(southWest, northEast);
                }));
        }

        private void _hvm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //if (e.PropertyName.Equals("TableHistory")) UpdateTable();
            if (!e.PropertyName.Equals("IsCheckedAccelerate") && !e.PropertyName.Equals("IsCheckedWay") && !e.PropertyName.Equals("IsCheckedSpeed")) return;
            if (_hvm.IsCheckedAccelerate || _hvm.IsCheckedWay || _hvm.IsCheckedSpeed)
            {
                MultiValueSlider.Visibility = Visibility.Visible;
                MultiValueSlider.UpdatePos(_hvm.LeftValue, _hvm.RightValue, _hvm.MinValue, _hvm.MaxValue);
            }
        }

        //private void UpdateTable()
        //{
        //    HistoryTable.Columns.Clear();
        //    HistoryTable.Columns.Add(new DataGridTextColumn { Header = "Время", Binding = new Binding("Time") });
        //    HistoryTable.Columns.Add(new DataGridTextColumn
        //    {
        //        Header = "Скорость",
        //        Binding = new Binding("Speed"),
        //        CellStyle = Resources["StyleCell"] as Style
        //    });
        //    HistoryTable.Columns.Add(new DataGridTextColumn { Header = "Спутники", Binding = new Binding("Satelite") });
        //    var converter = new Engine.AppLogic.PIDConverter();
        //    if (_hvm.OBDHistory == null || _hvm.OBDHistory.Data.Count == 0) return;
        //    foreach (var item in _hvm.OBDHistory.Data.Select(p => p.Code).Distinct())
        //    {
        //        var tb = new TextBlock
        //        {
        //            Text = converter.GetPidInfo(item),
        //            TextWrapping = TextWrapping.Wrap,
        //            MaxWidth = 100
        //        };
        //        var bd = new Binding() { Converter = new OBDKeyConverter(), ConverterParameter = item };
        //        HistoryTable.Columns.Add(new DataGridTextColumn { Header = tb, Binding = bd });
        //    }
        //}

        //private void grdHistoryWork_MouseEnter(object sender, MouseEventArgs e)
        //{
        //    var animation = _stDisplayHistory.Children[0] as DoubleAnimation;
        //    if (animation == null || _isAnimation) return;
        //    _isAnimation = true;
        //    btnShowDetail.Visibility = Visibility.Collapsed;
        //    animation.To = _gridWhidt;
        //    _stDisplayHistory.Begin();
        //}

        //private void grdHistoryWork_MouseLeave(object sender, MouseEventArgs e)
        //{
        //    var animation = _stbHideHistory.Children[0] as DoubleAnimation;
        //    if (animation == null || _isAnimation) return;
        //    _isAnimation = true;
        //    animation.From = _gridWhidt;
        //    _stbHideHistory.Begin();
        //}

        //private bool _displayedDistance = false;
        //private void btnDistance_Click(object sender, RoutedEventArgs e)
        //{
        //    if (_displayedDistance)
        //        grdDistance.Visibility = Visibility.Collapsed;
        //    else
        //        grdDistance.Visibility = Visibility.Visible;
        //    _displayedDistance = !_displayedDistance;
        //}

        //private double _gridWhidt;
        //private Storyboard _stDisplayHistory;
        //private Storyboard _stbHideHistory;
        //private bool _isAnimation;

        //private void grdHistoryWork_SizeChanged(object sender, SizeChangedEventArgs e)
        //{
        //    var control = sender as Grid;
        //    if (control == null) return;
        //    if (control.Visibility == Visibility.Visible) _gridWhidt = Math.Max(_gridWhidt, control.ActualWidth);
        //}

        private void CheckedSpeed_Checked(object sender, RoutedEventArgs e)
        {
            if(_hvm == null) return;
            _hvm.IsCheckedSpeed = _hvm.IsCheckedWay = _hvm.IsCheckedAccelerate = false;
            if (CheckedSpeed.IsChecked ?? false)
            {
                CheckedAccelerate.IsChecked = CheckedWay.IsChecked = false;
                _hvm.IsCheckedSpeed = true;
            }
        }

        private void CheckedWay_Checked(object sender, RoutedEventArgs e)
        {
            if (_hvm == null) return;
            _hvm.IsCheckedSpeed = _hvm.IsCheckedWay = _hvm.IsCheckedAccelerate = false;
            if (CheckedWay.IsChecked ?? false)
            {
                CheckedAccelerate.IsChecked = CheckedSpeed.IsChecked = false;
                _hvm.IsCheckedWay = true;
            }
        }

        private void CheckedAccelerate_Checked(object sender, RoutedEventArgs e)
        {
            if (_hvm == null) return;
            _hvm.IsCheckedSpeed = _hvm.IsCheckedWay = _hvm.IsCheckedAccelerate = false;
            if (CheckedAccelerate.IsChecked ?? false)
            {
                CheckedWay.IsChecked = CheckedSpeed.IsChecked = false;
                _hvm.IsCheckedAccelerate = true;
            }
        }

        private void GridSplitter_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            Properties.Settings.Default.Save();
        }
    }

    public class OBDKeyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var row = value as HistoryRow;
            if (row == null) return null;
            var item = row.Data.FirstOrDefault(o => o.Code.Equals(parameter.ToString()));
            if (item == null) return null;
            return string.Format("{0} ({1})",item.Vol, item.Time) ;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }

    public class MaxMinValues
    {
        public int MaxValue { get; set; }
        public int MinValue { get; set; }
    }
    public class ValuesMultyConverter<T> : IMultiValueConverter
    {
        public T FalseValue { get; set; }
        public T TrueValue { get; set; }
        public T СonditionValue { get; set; }
        public T DesibleValue { get; set; }

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var cell = values[0] as DataGridCell;
            var vm = values[1] as HistoryRow;
            if (cell == null || vm == null || string.IsNullOrEmpty(vm.Speed)) return DesibleValue;
            if (vm.CurentSpeed <= vm.MinSpeed) return TrueValue;
            return vm.CurentSpeed >= vm.MaxSpeed ? FalseValue : СonditionValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class SpeedValueToStyleConverter : ValuesMultyConverter<Brush> { }
}
