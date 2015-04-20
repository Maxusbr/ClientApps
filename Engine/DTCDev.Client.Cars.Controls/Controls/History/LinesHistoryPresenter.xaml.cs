using DTCDev.Client.Cars.Engine.AppLogic;
using DTCDev.Client.Cars.Engine.DisplayModels;
using DTCDev.Client.Sensors;
using DTCDev.Models;
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

namespace DTCDev.Client.Cars.Controls.Controls.History
{
    /// <summary>
    /// Interaction logic for LinesHistoryPresenter.xaml
    /// </summary>
    public partial class LinesHistoryPresenter : UserControl
    {
        public LinesHistoryPresenter()
        {
            InitializeComponent();

            CarSelector.OnCarChanged += CarSelector_OnCarChanged;
        }

        void CarSelector_OnCarChanged(Engine.DisplayModels.DISP_Car car)
        {
            _car = car;
        }

        private LinesDataModel _data;
        private DISP_Car _car;

        private static DependencyProperty DataProperty = DependencyProperty.Register("Data", typeof(LinesDataModel),
            typeof(LinesHistoryPresenter),
            new PropertyMetadata(new LinesDataModel(), DataPropertyChanged));

        public LinesDataModel Data
        {
            get { return (LinesDataModel)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        private static void DataPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            LinesHistoryPresenter control = sender as LinesHistoryPresenter;
            control._data = (LinesDataModel)e.NewValue;
            control.DisplayData();
        }

        private void DisplayData()
        {
            if (_car == null || _data == null)
                return;

            stkDisplay.Children.Clear();

            foreach (var item in _car.Device.Sensors)
            {
                ControllerLineHistoryRow row = new ControllerLineHistoryRow(item, _data);
                stkDisplay.Children.Add(row);
            }
        }
    }
}
