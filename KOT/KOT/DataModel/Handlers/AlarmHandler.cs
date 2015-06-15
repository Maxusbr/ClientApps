using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using KOT.DataModel.Model;
using KOT.DataModel.Network;
using KOT.DataModel.ViewModel;
using Newtonsoft.Json;

namespace KOT.DataModel.Handlers
{
    public class AlarmHandler
    {
        public const string AlarmHeader = "КОТ под Ударом!";
        public const string LightsOnHeader = "Свет!";
        public const string DoorClosedHeader = "Двери!";
        public const string EvacuationHeader = "Эвакуация!";

        public const string AlarmMsg = "Вашу машину кто-то стукнул.";
        public const string LightsOnMsg = "Уходя тушите свет.";
        public const string DoorClosedMsg = "КОТ не трамвай, двери закрывать надо.";
        public const string EvacuationMsg = "Мы абсолютно уверены, что вашего КОТ`а пытаются эвакуировать в неизвестном направлении.";
        private readonly ObservableCollection<AlarmViewModel> _historyAlarmList = new ObservableCollection<AlarmViewModel>(); 
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
            CarsHandler.SelectionChanged += CarsHandler_SelectionChanged;
        }

        async void CarsHandler_SelectionChanged(object sender, EventArgs e)
        {
            PressedAlarmList.Clear();
            await UpdateSourceAsync();
        }

        public event PropertyChangedEventHandler Alarm;

        protected virtual void OnAlarm(string propertyName)
        {
            var res = PressedAlarmList.FirstOrDefault(o => o.Equals(propertyName));
            if(res != null) return;
            HistoryAlarmList.Insert(0, new AlarmViewModel(propertyName, DateTime.Now));
            if (Alarm != null) Alarm(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<AlarmViewModel> HistoryAlarmList { get { return _historyAlarmList; } }

        public CarAlarmsState CurentModel
        {
            get { return Instance._curentModel; }
            set { Instance._curentModel = value; }
        }
        private readonly List<string> _pressedAlarmList = new List<string>();
        public List<string> PressedAlarmList { get { return _pressedAlarmList; } } 

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


        internal void PressedAlarm(string propertyName)
        {
            switch (propertyName)
            {
                case "AlarmLevel":
                    Instance.CurentModel.AlarmLevel = 0;
                    break;
                case "IsLightsOn":
                    Instance.CurentModel.IsLightsOn = 0;
                    break;
                case "IsDoorClosed":
                    Instance.CurentModel.IsDoorClosed = 0;
                    break;
                case "IsEvacuation":
                    Instance.CurentModel.IsEvacuation = 0;
                    break;
            }
            PressedAlarmList.Add(propertyName);
        }

        internal async static void UpdateDate(DateTime start, DateTime end)
        {
            StartDate = start;
            EndDate = end;
            //await UpdateSource();
        }

        public static DateTime StartDate { get; set; }

        public static DateTime EndDate { get; set; }
    }
}
