using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using KOT.Annotations;
using KOT.DataModel.Handlers;
using KOT.DataModel.Model;

namespace KOT.DataModel.ViewModel
{
    public class AlarmViewModel : INotifyPropertyChanged
    {
        public AlarmViewModel()
        {

        }
        private const string AlarmHeader = "Удар!";
        private const string LightsOnHeader = "Свет!";
        private const string DoorClosedHeader = "Двери!";
        private const string EvacuationHeader = "Эвакуация!";

        private const string AlarmMsg = "Вашу машину кто-то стукнул.";
        private const string LightsOnMsg = "Уходя тушите свет.";
        private const string DoorClosedMsg = "КОТ не трамвай, двери надо закрывать.";
        private const string EvacuationMsg = "Мы абсолютно уверены, что вашего КОТ`а пытаются эвакуировать в неизвестном направлении.";

        private CarAlarmsState _model;
        private string _header;
        private string _msg;

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

        internal void Update(string propertyName)
        {
            //if (string.IsNullOrEmpty(propertyName)) return;
            switch (propertyName)
            {
                case "AlarmLevel":
                    Header = AlarmHeader;
                    Msg = AlarmMsg;
                    break;
                case "IsLightsOn": 
                    Header = LightsOnHeader;
                    Msg = LightsOnMsg;
                    break;
                case "IsDoorClosed": 
                    Header = DoorClosedHeader;
                    Msg = DoorClosedMsg;
                    break;
                case "IsEvacuation": 
                    Header = EvacuationHeader;
                    Msg = EvacuationMsg;
                    break;
            }
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
