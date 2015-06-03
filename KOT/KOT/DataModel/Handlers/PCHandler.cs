using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using KOT.DataModel.Model;
using KOT.DataModel.Network;
using KOT.DataModel.ViewModel;
using Newtonsoft.Json;

namespace KOT.DataModel.Handlers
{
    public class PCHandler
    {
        private static PCHandler _instance;
        public static PCHandler Instance
        {
            get { return _instance ?? (_instance = new PCHandler()); }
        }

        public PCHandler()
        {
            _instance = this;
        }

        public event EventHandler SourceChenged;
        protected virtual void OnSourceChenged()
        {
            EventHandler handler = SourceChenged;
            if (handler != null) handler(this, EventArgs.Empty);
        }
        private string CarId { get { return CarsHandler.SelectedCar.DID; } }
        private DrivingStyle _styleDriver = new DrivingStyle();
        private DateTime _current = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        private DateTime _startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        private DateTime _endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        private readonly ObservableCollection<TripAdvisorViewModel> _listTrip = new ObservableCollection<TripAdvisorViewModel>();
        public ObservableCollection<TripAdvisorViewModel> ListTrip { get { return _listTrip; } }

        public static DrivingStyle DriverStyle { get { return Instance._styleDriver; } }

        public static DateTime StartDate
        {
            get { return Instance._startDate; }
            set
            {
                Instance._startDate = value;
                UpdateSource();
            }
        }

        public static DateTime EndDate
        {
            get { return Instance._endDate; }
            set
            {
                Instance._endDate = value;
                UpdateSource();
            }
        }

        private async Task UpdateSourceAsync()
        {
            ListTrip.Clear();

            #region DesignMode

            if (DesignMode.DesignModeEnabled)
            {
                ListTrip.Add(new TripAdvisorViewModel(new TripAdvisorModel
                {
                    DID = "1",
                    StartEng = new DateTimeDataModel(_current.AddHours(4).AddMinutes(45)),
                    EndEng = new DateTimeDataModel(_current.AddHours(5).AddMinutes(15)),
                    CurrentDistance = 150,
                    TripTime = 256,
                    MedianSpeed = 65,
                    EngCurrentDistance = 50,
                    EngMedianSpeed = 80,
                    EngTripTime = 150,
                }));
                ListTrip.Add(new TripAdvisorViewModel(new TripAdvisorModel
                {
                    DID = "1",
                    StartEng = new DateTimeDataModel(_current.AddHours(8).AddMinutes(50)),
                    EndEng = new DateTimeDataModel(_current.AddHours(9).AddMinutes(20)),
                    CurrentDistance = 180,
                    TripTime = 298,
                    MedianSpeed = 78,
                    EngCurrentDistance = 60,
                    EngMedianSpeed = 70,
                    EngTripTime = 180,
                }));
                ListTrip.Add(new TripAdvisorViewModel(new TripAdvisorModel
                {
                    DID = "1",
                    StartEng = new DateTimeDataModel(_current.AddHours(-32).AddMinutes(45)),
                    EndEng = new DateTimeDataModel(_current.AddHours(-32).AddMinutes(15)),
                    CurrentDistance = 150,
                    TripTime = 256,
                    MedianSpeed = 65,
                    EngCurrentDistance = 50,
                    EngMedianSpeed = 80,
                    EngTripTime = 150,
                }));
                ListTrip.Add(new TripAdvisorViewModel(new TripAdvisorModel
                {
                    DID = "1",
                    StartEng = new DateTimeDataModel(_current.AddHours(-38).AddMinutes(50)),
                    EndEng = new DateTimeDataModel(_current.AddHours(-36).AddMinutes(20)),
                    CurrentDistance = 180,
                    TripTime = 298,
                    MedianSpeed = 78,
                    EngCurrentDistance = 60,
                    EngMedianSpeed = 70,
                    EngTripTime = 180,
                }));
                _styleDriver = new DrivingStyle
                {
                    DID = "1",
                    CurrentDrivingStyle = 67,
                    CurrentEcoStyle = 89,
                    TodayDrivingScore = 58,
                    TodayEcoScore = 68,
                    TotalDrivingScore = 68,
                    TotalEcoScore = 98
                };
            }

            #endregion

            var res = await TcpConnection.Send("BI" + CarId);
            if (!string.IsNullOrEmpty(res.Msg))
                await Split(res.Fx, res.Msg);
            OnSourceChenged();
        }

        internal async static void UpdateSource()
        {
            await Instance.UpdateSourceAsync();
        }

        internal async static Task GetDriverStile()
        {
            await Instance.GetDriverStileAsync();

        }

        private async Task Split(char fx, string msg)
        {
            try
            {
                if (fx == 'I' || fx == 'i')
                    foreach (var el in JsonConvert.DeserializeObject<TripAdvisorModel[]>(msg))
                    {
                        ListTrip.Add(new TripAdvisorViewModel(el));
                    }
                if (fx == 'H' || fx == 'h')
                {
                    var res = JsonConvert.DeserializeObject<DrivingStyle[]>(msg);
                    _styleDriver = res.FirstOrDefault(o => o.DID.Equals(CarId));
                }
            }
            catch (Exception)
            {

            }
        }

        private async Task GetDriverStileAsync()
        {
            if (DesignMode.DesignModeEnabled)
            _styleDriver = new DrivingStyle
            {
                DID = "1",
                CurrentDrivingStyle = 67,
                CurrentEcoStyle = 89,
                TodayDrivingScore = 58,
                TodayEcoScore = 68,
                TotalDrivingScore = 68,
                TotalEcoScore = 98
            };
            var res = await TcpConnection.Send("BH" + CarId);
            if (!string.IsNullOrEmpty(res.Msg))
                await Split(res.Fx, res.Msg);
        }
    }
}
