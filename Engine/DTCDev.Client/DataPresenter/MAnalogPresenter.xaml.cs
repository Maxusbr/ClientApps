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

namespace DTCDev.Client.DataPresenter
{
    /// <summary>
    /// Interaction logic for MAnalogPresenter.xaml
    /// </summary>
    public partial class MAnalogPresenter : UserControl
    {
        public MAnalogPresenter()
        {
            InitializeComponent();
        }

        private int _start = 0;
        private int _stop = 180;
        private int _mn = 0;
        private string _milest = "";

        public int Start
        {
            get { return _start; }
            set
            {
                _start = value;
                UpdateParams();
            }
        }

        public int Stop
        {
            get { return _stop; }
            set
            {
                _stop = value;
                UpdateParams();
            }
        }

        public string Milest
        {
            get { return _milest; }
            set
            {
                _milest = value;
                if (_mn < 1)
                    txtData.Text = _milest;
                else
                    txtData.Text = _milest + ", x" + _mn.ToString();
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //Storyboard sb = this.FindResource("stbRange") as Storyboard;
            //sb.Begin();
        }

        private void UpdateParams()
        {
            if(Stop>999)
            {
                Stop = Stop / 1000;
                _mn = 1000;
            }
            int total = _stop - _start;
            int step = total / 6;
            txt1.Text = _start.ToString();
            txt2.Text = (_start + step).ToString();
            txt3.Text = (_start + step * 2).ToString();
            txt4.Text = (_start + step * 3).ToString();
            txt5.Text = (_start + step * 4).ToString();
            txt6.Text = (_start + step * 5).ToString();
            txt7.Text = (_start + step * 6).ToString();
            _stop = _start + step * 6;
            double totDouble = total;
            _degreesPerStep = 270.0f / totDouble;
        }

        private int _lastVol = 0;

        private double _degreesPerStep;

        public void SetData(int vol)
        {
            if (_lastVol == vol)
                return;
            if (_stbBeginComplete == false)
            {
                _display = vol;
                return;
            }
            double v = vol;
            double ov = _lastVol;
            double angleOld = ov * _degreesPerStep;
            double angleNew = v * _degreesPerStep;
            angleOld = Math.Round(angleOld);
            angleNew = Math.Round(angleNew);
            var da = new DoubleAnimation
            {
                From = angleOld,
                To = angleNew,
                Duration = TimeSpan.FromMilliseconds(500)
            };
            Storyboard sb = new Storyboard();
            RotateTransform rt = (RotateTransform)((TransformGroup)brdrViewer.RenderTransform).Children[2];
            rt.BeginAnimation(RotateTransform.AngleProperty, da);
            _lastVol = vol;
        }

        bool _stbBeginComplete = false;
        int _display = 0;

        private void Storyboard_Completed(object sender, EventArgs e)
        {
            _stbBeginComplete = true;
            SetData(_display);
        }
    }
}
