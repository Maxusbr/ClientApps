using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using DTCDev.Models.DeviceSender.DISP;

namespace DTCDev.Client.Sensors.Discret
{
    public class DiscretValuePresenter : BaseViewModel
    {

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
                if (_sensorModel.State != null)
                    SetValue(_sensorModel.State.Vol);
            }
        }

        void _sensorModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "State")
            {
                SetValue(_sensorModel.State.Vol);
            }
        }


        public void SetValue(int alarm)
        {
            
                if (alarm==0)
                    VisAlarm = Visibility.Collapsed;
                else
                    VisAlarm = Visibility.Visible;
            
        }
    }
}
