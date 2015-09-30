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
using DTCDev.Client.Cars.Engine.AppLogic;
using DTCDev.Client.Cars.Engine.DisplayModels;

namespace DTCDev.Client.Cars.Controls.Controls.Car
{
    /// <summary>
    /// Логика взаимодействия для CarDetailView.xaml
    /// </summary>
    public partial class CarDetailView : UserControl
    {
        private DISP_Car _vm;

        private DISP_Car Vm
        {
            get { return _vm; }
            set
            {
                if(_vm == value) return;
                if(_vm != null) _vm.PropertyChanged -= nValue_PropertyChanged;
                _vm = value;
                if (_vm != null) _vm.PropertyChanged += nValue_PropertyChanged;
                DisplayOBD(value);
            }
        }
        public CarDetailView()
        {
            InitializeComponent();
            
        }

        void nValue_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Data")
                DisplayOBD((DISP_Car)sender);
        }

        private void DisplayOBD(DISP_Car car)
        {
            if (car.OBD == null)
                return;
            stkOBDParams.Children.Clear();
            var converter = new PIDConverter();
            var errorfinded = false;
            if (!car.OBD.Any()) return;
            foreach (var item in car.OBD)
            {
                var sb = new StringBuilder();
                sb.Append(item.Value);
                sb.Append(" - ");
                sb.Append(converter.GetPidInfo(item.Key));
                var min = converter.GetMinVol(item.Key);
                var max = converter.GetMaxVol(item.Key);
                var vol = 0;
                int.TryParse(item.Value, out vol);
                var text = new TextBlock { Text = sb.ToString() };
                if (vol < min || vol > max)
                {
                    errorfinded = true;
                    text.Foreground = new SolidColorBrush(Colors.DarkRed);
                    text.FontWeight = FontWeights.Bold;
                }
                stkOBDParams.Children.Add(text);
            }
            //brdrOBDStatus.Background = errorfinded ? new SolidColorBrush(Colors.Orange) : new SolidColorBrush(Colors.White);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Vm = DataContext as DISP_Car;
        }
    }
}
