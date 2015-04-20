using DTCDev.Client.Cars.Engine.AppLogic;
using DTCDev.Client.Cars.Engine.DisplayModels;
using DTCDev.Client.Cars.Engine.Handlers.Cars;
using DTCDev.Models.CarsSending.Car;
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
    /// Interaction logic for OBDDataPresenter.xaml
    /// </summary>
    public partial class OBDDataPresenter : UserControl
    {
        public OBDDataPresenter()
        {
            InitializeComponent();
            CarSelector.OnCarChanged += CarSelector_OnCarChanged;
            _selectedCar = CarSelector.SelectedCar;
        }
        DISP_Car _selectedCar;
        OBDHistoryDataModel _data;

        void CarSelector_OnCarChanged(Engine.DisplayModels.DISP_Car car)
        {
            if (car != null)
                _selectedCar = car;

        }

        private static DependencyProperty DataProperty = DependencyProperty.Register("Data", typeof(OBDHistoryDataModel),
            typeof(OBDDataPresenter),
            new PropertyMetadata(new OBDHistoryDataModel(), DataPropertyChanged));

        public OBDHistoryDataModel Data
        {
            get { return (OBDHistoryDataModel)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        private static void DataPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            OBDDataPresenter control = sender as OBDDataPresenter;
            control._data = (OBDHistoryDataModel)e.NewValue;
            control.DisplayData();
        }


        private void DisplayData()
        {
            stkData.Children.Clear();
            if (_data == null)
                return;
            else
            {
                List<string> prms = _data.Data.Select(p => p.Code).Distinct().ToList();
                foreach (var item in prms)
                {
                    List<OBDHistoryDataModel.OBDParam> temp = _data.Data.Where(p => p.Code == item).ToList();
                    OBDHistoryRow row = new OBDHistoryRow(item, temp);
                    row.Margin = new Thickness(0, 2, 0, 2);
                    stkData.Children.Add(row);

                }
            }
        }
    }
}
