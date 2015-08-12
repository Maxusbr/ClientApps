using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DTCDev.Client.Cars.Controls.ViewModels.History;

namespace DTCDev.Client.Cars.Controls.Controls.History
{
    /// <summary>
    /// Логика взаимодействия для HistoryWorkControl.xaml
    /// </summary>
    public partial class HistoryWorkControl : UserControl
    {
        private readonly HistoryWorkViewModel _hvm;
        private bool _displayedDistance = false;
        private double _gridWhidt;
        private Storyboard _stDisplayHistory;
        private Storyboard _stbHideHistory;
        private bool _isAnimation;

        public HistoryWorkControl()
        {
            InitializeComponent();
            DataContext = _hvm = new HistoryWorkViewModel(Dispatcher);
            Init();
        }

        private void Init()
        {
            _hvm.PropertyChanged += _hvm_PropertyChanged;
            _stDisplayHistory = Resources["stbDisplayWorkHistory"] as Storyboard;
            if (_stDisplayHistory != null)
                _stDisplayHistory.Completed += (o, args) => _isAnimation = false;
            _stbHideHistory = Resources["stbHideWorkHistory"] as Storyboard;
            if (_stbHideHistory != null)
                _stbHideHistory.Completed += (o, args) =>
                {
                    btnShowDetail.Visibility = Visibility.Visible;
                    _isAnimation = false;
                };
        }

        private void grdHistoryWork_MouseEnter(object sender, MouseEventArgs e)
        {
            var animation = _stDisplayHistory.Children[0] as DoubleAnimation;
            if (animation == null || _isAnimation) return;
            _isAnimation = true;
            btnShowDetail.Visibility = Visibility.Collapsed;
            animation.To = _gridWhidt;
            _stDisplayHistory.Begin();
        }

        private void grdHistoryWork_MouseLeave(object sender, MouseEventArgs e)
        {
            var animation = _stbHideHistory.Children[0] as DoubleAnimation;
            if (animation == null || _isAnimation) return;
            _isAnimation = true;
            animation.From = _gridWhidt;
            _stbHideHistory.Begin();
        }

        private void grdHistoryWork_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var control = sender as Grid;
            if (control == null) return;
            if (control.Visibility == Visibility.Visible) _gridWhidt = Math.Max(_gridWhidt, control.ActualWidth);
        }

        private void btnDistance_Click(object sender, RoutedEventArgs e)
        {
            grdDistance.Visibility = _displayedDistance ? Visibility.Collapsed : Visibility.Visible;
            _displayedDistance = !_displayedDistance;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _hvm.LoadData();
        }

        private void _hvm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("TableHistory")) UpdateTable();
        }

        private void UpdateTable()
        {
            HistoryTable.Columns.Clear();
            HistoryTable.Columns.Add(new DataGridTextColumn { Header = "Время", Binding = new Binding("Time") });
            HistoryTable.Columns.Add(new DataGridTextColumn
            {
                Header = "Скорость",
                Binding = new Binding("Speed"),
                CellStyle = Resources["StyleCell"] as Style
            });
            HistoryTable.Columns.Add(new DataGridTextColumn { Header = "Спутники", Binding = new Binding("Satelite") });
            var converter = new Engine.AppLogic.PIDConverter();
            if (_hvm.OBDHistory == null || _hvm.OBDHistory.Data.Count == 0) return;
            foreach (var item in _hvm.OBDHistory.Data.Select(p => p.Code).Distinct())
            {
                var tb = new TextBlock
                {
                    Text = converter.GetPidInfo(item),
                    TextWrapping = TextWrapping.Wrap,
                    MaxWidth = 100
                };
                var bd = new Binding() { Converter = new OBDKeyConverter(), ConverterParameter = item };
                HistoryTable.Columns.Add(new DataGridTextColumn { Header = tb, Binding = bd });
            }
        }
    }
}
