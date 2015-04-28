using DTCDev.Client.Cars.Service.Engine.Storage;
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

namespace DTCDev.Client.Cars.Service.Controls.Car
{
    /// <summary>
    /// Interaction logic for CarCurrentParamsViewer.xaml
    /// </summary>
    public partial class CarCurrentParamsViewer : UserControl
    {
        public CarCurrentParamsViewer()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            CarStorage.Instance.CurrentOBDLoaded += Instance_CurrentOBDLoaded;
            UpdateItems();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            CarStorage.Instance.CurrentOBDLoaded -= Instance_CurrentOBDLoaded;
        }

        void Instance_CurrentOBDLoaded(object sender, EventArgs e)
        {
            UpdateItems();
        }

        private bool _displayGraph = true;

        private void UpdateItems()
        {
            wrpData.Children.Clear();
            if (CarStorage.Instance.SelectedCar != null)
            {
                if (CarStorage.Instance.SelectedCar.CurrentOBD != null)
                {
                    foreach (var item in CarStorage.Instance.SelectedCar.CurrentOBD.OBD)
                    {
                        if (_displayGraph)
                        {
                            if (CarStorage.Instance.SelectedCar.OBDLast.ContainsKey(item.Key))
                            {
                                ParamsViewer.CarParamItem control = new ParamsViewer.CarParamItem(item.Key, item.Value, CarStorage.Instance.SelectedCar.OBDLast[item.Key]);
                                wrpData.Children.Add(control);
                            }
                            else
                            {
                                ParamsViewer.CarParamItem control = new ParamsViewer.CarParamItem(item.Key, item.Value);
                                wrpData.Children.Add(control);
                            }
                        }
                        else
                        {
                            ParamsViewer.CarParamItem control = new ParamsViewer.CarParamItem(item.Key, item.Value);
                            wrpData.Children.Add(control);
                        }
                    }
                }
            }
        }

        private void btnChsowChart_Click(object sender, RoutedEventArgs e)
        {
            _displayGraph = (bool)btnChsowChart.IsChecked;
            UpdateItems();
        }
    }
}
