using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using KOT.Annotations;
using KOT.DataModel.Handlers;

namespace KOT.DataModel.ViewModel
{
    public class SettingViewModel : INotifyPropertyChanged
    {
        public SettingViewModel() { }

        public string ValueAlarmString
        {
            get
            {
                var res = string.Format("{0:0}%: ", ValueAlarm);
                if (ValueAlarm < 20) return res +"«Похоже, КОТ’а придавил рояль»";
                if (ValueAlarm < 40) return res + "«Ух! Мимо пронеслась фура»";
                if (ValueAlarm < 60) return res + "«Паркуются по звуку бампера»";
                if (ValueAlarm < 80) return res + "«Кто-то задел Ваш автомобиль»";
                return ValueAlarm < 100 ? res + "«Кошка греется на капоте»" : res + "«На Ваше авто упало перышко»";
            }
        }

        public double ValueAlarm
        {
            get { return SettingsHandler.Instance.ValueAlarm; }
            set
            {
                if (value.Equals(SettingsHandler.Instance.ValueAlarm)) return;
                SettingsHandler.Instance.ValueAlarm = value;
                OnPropertyChanged();
                OnPropertyChanged("ValueAlarmString");
            }
        }

        public string Phone
        {
            get { return SettingsHandler.Instance.Phone; }
            set
            {
                if (SettingsHandler.Instance.Phone.Equals(value)) return;
                SettingsHandler.Instance.Phone = value;
            }
        }

        public bool SendAlarmSms
        {
            get { return SettingsHandler.Instance.SndAlarmSms; }
            set
            {
                if (value.Equals(SettingsHandler.Instance.SndAlarmSms)) return;
                SettingsHandler.Instance.SndAlarmSms = value;
                OnPropertyChanged();
            }
        }

        public bool SendToSms
        {
            get { return SettingsHandler.Instance.SendToSms; }
            set
            {
                if (value.Equals(SettingsHandler.Instance.SendToSms)) return;
                SettingsHandler.Instance.SendToSms = value;
                OnPropertyChanged();
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
