using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using KOT.DataModel.Model;
using KOT.DataModel.Network;
using Newtonsoft.Json;

namespace KOT.DataModel.Handlers
{
    public class AlarmHandler
    {
        private static AlarmHandler _instance;
        private CarAlarmsState _curentModel;

        public static AlarmHandler Instance
        {
            get { return _instance ?? (_instance = new AlarmHandler()); }
        }

        public AlarmHandler()
        {
            _instance = this;
            LoopUpdate();
        }

        public event PropertyChangedEventHandler Alarm;
        protected virtual void OnAlarm(string propertyName)
        {
            if (Alarm != null) Alarm(this, new PropertyChangedEventArgs(propertyName));
        }

        public CarAlarmsState CurentModel
        {
            get { return Instance._curentModel; }
            set { Instance._curentModel = value; }
        }

        private string CarId { get { return CarsHandler.SelectedCar.DID; } }

        public static async Task UpdateSource()
        {
            await Instance.UpdateSourceAsync();
        }

        private async Task UpdateSourceAsync()
        {
            var res = await TcpConnection.Send("BE" + CarId);
            if (!string.IsNullOrEmpty(res.Msg))
                await Split(res.Msg);
        }

        async void LoopUpdate()
        {
            while (true)
            {
                await UpdateSourceAsync();
                await Task.Delay(new TimeSpan(0, 0, 10));
            }
        }

        private async Task Split(string msg)
        {
            try
            {
                var res = JsonConvert.DeserializeObject<CarAlarmsState[]>(msg);
                if (res == null) return;
                CurentModel = res.FirstOrDefault(o => o.DID.Equals(CarId));
                if (CurentModel.AlarmLevel > 0 )
                    OnAlarm("AlarmLevel");
                if (CurentModel.IsDoorClosed > 0)
                    OnAlarm("IsDoorClosed");
                if (CurentModel.IsEvacuation > 0)
                    OnAlarm("IsEvacuation");
                if(CurentModel.IsLightsOn>0)
                    OnAlarm("IsLightsOn");
            }
            catch (Exception)
            {

            }
        }

    }
}
