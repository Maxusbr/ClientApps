using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using DTCDev.Models.DeviceSender.DISP;

namespace DTCDev.Client.Sensors.Analog.Max.Help
{
    class AnalogViewModel : BaseViewModel
    {
        public AnalogViewModel()
        {
        }

        public AnalogViewModel(DevicePresenter.Sensor pres)
        {
            SensorModel = pres;
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.RunWorkerAsync();
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (_sensorModel.State != null)
                SetValue(_sensorModel.State.Vol);
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(800);
        }




        private Visibility _visAlarm = Visibility.Collapsed;
        public Visibility VisAlarm
        {
            get { return _visAlarm; }
            set
            {
                _visAlarm = value;
                this.OnPropertyChanged("VisAlarm");
            }
        }

        private Visibility _visMinArc = Visibility.Collapsed;
        public Visibility VisMinArc
        {
            get { return _visMinArc; }
            set
            {
                _visMinArc = value;
                this.OnPropertyChanged("VisMinArc");
            }
        }

        private Visibility _visMaxArc = Visibility.Collapsed;
        public Visibility VisMaxArc
        {
            get { return _visMaxArc; }
            set
            {
                _visMaxArc = value;
                this.OnPropertyChanged("VisMaxArc");
            }
        }

        private string _vol0 = "";
        public string Vol0
        {
            get { return _vol0; }
            set
            {
                _vol0 = value;
                this.OnPropertyChanged("Vol0");
            }
        }

        private string _vol1 = "";
        public string Vol1
        {
            get { return _vol1; }
            set
            {
                _vol1 = value;
                this.OnPropertyChanged("Vol1");
            }
        }

        private string _vol2 = "";
        public string Vol2
        {
            get { return _vol2; }
            set
            {
                _vol2 = value;
                this.OnPropertyChanged("Vol2");
            }
        }

        private string _vol3 = "";
        public string Vol3
        {
            get { return _vol3; }
            set
            {
                _vol3 = value;
                this.OnPropertyChanged("Vol3");
            }
        }

        private string _vol4 = "";
        public string Vol4
        {
            get { return _vol4; }
            set
            {
                _vol4 = value;
                this.OnPropertyChanged("Vol4");
            }
        }

        private int _lastAngle = -30;
        public int LastAngle
        {
            get { return _lastAngle; }
            set
            {
                _lastAngle = value;
                this.OnPropertyChanged("LastAngle");
            }
        }

        private int _newAngle = -30;
        public int NewAngle
        {
            get { return _newAngle; }
            set
            {
                _newAngle = value;
                this.OnPropertyChanged("NewAngle");
            }
        }

        private string _vol = "0";
        public string Vol
        {
            get { return _vol; }
            set
            {
                _vol = value;
                this.OnPropertyChanged("Vol");
            }
        }

        public event EventHandler StartAnimate;



        private DevicePresenter.Sensor _sensorModel = new DevicePresenter.Sensor();

        public DevicePresenter.Sensor SensorModel
        {
            get { return _sensorModel; }
            set
            {
                _sensorModel = value;
                if (_sensorModel == null)
                    return;
                _sensorModel.PropertyChanged += _sensorModel_PropertyChanged;
                PreInitValues();
                
            }
        }

        void _sensorModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "State")
            {
                SetValue(_sensorModel.State.Vol);
            }
        }


        private void SetValue(int alarm)
        {
            LastAngle = NewAngle;
            if (alarm <= start)
                NewAngle = -30;
            else if (alarm >= stop)
                NewAngle = 210;
            else
            {
                decimal step = (decimal)stop - (decimal)start;
                step = step / 240;
                int delta = alarm - start;
                int offset = (int)(delta / step);
                NewAngle = -30 + offset;
            }
            if (StartAnimate != null)
                StartAnimate(this, new EventArgs());
            Vol = (alarm / 1000).ToString();

            if (alarm < SensorModel.Model.NormalMin || alarm > SensorModel.Model.NormalMax)
                VisAlarm = Visibility.Visible;
            else
                VisAlarm = Visibility.Collapsed;
        }

        int start = 0;
        int stop = 0;

        private void PreInitValues()
        {
            if (_sensorModel == null)
                return;

            int step = _sensorModel.Model.NormalMax - SensorModel.Model.NormalMin;
            step = step / 3;
            //min arc
            if (_sensorModel.Model.Min == _sensorModel.Model.NormalMin)
            {
                VisMinArc = Visibility.Collapsed;
                start = _sensorModel.Model.Min;
            }
            else
            {
                VisMinArc = Visibility.Visible;
                start = _sensorModel.Model.NormalMin - step;
            }

            //max arc
            if (_sensorModel.Model.Max == _sensorModel.Model.NormalMax)
            {
                VisMaxArc = Visibility.Collapsed;
            }
            else
            {
                VisMaxArc = Visibility.Visible;
                stop = _sensorModel.Model.NormalMax + step;
            }

            //values
            decimal v0 = (decimal)start / 1000.0m;
            Vol0 = SetDisplayedVol(v0);

            decimal v1 = ((decimal)start + (decimal)step) / 1000.0m;
            Vol1 = SetDisplayedVol(v1);

            decimal v2 = ((decimal)start + 2*(decimal)step) / 1000.0m;
            Vol2 = SetDisplayedVol(v2);

            decimal v3 = ((decimal)start + 3*(decimal)step) / 1000.0m;
            Vol3 = SetDisplayedVol(v3);

            decimal v4 = (decimal)stop / 1000.0m;
            Vol4 = SetDisplayedVol(v4);
        }

        private string SetDisplayedVol(decimal v0)
        {
            string setted = "";
            if (v0 < 0.01m)
                setted = Math.Round(v0, 3).ToString();
            else if (v0 < 0.1m)
                setted = Math.Round(v0, 2).ToString();
            else if (v0 < 10)
                setted = Math.Round(v0, 1).ToString();
            else
                setted = Math.Round(v0).ToString();
            return setted;
        }
    }
}
