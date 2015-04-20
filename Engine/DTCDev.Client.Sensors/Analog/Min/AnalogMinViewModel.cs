using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using DTCDev.Models.DeviceSender.DISP;

namespace DTCDev.Client.Sensors.Analog.Min
{
    class AnalogMinViewModel : BaseViewModel 
    {
        DevicePresenter.Sensor _model;

        public DevicePresenter.Sensor Model
        {
            get { return _model; }
            set
            {
                _model = value;
                InitValues();
            }
        }

        SensorsTypeEnum.SensorsMode _displayMode = SensorsTypeEnum.SensorsMode.MIN;
        public SensorsTypeEnum.SensorsMode DisplayMode
        {
            get { return _displayMode; }
            set { _displayMode = value; }
        }


        private Visibility _alarmVis = Visibility.Collapsed;
        public Visibility AlarmVisibility
        {
            get { return _alarmVis; }
            set
            {
                _alarmVis = value;
                this.OnPropertyChanged("AlarmVisibility");
            }
        }

        private Visibility _upVis = Visibility.Collapsed;
        public Visibility UpVisibility
        {
            get { return _upVis; }
            set
            {
                _upVis = value;
                this.OnPropertyChanged("UpVisibility");
            }
        }

        private Visibility _bottomVis = Visibility.Collapsed;
        public Visibility BottomVisibility
        {
            get { return _bottomVis; }
            set
            {
                _bottomVis = value;
                this.OnPropertyChanged("BottomVisibility");
            }
        }

        private string _displayValue = "";
        public string DisplayValue
        {
            get { return _displayValue; }
            set
            {
                _displayValue = value;
                this.OnPropertyChanged("DisplayValue");
            }
        }

        private int _displayWidth = 0;
        public int DisplayWidth
        {
            get { return _displayWidth; }
            set
            {
                _displayWidth = value;
                this.OnPropertyChanged("DisplayWidth");
            }
        }



        private void InitValues()
        {
            if (Model == null)
                return;
            Model.PropertyChanged += Model_PropertyChanged;
            Model.Updated += Model_Updated;
            CalcData();
        }

        void Model_Updated(object sender, EventArgs e)
        {
            CalcData();
        }

        void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "State")
            {
                CalcData();
            }
        }

        private void CalcData()
        {
            if (Model == null)
                return;
            if (Model.State == null)
                return;
            int state = Model.State.Vol;
            if (state >= Model.Model.NormalMin && state <= Model.Model.NormalMax)
            {
                UpVisibility = Visibility.Collapsed;
                BottomVisibility = Visibility.Collapsed;
                AlarmVisibility = Visibility.Collapsed;
            }
            else
            {
                AlarmVisibility = Visibility.Visible;
                if (state < Model.Model.NormalMin)
                {
                    BottomVisibility = Visibility.Visible;
                    UpVisibility = Visibility.Collapsed;
                }
                else
                {
                    BottomVisibility = Visibility.Collapsed;
                    UpVisibility = Visibility.Visible;
                }
            }
            if (_displayMode == SensorsTypeEnum.SensorsMode.HIST)
            {
                DisplayWidth = 50;
                decimal vol = Model.State.Vol / 1000.0m;
                string sv = vol.ToString();
                if (sv.Length > 5)
                    sv = sv.Substring(0, 5);
                DisplayValue = sv;
            }
            else
            {
                decimal vol = Model.State.Vol / 1000.0m;
                string sv = vol.ToString();
                if (sv.Length > 3)
                    sv = sv.Substring(0, 3);
                DisplayValue = sv;
            }
        }
    }
}
