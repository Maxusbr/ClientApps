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
using System.Windows.Controls.Primitives;
using DTCDev.Client.Cars.Engine.DisplayModels.CarModelHelper;
using DTCDev.Client.Cars.Engine.Handlers.Cars;

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
            try
            {
                if (_currentCar != null)
                    _currentCar.PropertyChanged -= _currentCar_PropertyChanged;
                _currentCar = carData;
                _currentCar.PropertyChanged += _currentCar_PropertyChanged;
                UpdateData();
                _vm.CAR = carData;
                SetOutStates();
            }
            catch { }
        }

        void _currentCar_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            UpdateData();
        }

        private void UpdateData()
        {
            speedPresenter.SetData((int)_currentCar.Navigation.Current_Speed);
            sattelitePresenter.SetData(_currentCar.Data.Navigation.Sattelites);
            if(_currentCar.OBD.Where(p=>p.Key=="0C").FirstOrDefault()!=null)
            {
                int vol = 0;
                Int32.TryParse(_currentCar.OBD.Where(p => p.Key == "0C").First().Value, out vol);
                engineRPMPresenter.SetData(vol);
            }
            txtDateUpdate.Text = _currentCar.Navigation.DateLastUpdate.ToString("dd.MM.yyyy");
            txtTimeUpdate.Text = _currentCar.Navigation.DateLastUpdate.ToString("HH:mm:ss");

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

        /// <summary>
        /// исходное состояние линий выхода
        /// </summary>
        private OutStateModel _currentOutsState = new OutStateModel();

        /// <summary>
        /// Установить текущее состояние выходов
        /// </summary>
        private void SetOutStates()
        {
            if (_currentCar == null)
                return;
            else
            {
                imgChangedState1.Visibility = Visibility.Collapsed;
                imgChangedState3.Visibility = Visibility.Collapsed;

                brdrSetSate1.Visibility = Visibility.Collapsed;
                brdrSetSate3.Visibility = Visibility.Collapsed;

                if (_currentCar.Outs.Out1)
                    brdrCurrentSate1.Background = new SolidColorBrush(Colors.LightGreen);
                else
                    brdrCurrentSate1.Background = new SolidColorBrush(Colors.Gray);

                if (_currentCar.Outs.Out2)
                    btnChangeOut_2.Content = "Разрешить запуск";
                else
                    btnChangeOut_2.Content = "Заглушить автомобиль";
                
                if (_currentCar.Outs.Out3)
                    brdrCurrentSate3.Background = new SolidColorBrush(Colors.LightGreen);
                else
                    brdrCurrentSate3.Background = new SolidColorBrush(Colors.Gray);

                _currentOutsState.Out1 = _currentCar.Outs.Out1;
                _currentOutsState.Out2 = _currentCar.Outs.Out2;
                _currentOutsState.Out3 = _currentCar.Outs.Out3;
                CheckButton();
            }
        }

        void outBorder_SizeChanged(object sender, SizeChangedEventArgs e)
        {

            var outBorder = sender as Border;
            if(outBorder == null) return;
            var value = 0;
            if(!int.TryParse(outBorder.Tag.ToString(), out value)) return;
            var converter = new PIDConverter();
            var maxVal = converter.GetMaxVol(outBorder.Uid);
            var minVal = converter.GetMinVol(outBorder.Uid);
            double curWidth = value * outBorder.ActualWidth / (maxVal - minVal);
            if (curWidth < 0)
                curWidth = 0;
            var inBorder = new Border
            {
                MinWidth = 20, Height = 6, 
                HorizontalAlignment = HorizontalAlignment.Left,
                Width = curWidth,
                BorderThickness = new Thickness(1),
                Background = new SolidColorBrush(Colors.White)
            };
            outBorder.Child = inBorder;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            if (CloseMe != null)
                CloseMe(this, new EventArgs());
        }

        private void btnPanel_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton tb = (ToggleButton)sender;
            foreach (var item in grdButtons.Children)
            {
                if (((ToggleButton)item).Name != tb.Name)
                    ((ToggleButton)item).IsChecked = false;
            }
            if (tb.Name == "btnSettings")
            {
                grdSettings.Visibility = Visibility.Visible;
                grdControl.Visibility = Visibility.Collapsed;
            }
            else if (tb.Name == "btnPanel")
            {
                grdSettings.Visibility = Visibility.Collapsed;
                grdControl.Visibility = Visibility.Collapsed;
            }
            else
            {
                grdSettings.Visibility = Visibility.Collapsed;
                grdControl.Visibility = Visibility.Visible;
            }
        }

        private void btnChangeOut_1_Click(object sender, RoutedEventArgs e)
        {
            _currentCar.Outs.Out1 = !_currentCar.Outs.Out1;
            if(_currentCar.Outs.Out1==_currentOutsState.Out1)
            {
                imgChangedState1.Visibility = Visibility.Collapsed;
                brdrSetSate1.Visibility = Visibility.Collapsed;
            }
            else
            {
                imgChangedState1.Visibility = Visibility.Visible;
                brdrSetSate1.Visibility = Visibility.Visible;
                if (_currentCar.Outs.Out1)
                    brdrSetSate1.Background = new SolidColorBrush(Colors.LightGreen);
                else
                    brdrSetSate1.Background = new SolidColorBrush(Colors.Gray);
            }
            CheckButton();
        }

        private void btnChangeOut_2_Click(object sender, RoutedEventArgs e)
        {
            _currentCar.Outs.Out2 = !_currentCar.Outs.Out2;
            if (_currentCar.Outs.Out2 == _currentOutsState.Out2)
            {
                txtSendLock.Visibility = Visibility.Collapsed;
            }
            else
            {
                txtSendLock.Visibility = Visibility.Visible;
                if (_currentCar.Outs.Out2)
                    txtSendLock.Text = "Будет отправлена команда заглушить";
                else
                    txtSendLock.Text = "Будет отправлена команда разрешить запуск";
            }
            CheckButton();
        }

        private void btnChangeOut_3_Click(object sender, RoutedEventArgs e)
        {
            _currentCar.Outs.Out3 = !_currentCar.Outs.Out3;
            if (_currentCar.Outs.Out3 == _currentOutsState.Out3)
            {
                imgChangedState3.Visibility = Visibility.Collapsed;
                brdrSetSate3.Visibility = Visibility.Collapsed;
            }
            else
            {
                imgChangedState3.Visibility = Visibility.Visible;
                brdrSetSate3.Visibility = Visibility.Visible;
                if (_currentCar.Outs.Out3)
                    brdrSetSate3.Background = new SolidColorBrush(Colors.LightGreen);
                else
                    brdrSetSate3.Background = new SolidColorBrush(Colors.Gray);
            }
            CheckButton();
        }

        private void btnSaveDriving_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_currentCar == null)
                    return;
                if ((DateTime.Now - _currentCar.Navigation.DateLastUpdate).Seconds > 60)
                {
                    MessageBox.Show("Автомобиль не на связи. Команда не будет отправлена");
                    return;
                }
                else if (_currentCar.Navigation.Current_Speed > 5)
                {
                    MessageBox.Show("Автомобиль находится в движении. Глушение автомобиля невозможно");
                    return;
                }
                else
                {
                    CarsHandler.Instance.SaveOutState(_currentCar);
                    SetOutStates();
                }
            }
            catch
            {
                MessageBox.Show("При выполнении запроса произошла ошибка. Попробуйте повторить позднее");
            }
        }

        private void CheckButton()
        {
            if (_currentOutsState.Out1 != _currentCar.Outs.Out1 ||
                _currentOutsState.Out2 != _currentCar.Outs.Out2 ||
                _currentOutsState.Out3 != _currentCar.Outs.Out3)
                btnSaveDriving.Visibility = Visibility.Visible;
            else
                btnSaveDriving.Visibility = Visibility.Collapsed;
        }
    }
}
