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
            _starTime = _startDate;
            _endTime = DateTime.Now;
        }

        public delegate void SourceChengedHandler(string property);
        public event SourceChengedHandler SourceChenged;
        protected virtual void OnSourceChenged(string property)
        {
            if (SourceChenged != null) SourceChenged(property);
        }

        private string CarId { get { return CarsHandler.SelectedCar.DID; } }
        private DrivingStyle _styleDriver = new DrivingStyle();
        private DateTime _current = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        private DateTime _startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        private DateTime _endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        private readonly ObservableCollection<TripAdvisorViewModel> _listTrip = new ObservableCollection<TripAdvisorViewModel>();
        public ObservableCollection<TripAdvisorViewModel> ListTrip { get { return _listTrip; } }

        private readonly ObservableCollection<TimeModel> _listStartPoint = new ObservableCollection<TimeModel>();
        public ObservableCollection<TimeModel> ListStartPoint { get { return _listStartPoint; } }

        private readonly ObservableCollection<TimeModel> _listEndPoint = new ObservableCollection<TimeModel>();
        public ObservableCollection<TimeModel> ListEndPoint { get { return _listEndPoint; } }


        public static DrivingStyle DriverStyle { get { return Instance._styleDriver; } }

        public static DateTime StartDate
        {
            get { return Instance._startDate; }
            set
            {
                Instance._startDate = value;
                UpdateSource("StartDate");
            }
        }

        public static DateTime EndDate
        {
            get { return Instance._endDate; }
            set
            {
                Instance._endDate = value;
                UpdateSource("EndDate");
            }
        }

        private DateTime _starTime;
        private DateTime _endTime;

        private async Task UpdateSourceAsync(string property = "")
        {
            ListTrip.Clear();

            #region DesignMode

            if (DesignMode.DesignModeEnabled)
            {
                ListTrip.Add(new TripAdvisorViewModel(new TripAdvisorModel
                {
                    DID = "1",
                    CurrentDistance = 150,
                    TripTime = 256,
                    MedianSpeed = 65,
                }));
                ListTrip.Add(new TripAdvisorViewModel(new TripAdvisorModel
                {
                    DID = "1",
                    CurrentDistance = 180,
                    TripTime = 298,
                    MedianSpeed = 78,
                }));
                ListTrip.Add(new TripAdvisorViewModel(new TripAdvisorModel
                {
                    DID = "1",
                    CurrentDistance = 150,
                    TripTime = 256,
                    MedianSpeed = 65,
                }));
                ListTrip.Add(new TripAdvisorViewModel(new TripAdvisorModel
                {
                    DID = "1",
                    CurrentDistance = 180,
                    TripTime = 298,
                    MedianSpeed = 78,
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

            ReciveMessageModel res = null;
            var request = string.Empty;
            switch (property)
            {
                case "StartDate":
                    ListStartPoint.Clear();
                    request =
                        JsonConvert.SerializeObject(new PointsRequestModel
                        {
                            Date = new DateDataModel(StartDate),
                            DevID = CarId
                        });
                    res = await TcpConnection.Send("BN" + request);
                    break;
                case "EndDate":
                    ListEndPoint.Clear();
                    request =
                        JsonConvert.SerializeObject(new PointsRequestModel
                        {
                            Date = new DateDataModel(EndDate),
                            DevID = CarId
                        });
                    res = await TcpConnection.Send("BN" + request);
                    break;
                default:
                    request =
                        JsonConvert.SerializeObject(new TripRequestModel
                        {
                            Start = new DateTimeDataModel(_starTime),
                            Stop = new DateTimeDataModel(_endTime),
                            devID = CarId
                        });
                    res = await TcpConnection.Send("BM" + request);
                    break;
            }
            if (res == null) return;
            if (!string.IsNullOrEmpty(res.Msg))
                await Split(res.Fx, res.Msg, property.Equals("EndDate"));
            OnSourceChenged(property);
        }

        internal async static void UpdateSource(string property = "")
        {
            await Instance.UpdateSourceAsync(property);
        }

        internal async static void UpdateSource(DateTime start, DateTime end)
        {
            Instance._starTime = start;
            Instance._endTime = end;
            await Instance.UpdateSourceAsync("Model");
        }


        internal async static Task GetDriverStile()
        {
            await Instance.GetDriverStileAsync();

        }

        private async Task Split(char fx, string msg, bool fillEnd = false)
        {
            try
            {
                if (fx == 'N' || fx == 'n')
                    foreach (var el in JsonConvert.DeserializeObject<TimeModel[]>(msg).Where(el => ListStartPoint.IndexOf(el) < 0))
                        if (fillEnd) ListEndPoint.Add(el);
                        else ListStartPoint.Add(el);
                if (fx == 'M' || fx == 'm')
                    TripModel = (JsonConvert.DeserializeObject<TripAdvisorModel[]>(msg)).FirstOrDefault(o => o.DID.Equals(CarId));
                    //foreach (var el in JsonConvert.DeserializeObject<TripAdvisorModel[]>(msg))
                    //{
                    //    ListTrip.Add(new TripAdvisorViewModel(el));
                    //}
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

        public TripAdvisorModel TripModel { get; set; }
    }
}
