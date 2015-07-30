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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DTCDev.Client.Cars.Controls.ViewModels.Car;
using DTCDev.Client.Cars.Engine.AppLogic;
using DTCDev.Client.Cars.Engine.DisplayModels;

namespace DTCDev.Client.Cars.Controls.Controls.Car
{
    /// <summary>
    /// Interaction logic for CarDetailsView.xaml
    /// </summary>
    public partial class CarDetailsView : UserControl
    {
        readonly CarDetailsViewModel _vm = new CarDetailsViewModel();
        public CarDetailsView()
        {
            InitializeComponent();
            this.DataContext = _vm;
        }

        DISP_Car _currentCar;

        public event EventHandler CloseMe;

        public void UpdateCarData(DISP_Car carData)
        {
            if (_currentCar != null)
                _currentCar.PropertyChanged -= _currentCar_PropertyChanged;
            _currentCar = carData;
            _currentCar.PropertyChanged += _currentCar_PropertyChanged;
            UpdateData();
            _vm.CAR = carData;
        }

        void _currentCar_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            UpdateData();
        }

        private void UpdateData()
        {
            speedPresenter.SetData((int)_currentCar.Current_Speed);
            sattelitePresenter.SetData(_currentCar.Data.Navigation.Sattelites);
            if(_currentCar.OBD.Where(p=>p.Key=="0C").FirstOrDefault()!=null)
            {
                int vol = 0;
                Int32.TryParse(_currentCar.OBD.Where(p => p.Key == "0C").First().Value, out vol);
                engineRPMPresenter.SetData(vol);
            }
            txtDateUpdate.Text = _currentCar.DateLastUpdate.ToString("dd.MM.yyyy");
            txtTimeUpdate.Text = _currentCar.DateLastUpdate.ToString("HH:mm:ss");

            var converter = new PIDConverter();
            stkOBD.Children.Clear();
            stkOBD.RowDefinitions.Clear();
            txtCar.Text = _currentCar.Mark + " " + _currentCar.Model;
            for (var i = 0; i< _currentCar.OBD.Count; i++)
            {
                stkOBD.RowDefinitions.Add(new RowDefinition());
                var item = _currentCar.OBD[i];
                var txtText = new TextBlock {TextWrapping = TextWrapping.Wrap, VerticalAlignment = VerticalAlignment.Center, Foreground = new SolidColorBrush(Colors.White),
                    Text = converter.GetPidInfo(item.Key), Margin = new Thickness(2)};
                txtText.SetValue(Grid.RowProperty, i); txtText.SetValue(Grid.ColumnProperty, 0);
                stkOBD.Children.Add(txtText);
                
                var outBorder = new Border
                {
                    Tag = item.Value,
                    Uid = item.Key,
                    Height = 6, 
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    BorderThickness = new Thickness(1), Margin = new Thickness(2, 2, 10, 2),
                    BorderBrush = new SolidColorBrush(Colors.White)
                };
                outBorder.SetValue(Grid.RowProperty, i);
                outBorder.SetValue(Grid.ColumnProperty, 1);
                outBorder.SizeChanged += outBorder_SizeChanged;
                stkOBD.Children.Add(outBorder);

                
            }
        }

        void outBorder_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var outBorder = sender as Border;
            if(outBorder == null) return;
            var value = 0;
            if(!int.TryParse(outBorder.Tag.ToString(), out value)) return;
            //var txtVol = new TextBlock { FontWeight = FontWeights.Bold, Text = value.ToString(), FontSize = 10,
            //    Margin = new Thickness(2), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center};
            var converter = new PIDConverter();
            var maxVal = converter.GetMaxVol(outBorder.Uid);
            var minVal = converter.GetMinVol(outBorder.Uid);
            var inBorder = new Border
            {
                MinWidth = 20, Height = 6, 
                HorizontalAlignment = HorizontalAlignment.Left,
                Width = value * outBorder.ActualWidth / (maxVal - minVal),
                BorderThickness = new Thickness(1),
                Background = new SolidColorBrush(Colors.White)
            };
            outBorder.Child = inBorder;
            //inBorder.Child = txtVol; 
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            if (CloseMe != null)
                CloseMe(this, new EventArgs());
        }
    }
}
