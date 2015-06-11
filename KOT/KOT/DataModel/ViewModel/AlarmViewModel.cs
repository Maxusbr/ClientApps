using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using KOT.Annotations;
using KOT.DataModel.Handlers;

namespace KOT.DataModel.ViewModel
{
    public class AlarmViewModel : INotifyPropertyChanged
    {
        public AlarmViewModel()
        {

        }

        public AlarmViewModel(string propertyName, DateTime dt)
        {
            _selectedProperty = propertyName;
            Update(propertyName);
            TimeRecive = dt;
        }
        const string Link = "ms-appx:///Assets/drawable-xxhdpi/{0}.png";
        private string _header;
        private string _msg;
        private bool _viewDetails;
        private readonly string _selectedProperty;

        //Уровень удара – 0 – нет, 1- слабое, 2 – среднее, 3 - сильное
        public int AlarmLevel { get; set; }
        //Включен ли свет 0 – нет, 1 - да
        public int IsLightsOn { get; set; }
        //Заперта ли дверь 0 – нет, 1 - да
        public int IsDoorClosed { get; set; }
        //Эвакуация 0 – нет, 1 - да
        public int IsEvacuation { get; set; }

        public string Header
        {
            get { return _header; }
            set
            {
                if (value == _header) return;
                _header = value;
                OnPropertyChanged();
            }
        }

        public string Msg
        {
            get { return _msg; }
            set
            {
                if (value == _msg) return;
                _msg = value;
                OnPropertyChanged();
            }
        }

        public DateTime TimeRecive { get; set; }

        public string Date { get { return TimeRecive.ToString("d"); } }

        public string Time { get { return TimeRecive.ToString("t"); } }

        internal void Update(string propertyName)
        {
            //if (string.IsNullOrEmpty(propertyName)) return;
            switch (propertyName)
            {
                case "AlarmLevel":
                    Header = AlarmHandler.AlarmHeader;
                    Msg = AlarmHandler.AlarmMsg;
                    break;
                case "IsLightsOn":
                    Header = AlarmHandler.LightsOnHeader;
                    Msg = AlarmHandler.LightsOnMsg;
                    break;
                case "IsDoorClosed":
                    Header = AlarmHandler.DoorClosedHeader;
                    Msg = AlarmHandler.DoorClosedMsg;
                    break;
                case "IsEvacuation":
                    Header = AlarmHandler.EvacuationHeader;
                    Msg = AlarmHandler.EvacuationMsg;
                    break;
            }
        }

        public bool ViewDetails
        {
            get { return _viewDetails; }
            set
            {
                if (value.Equals(_viewDetails)) return;
                _viewDetails = value;
                OnPropertyChanged();
                OnPropertyChanged("ImageSource");
            }
        }

        public ImageSource ImageSource
        {
            get { return GetImage(); }
        }

        private ImageSource GetImage()
        {
            var res = new Uri(string.Format(Link, GetImageName()));
            return new BitmapImage(res);
        }

        string GetImageName()
        {
            switch (_selectedProperty)
            {
                case "AlarmLevel":
                    return ViewDetails ? "ic_hint_alarm_48dp" : "ic_hint_48dp";
                case "IsLightsOn":
                    return ViewDetails ? "ic_phara_alarm_48dp" : "ic_phara_48dp";
                case "IsDoorClosed":
                    return ViewDetails ? "ic_door_alarm_48dp" : "ic_door_48dp";
                case "IsEvacuation":
                    return ViewDetails ? "ic_evacution_alarm_48dp" : "ic_evacution_48dp";
            }
            return "";
        }

        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion


    }
}
