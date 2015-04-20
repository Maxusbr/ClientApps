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

namespace DTCDev.Client.Sensors.OBD
{
    /// <summary>
    /// Interaction logic for AccelerationSensor.xaml
    /// </summary>
    public partial class AccelerationSensor : UserControl
    {
        public AccelerationSensor()
        {
            InitializeComponent();
        }

        public void Init(string PID, string vol, int min, int max, int normalMin, int normalMax)
        {
            _pid = PID;
            _vol = vol;
            _min = min;
            _max = max;
            _normalMin = normalMin;
            _normalMax = normalMax;
            DisplayData();
        }

        private string _pid;
        private string _vol;
        private int _min;
        private int _max;
        private int _normalMin;
        private int _normalMax;

        public string PID
        {
            get { return _pid; }
            set { _pid = value; }
        }

        public string Vol
        {
            get { return _vol; }
            set
            {
                _vol = value;
                DisplayData();
            }
        }

        public int Min
        {
            get { return _min; }
            set { _min = value; }
        }

        public int Max
        {
            get { return _max; }
            set { _max = value; }
        }

        private void DisplayData()
        {
            int v = 0;
            Int32.TryParse(_vol, out v);
            if (v < _normalMin || v > _normalMax)
            {
                brdrLoadBad.Visibility = Visibility.Visible;
            }
            else
                brdrLoadBad.Visibility = Visibility.Collapsed;

            double width = _max - _min;
            width = 26.0f / width * v;
            if (width >= 0)
                brdrLoadBad.Width = brdrLoadGood.Width = width;
            txtLoad.Text = _vol;
        }
    }
}
