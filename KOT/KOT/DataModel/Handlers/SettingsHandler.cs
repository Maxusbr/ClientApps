using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KOT.DataModel.Model;
using KOT.DataModel.Network;
using Newtonsoft.Json;

namespace KOT.DataModel.Handlers
{
    class SettingsHandler
    {
        private static SettingsHandler _instance;
        private double _valueAlarm = 60;
        private DateTime _dateEndService = DateTime.Now.AddMonths(6);
        private string _phone = "X XXX XXX XX XX";
        private string _mail = "XXXXX@XXX.XXX";
        private bool _sndAlarmSms = true;
        private bool _sendToSms = true;

        public static SettingsHandler Instance
        {
            get { return _instance ?? (_instance = new SettingsHandler()); }
        }

        public SettingsHandler()
        {
            _instance = this;
            Init();
        }

        private async void Init()
        {
            await GetSetting();
        }

        public double ValueAlarm
        {
            get { return _valueAlarm; }
            set { _valueAlarm = value; }
        }

        public DateTime DateEndService
        {
            get { return _dateEndService; }
            set { _dateEndService = value; }
        }

        public string Phone
        {
            get { return _phone; }
            set { _phone = value; }
        }

        public string Mail
        {
            get { return _mail; }
            set { _mail = value; }
        }

        public bool SndAlarmSms
        {
            get { return _sndAlarmSms; }
            set { _sndAlarmSms = value; }
        }

        public bool SendToSms
        {
            get { return _sendToSms; }
            set { _sendToSms = value; }
        }

        private string CarId { get { return CarsHandler.SelectedCar.DID; } }

        public async static Task GetSetting()
        {
            await Instance.GetSettingAsync();
        }

        private async Task GetSettingAsync()
        {

            var res = await TcpConnection.Send("BC" + CarId);
            if (!string.IsNullOrEmpty(res.Msg))
                await Split(res.Msg);
        }

        private async Task Split(string msg)
        {
            try
            {
                var res = JsonConvert.DeserializeObject<CarListWorkDataModel>(msg);
                if (res == null || string.IsNullOrEmpty(res.Phone)) return;
                Phone = res.Phone;
            }
            catch (Exception)
            {

            }
        }
    }
}
