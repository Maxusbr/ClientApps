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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DTCDev.Client.Cars.Controls.ViewModels.History;

namespace DTCDev.Client.Cars.Controls.Controls.History
{
    /// <summary>
    /// Логика взаимодействия для HistoryTablesControl.xaml
    /// </summary>
    public partial class HistoryTablesControl : UserControl
    {
        private readonly HistoryWorkViewModel _hvm;

        public HistoryTablesControl()
        {
            InitializeComponent();
            DataContext = _hvm = new HistoryWorkViewModel(Dispatcher);
            _hvm.PropertyChanged += _hvm_PropertyChanged;
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
                var bd = new Binding() { Converter = new OBDKeyConverter(), ConverterParameter = item };
                HistoryTable.Columns.Add(new DataGridTextColumn { Header = converter.GetPidInfo(item), Binding = bd });
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if(!_hvm.HistoryRows.Any())
                _hvm.LoadData();
        }
    }
}
