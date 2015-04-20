using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DTCDev.Client.ViewModel;
using DTCDev.Models.CarsSending.Car;
using DTCDev.Models;
using DTCDev.Models.DeviceSender.DISP;
using System.Windows.Controls;
using DTCDev.Client.Sensors;

namespace DTCDev.Client.Cars.Controls.ViewModels.History
{
    public class PackageViewModel : ViewModelBase
    {
        private string _strDate;
        private double _speed;
        private double _cntSatelete;
        private int _gsmLevel;

        public PackageViewModel(CarStateModel model)
        {
            if (Sensors == null)
                Sensors = new ObservableCollection<UserControl>();
            StrDate = string.Format("{0:00}.{1:00}.{2:00} {3:00}:{4:00}:{5:00}",
                model.dd, model.Mnth, model.yy, model.hh, model.mm, model.ss);
            Speed = model.Spd / 10.0;
            CntSatelete = model.St;
            GSM_Level = model.GSM;
        }

        public PackageViewModel(CarStateModel model, LinesDataModel.LineRow row, List<DevicePresenter.Sensor> sensors)
        {
            if (Sensors == null)
                Sensors = new ObservableCollection<UserControl>();
            StrDate = string.Format("{0:00}.{1:00}.{2:00} {3:00}:{4:00}:{5:00}",
                model.dd, model.Mnth, model.yy, model.hh, model.mm, model.ss);
            Speed = model.Spd / 10.0;
            CntSatelete = model.St;
            GSM_Level = model.GSM;
            FillSensors(row, sensors);
        }

        public string StrDate
        {
            get { return _strDate; }
            set
            {
                _strDate = value;
                this.OnPropertyChanged("StrDate");
            }
        }

        public double Speed
        {
            get { return _speed; }
            set
            {
                _speed = value;
                this.OnPropertyChanged("Speed");
            }
        }

        public double CntSatelete
        {
            get { return _cntSatelete; }
            set
            {
                _cntSatelete = value;
                this.OnPropertyChanged("CntSatelete");
            }
        }

        public int GSM_Level
        {
            get { return _gsmLevel; }
            set
            {
                _gsmLevel = value;
                this.OnPropertyChanged("GSM_Level");
            }
        }

        public ImageSource Satelete
        {
            get
            {
                return CntSatelete < 5 ?
                    new BitmapImage(new Uri("/DTCDev.Client.Cars.Controls;component/Assets/Content/Images/gpsor.png",
                        UriKind.Relative)) :
                    new BitmapImage(new Uri("/DTCDev.Client.Cars.Controls;component/Assets/Content/Images/gps.png",
                        UriKind.Relative));
            }
        }

        public ImageSource GSM
        {
            get
            {
                if (GSM_Level == 0)
                    return new BitmapImage(new Uri("/DTCDev.Client.Cars.Controls;component/Assets/Content/Images/off.png",
                       UriKind.Relative));
                if (GSM_Level < 30)
                    return new BitmapImage(new Uri("/DTCDev.Client.Cars.Controls;component/Assets/Content/Images/onone.png",
                        UriKind.Relative));
                if (GSM_Level >= 30 && GSM_Level < 60)
                    return new BitmapImage(new Uri("/DTCDev.Client.Cars.Controls;component/Assets/Content/Images/ontw.png",
                        UriKind.Relative));
                return new BitmapImage(new Uri("/DTCDev.Client.Cars.Controls;component/Assets/Content/Images/onth.png",
                    UriKind.Relative));
            }
        }

        public ImageSource SpeedImg
        {
            get
            {
                return Speed < 100 ?
                    new BitmapImage(new Uri("/DTCDev.Client.Cars.Controls;component/Assets/Content/Images/speed.png",
                        UriKind.Relative)) :
                    new BitmapImage(new Uri("/DTCDev.Client.Cars.Controls;component/Assets/Content/Images/speedor.png",
                        UriKind.Relative));
            }
        }

        private void FillSensors(LinesDataModel.LineRow row, List<DevicePresenter.Sensor> sensors)
        {
            //SensorLocator locator = new SensorLocator();
            //foreach (var item in sensors)
            //{
            //    decimal scale = item.Model.Max / 1000.0m;
            //    DTCDev.Models.DeviceSender.DISP.DevicePresenter.Sensor nSens = new DTCDev.Models.DeviceSender.DISP.DevicePresenter.Sensor
            //    {
            //        Model = new DTCDev.Models.DeviceSender.DeviceSensorsModel
            //        {
            //            id = item.Model.id,
            //            IsAnalog = item.Model.IsAnalog,
            //            IsInput = item.Model.IsInput,
            //            Max = item.Model.Max / 1000,
            //            Min = item.Model.Min / 1000,
            //            Name = item.Model.Name,
            //            NormalMax = item.Model.NormalMax,
            //            NormalMin = item.Model.NormalMin,
            //            Port = item.Model.Port,
            //            PrType = item.Model.PrType
            //        },
            //        State = new DTCDev.Models.DeviceSender.SensorState
            //        {
            //            id = 0,
            //            Vol = (int)((decimal)row.Values[item.Model.Port - 1] / scale) * 1000
            //        }
            //    };
            //    UserControl control = locator.GetSensor(SensorsTypeEnum.SensorsMode.HIST, nSens);
            //    if (control != null)
            //        Sensors.Add(control);
            //}
        }


        public ObservableCollection<UserControl> Sensors { get; set; }

    }
}
