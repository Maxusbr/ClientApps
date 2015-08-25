using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using DTCDev.Client.Cars.Engine.AppLogic;
using DTCDev.Client.Cars.Engine.DisplayModels;
using DTCDev.Client.Cars.Engine.Handlers.Cars;
using DTCDev.Client.ViewModel;
using DTCDev.Models.CarsSending.Car;
using DTCDev.Models.Date;
using Newtonsoft.Json;

namespace DTCDev.Client.Cars.Controls.ViewModels.History
{
    class HistoryChartViewModel : ViewModelBase
    {
        #region Fields

        private readonly HistoryHandler _handler;
        private readonly Dispatcher _dispatcher;
        private bool _isWayView = true;
        private int _scale = 5;
        private DISP_Car _selectedCar;
        private readonly List<LoadedHistoryRows> _loadedData = new List<LoadedHistoryRows>();
        private readonly List<OBDHistoryDataModel> _loadedObdData = new List<OBDHistoryDataModel>();
        private readonly List<CarAccHistoryModel> _loadedAccData = new List<CarAccHistoryModel>();
        private ScaleValuesData _listData;
        private DateTime _selectedDate;
        private int _valueNormal = 90;
        private int _valueWarning = 120;
        private bool _isWaiting;

        #endregion


        public HistoryChartViewModel() { }

        public HistoryChartViewModel(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
            CarSelector.OnCarChanged += CarSelector_OnCarChanged;
            _handler = HistoryHandler.Instance;
            _handler.LoadCompleted += Instance_LoadCompleted;
            _handler.DayRefreshed += Instance_DayRefreshed;
            _handler.OBDLoaded += Instance_OBDLoaded;
            _handler.AccLoaded += Instance_AccLoaded;
            _handler.DayStateChange += _handler_DayStateChange;
        }



        #region Events

        public delegate void AddControlHandler(ScaleValuesData model);
        public event AddControlHandler AddControl;
        protected virtual void OnAddControl(ScaleValuesData model)
        {
            if (AddControl != null) AddControl.Invoke(model);
        }

        public event EventHandler ClearControls;
        protected virtual void OnClearControls()
        {
            if (ClearControls != null) ClearControls.Invoke(this, EventArgs.Empty);
        }

        #endregion


        #region Properties

        public DISP_Car SelectedCar
        {
            get { return _selectedCar ?? (_selectedCar = new DISP_Car()); }
            set
            {
                if (_selectedCar == value) return;
                _selectedCar = value;
                OnPropertyChanged("VisableChart");
                LoadData();
            }
        }

        public bool VisableChart { get { return !string.IsNullOrEmpty(SelectedCar.Car.Id); } }

        public bool IsWaiting
        {
            get { return _isWaiting; }
            set
            {
                _isWaiting = value;
                OnPropertyChanged("IsWaiting");
            }
        }

        /// <summary>
        /// Масштаб отображения графика от 5 до 1 (день/час/30 минут/15 минут/минута)
        /// </summary>
        public int Scale
        {
            get { return _scale; }
            set
            {
                var scale = value;
                if (scale < 1) scale = 1;
                if (scale > 5) scale = 5;
                if (_scale == scale) return;
                _scale = scale;
                OnPropertyChanged("Scale");
                Recalqulate();
            }
        }

        /// <summary>
        /// Данные для пробега
        /// </summary>
        public ScaleValuesData ListData
        {
            get { return _listData; }
            set
            {
                _listData = value;
                OnPropertyChanged("ListData");
            }
        }

        /// <summary>
        /// Выбранная дата
        /// </summary>
        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                if (_selectedDate == value) return;
                _selectedDate = value;
                if (value <= new DateTime(1, 1, 1)) return;
                if (Scale == 5) Scale = 4;
                else Recalqulate();
                //if(!_loadedObdData.Any(o => o.DT.ToDate.Equals(value)))
                _handler.StartLoadOBD(SelectedCar.ID, value);
                _handler.OnDayChange(value);
            }
        }

        #endregion


        #region Handlers
        /// <summary>
        /// Хендлер обработки выбранной даты в истории
        /// </summary>
        /// <param name="list"></param>
        private void _handler_DayStateChange(List<CarStateModel> list)
        {
            var first = list.FirstOrDefault();
            if (first == null || first.Date <= new DateTime(1, 1, 1)) return;
            var dt = first.Date;
            SelectedDate = new DateTime(dt.Year, dt.Month, dt.Day);
        }
        
        /// <summary>
        /// Хендлер обработки выбранного авто
        /// </summary>
        /// <param name="car"></param>
        private void CarSelector_OnCarChanged(DISP_Car car)
        {
            SelectedCar = car;
            Scale = 5;
        }

        /// <summary>
        /// Хендлер обработки данных ACC
        /// </summary>
        /// <param name="model"></param>
        private void Instance_AccLoaded(CarAccHistoryModel model)
        {
            if (_loadedAccData.All(o => o.Date != SelectedDate))
                _loadedAccData.Add(model);

            if (!model.Date.Equals(SelectedDate) || Scale == 5) return;
            var list = new List<ScaleValuesData>();
            var slowTask = new Task(delegate
            {
                list.Add(new ScaleValuesData { Name = "XChart", Scale = Scale, Data = GetAccData("X"), UseMaxMin = true, MinVal = 1, MaxVal = 1.5 });
                list.Add(new ScaleValuesData { Name = "YChart", Scale = Scale, Data = GetAccData("Y"), UseMaxMin = true, MinVal = 1, MaxVal = 1.5 });
                list.Add(new ScaleValuesData { Name = "ZChart", Scale = Scale, Data = GetAccData("Z"), UseMaxMin = true, MinVal = 1, MaxVal = 1.5 });
            });
            slowTask.ContinueWith(delegate
            {
                DispatherThreadRun(delegate
                {
                    foreach (var item in list)
                        OnAddControl(item);
                });
            });
            slowTask.Start();
        }

        /// <summary>
        /// Хендлер обработки данных OBD
        /// </summary>
        /// <param name="data"></param>
        private void Instance_OBDLoaded(OBDHistoryDataModel data)
        {
            ////TODO OBDHistoryDataModel должен возвращать дату. public DateDataModel DT { get; set; }
            //return;
            _loadedObdData.Clear();
            //var exist = _loadedObdData.FirstOrDefault(o => o.DT.ToDate.Equals(data.DT.ToDate));
            //if (exist == null)
            _loadedObdData.Add(data);

            //if (!data.DT.ToDate.Equals(SelectedDate)) return;
            var list = new List<ScaleValuesData>();
            var slowTask = new Task(delegate
            {
                var prms = data.Data.Select(p => p.Code).Distinct();
                list.AddRange(prms.Select(el => new ScaleValuesData { Code = el, Scale = Scale, Data = GetOBDData(el) }));
                if (!_loadedAccData.Any(o => o.Date.Equals(SelectedDate)))
                    _handler.StartLoadAcc(SelectedCar.ID, SelectedDate);
            });
            slowTask.ContinueWith(delegate
            {
                DispatherThreadRun(delegate
                {
                    foreach (var model in list.Where(model => model.Data.Any()))
                        OnAddControl(model);

                });
            });
            slowTask.Start();
        }

        /// <summary>
        /// Хендлер обработки данных для дня day
        /// </summary>
        /// <param name="day"></param>
        /// <param name="data"></param>
        private void Instance_DayRefreshed(DateTime day, List<CarStateModel> data)
        {
            var f = data.FirstOrDefault();
            if (f == null || f.DevID != SelectedCar.ID) return;
            var list = new List<ChartValuesData>();
            var slowTask = new Task(delegate
            {
                var dt = new DateTime(day.Year, day.Month, day.Day);
                BuildDataRow(data, dt);
                if (dt.Equals(SelectedDate))
                    list = GetStaticData(true);
            });
            slowTask.ContinueWith(delegate
            {
                DispatherThreadRun(delegate
                {
                    if (list.Any())
                        ListData = new ScaleValuesData { Data = list, Scale = Scale};
                });
            });
            slowTask.Start();
        }

        private void Instance_LoadCompleted(object sender, EventArgs e)
        {

        }

        #endregion

        #region Metods

        /// <summary>
        /// Формирование списка данных для выбранного авто на 30 дней
        /// </summary>
        public void LoadData()
        {
            if (SelectedCar == null || string.IsNullOrEmpty(SelectedCar.Car.Id)) return;
            const int days = 30;
            var dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var list = new List<ChartValuesData>();
            var slowTask = new Task(delegate
            {
                for (var i = days; i >= 0; i--)
                {
                    GetCache(dt.AddDays(-i));
                }
                _selectedDate = _loadedData.Where(w => w.Data.Any()).Min(o => o.Date);
                list = GetStaticData(true);
            });
            slowTask.ContinueWith(delegate
            {
                DispatherThreadRun(delegate
                {
                    ListData = new ScaleValuesData { Data = list, Scale = Scale};
                });
            });
            slowTask.Start();
        }

        /// <summary>
        /// Получение навигационных данных из кэша
        /// </summary>
        /// <param name="date"></param>
        private void GetCache(DateTime date)
        {
            try
            {
                var myDocs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\M2B\\Cache\\";
                var name = string.Format("[{0}]-{1}-{2}-{3}", SelectedCar.ID, date.Day, date.Month, date.Year);
                if (!File.Exists(myDocs + name)) return;
                List<CarStateModel> cr;

                using (var reader = new StreamReader(myDocs + name))
                {
                    lock (reader)
                        cr = JsonConvert.DeserializeObject<List<CarStateModel>>(reader.ReadToEnd());
                }
                BuildDataRow(cr, date);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Метод обновления данных графиков
        /// </summary>
        private void Recalqulate()
        {
            IsWaiting = true;
            var list = new List<ChartValuesData>();
            var slowTask = new Task(delegate
            {
                list = GetStaticData(true);
            });
            slowTask.ContinueWith(delegate
            {
                DispatherThreadRun(delegate
                {
                    ListData = new ScaleValuesData { Data = list, Scale = Scale };
                    if (Scale == 5) OnClearControls();
                    else AddMoreCharts();
                    IsWaiting = false;
                });
            });
            slowTask.Start();
        }

        /// <summary>
        /// Добавление данных для контроллеров
        /// </summary>
        private void AddMoreCharts()
        {
            var list = new List<ScaleValuesData>();
            var slowTask = new Task(delegate
            {
                list.Add(new ScaleValuesData
                {
                    Name = "speed",
                    Data = GetStaticData(false),
                    UseMaxMin = true,
                    Scale = Scale,
                    MinVal = _valueNormal,
                    MaxVal = _valueWarning
                });
                list.Add(new ScaleValuesData
                {
                    Name = "satelites",
                    Data = GetMoreData(),
                    Scale = Scale,
                });
                //TODO OBDHistoryDataModel должен возвращать дату. public DateDataModel DT { get; set; }
                //return;
                var obd = _loadedObdData.FirstOrDefault();//o => o.DT.ToDate.Equals(SelectedDate));
                if (obd == null) return;
                var prms = obd.Data.Select(p => p.Code).Distinct();
                list.AddRange(prms.Select(el => new ScaleValuesData { Code = el, Scale = Scale, Data = GetOBDData(el) }));
                list.Add(new ScaleValuesData { Name = "XChart", Scale = Scale, Data = GetAccData("X"), UseMaxMin = true, MinVal = 1, MaxVal = 1.5 });
                list.Add(new ScaleValuesData { Name = "YChart", Scale = Scale, Data = GetAccData("Y"), UseMaxMin = true, MinVal = 1, MaxVal = 1.5 });
                list.Add(new ScaleValuesData { Name = "ZChart", Scale = Scale, Data = GetAccData("Z"), UseMaxMin = true, MinVal = 1, MaxVal = 1.5 });
            });
            slowTask.ContinueWith(delegate
            {
                DispatherThreadRun(delegate
                {
                    foreach (var model in list.Where(model => model.Data.Any()))
                        OnAddControl(model);
                });
            });
            slowTask.Start();
        }

        /// <summary>
        /// Формирование массива данных для графика путь/скорость
        /// </summary>
        /// <returns></returns>
        private List<ChartValuesData> GetStaticData(bool getway = true)
        {
            var data = new List<ChartValuesData>();
            switch (Scale)
            {
                case 1:
                    data.AddRange(getway ? GetMinutesMileage(1) : GetMinutesSpeed(1));
                    break;
                case 2:
                    data.AddRange(getway ? GetMinutesMileage(15) : GetMinutesSpeed(15));
                    break;
                case 3:
                    data.AddRange(getway ? GetMinutesMileage(30) : GetMinutesSpeed(30));
                    break;
                case 4:
                    data.AddRange(getway ? GetHourMileage() : GetHourSpeed());
                    break;
                case 5:
                    data.AddRange(getway ? GetDayMileage() : GetDaySpeed());
                    break;
            }
            return data;
        }
        /// <summary>
        /// Дневной график пробега
        /// </summary>
        /// <returns></returns>
        private IEnumerable<ChartValuesData> GetDayMileage()
        {
            return _loadedData.OrderByDescending(o => o.Date).
                Select(item => new ChartValuesData { Date = item.Date, Value = item.Mileage });
        }
        /// <summary>
        /// Часовой график пробега
        /// </summary>
        /// <returns></returns>
        private IEnumerable<ChartValuesData> GetHourMileage()
        {
            var list = new List<CarStateModel>();
            foreach (var item in _loadedData.Where(o => o.Date.Equals(SelectedDate)))
                list.AddRange(item.Data);
            return list.GroupBy(x =>
            {
                var stamp = x.Date;
                stamp = stamp.AddMinutes(-stamp.Minute);
                stamp = stamp.AddMilliseconds(-stamp.Millisecond - 1000 * stamp.Second);
                return stamp;
            })
           .Select(g => new ChartValuesData { Date = g.Key, Value = g.Sum(s => s.Mileage) });
        }
        /// <summary>
        /// Минутные графики пробега
        /// </summary>
        /// <returns></returns>
        private IEnumerable<ChartValuesData> GetMinutesMileage(int minute)
        {
            var list = new List<CarStateModel>();
            foreach (var item in _loadedData.Where(o => o.Date.Equals(SelectedDate)))
                list.AddRange(item.Data);
            return list.GroupBy(x =>
            {
                var stamp = x.Date;
                stamp = stamp.AddMinutes(-(stamp.Minute % minute));
                stamp = stamp.AddMilliseconds(-stamp.Millisecond - 1000 * stamp.Second);
                return stamp;
            })
           .Select(g => new ChartValuesData { Date = g.Key, Value = g.Sum(s => s.Mileage) });
        }
        /// <summary>
        /// Дневной график скорости
        /// </summary>
        /// <returns></returns>
        private IEnumerable<ChartValuesData> GetDaySpeed()
        {
            return _loadedData.OrderByDescending(o => o.Date).
                Select(item => new ChartValuesData { Date = item.Date, Value = item.Data.Any() ? item.Data.Max(a => a.Spd / 10.0) : 0 });
        }
        /// <summary>
        /// Часовой график скорости
        /// </summary>
        /// <returns></returns>
        private IEnumerable<ChartValuesData> GetHourSpeed()
        {
            var list = new List<CarStateModel>();
            foreach (var item in _loadedData.Where(o => o.Date.Equals(SelectedDate)))
                list.AddRange(item.Data);
            return list.GroupBy(x =>
            {
                var stamp = x.Date;
                stamp = stamp.AddMinutes(-stamp.Minute);
                stamp = stamp.AddMilliseconds(-stamp.Millisecond - 1000 * stamp.Second);
                return stamp;
            })
           .Select(g => new ChartValuesData { Date = g.Key, Value = g.Max(s => s.Spd / 10.0) });
        }
        /// <summary>
        /// Минутные графики скорости
        /// </summary>
        /// <returns></returns>
        private IEnumerable<ChartValuesData> GetMinutesSpeed(int minute)
        {
            var list = new List<CarStateModel>();
            foreach (var item in _loadedData.Where(o => o.Date.Equals(SelectedDate)))
                list.AddRange(item.Data);
            return list.GroupBy(x =>
            {
                var stamp = x.Date;
                stamp = stamp.AddMinutes(-(stamp.Minute % minute));
                stamp = stamp.AddMilliseconds(-stamp.Millisecond - 1000 * stamp.Second);
                return stamp;
            })
           .Select(g => new ChartValuesData { Date = g.Key, Value = g.Max(s => s.Spd / 10.0) });
        }
        /// <summary>
        /// Формирование массива данных для графика количества спутников
        /// </summary>
        /// <returns></returns>
        private List<ChartValuesData> GetMoreData()
        {
            var data = new List<ChartValuesData>();
            switch (Scale)
            {
                case 1:
                    data.AddRange(GetMinutesSatelite(1));
                    break;
                case 2:
                    data.AddRange(GetMinutesSatelite(15));
                    break;
                case 3:
                    data.AddRange(GetMinutesSatelite(30));
                    break;
                case 4:
                    data.AddRange(GetHourSatelite());
                    break;
            }
            return data;
        }
        /// <summary>
        /// Часовой график количества спутников
        /// </summary>
        /// <returns></returns>
        private IEnumerable<ChartValuesData> GetHourSatelite()
        {
            var list = new List<CarStateModel>();
            foreach (var item in _loadedData.Where(o => o.Date.Equals(SelectedDate)))
                list.AddRange(item.Data);
            return list.GroupBy(x =>
            {
                var stamp = x.Date;
                stamp = stamp.AddMinutes(-stamp.Minute);
                stamp = stamp.AddMilliseconds(-stamp.Millisecond - 1000 * stamp.Second);
                return stamp;
            })
           .Select(g => new ChartValuesData { Date = g.Key, Value = g.Average(s => s.St) });
        }
        /// <summary>
        /// Минутные графики количества спутников
        /// </summary>
        /// <returns></returns>
        private IEnumerable<ChartValuesData> GetMinutesSatelite(int minute)
        {
            var list = new List<CarStateModel>();
            foreach (var item in _loadedData.Where(o => o.Date.Equals(SelectedDate)))
                list.AddRange(item.Data);
            return list.GroupBy(x =>
            {
                var stamp = x.Date;
                stamp = stamp.AddMinutes(-(stamp.Minute % minute));
                stamp = stamp.AddMilliseconds(-stamp.Millisecond - 1000 * stamp.Second);
                return stamp;
            })
           .Select(g => new ChartValuesData { Date = g.Key, Value = g.Average(s => s.St) });
        }
        /// <summary>
        /// Формирование массива данных для графиков контроллеров OBD
        /// </summary>
        /// <returns></returns>
        private List<ChartValuesData> GetOBDData(string code)
        {
            var data = new List<ChartValuesData>();
            switch (Scale)
            {
                case 1:
                    data.AddRange(GetMinutesOBD(1, code));
                    break;
                case 2:
                    data.AddRange(GetMinutesOBD(15, code));
                    break;
                case 3:
                    data.AddRange(GetMinutesOBD(30, code));
                    break;
                case 4:
                    data.AddRange(GetHourOBD(code));
                    break;
            }
            return data;
        }
        /// <summary>
        /// Часовой график данных OBD
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private IEnumerable<ChartValuesData> GetHourOBD(string code)
        {
            var list = new List<OBDHistoryDataModel.OBDParam>();
            foreach (var item in _loadedObdData)//.Where(o => o.DT.ToDate.Equals(SelectedDate)))
                for (var i = 0; i < 24; i++)
                {
                    var range = item.Data.Where(o => o.Code.Equals(code) && o.Time.H == i).ToList();
                    if (range.Any())
                        list.AddRange(range);
                    else
                        list.Add(new OBDHistoryDataModel.OBDParam { Code = code, Time = new TimeModel { H = i, M = 0, S = 0 } });
                }
            return list.GroupBy(x =>
            {
                var stamp = SelectedDate + x.Time.ToTimeSpan();
                stamp = stamp.AddMinutes(-stamp.Minute);
                stamp = stamp.AddMilliseconds(-stamp.Millisecond - 1000 * stamp.Second);
                return stamp;
            })
           .Select(g => new ChartValuesData { Date = g.Key, Value = g.Average(s => s.Vol) });
        }
        /// <summary>
        /// Минутные графики данных OBD
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private IEnumerable<ChartValuesData> GetMinutesOBD(int minute, string code)
        {
            var list = new List<OBDHistoryDataModel.OBDParam>();
            foreach (var item in _loadedObdData)//.Where(o => o.DT.ToDate.Equals(SelectedDate)))
                for (var i = 0; i < 24; i++)
                {
                    var range = item.Data.Where(o => o.Code.Equals(code) && o.Time.H == i).ToList();
                    if (range.Any())
                        list.AddRange(range);
                    else
                        list.Add(new OBDHistoryDataModel.OBDParam { Code = code, Time = new TimeModel { H = i, M = 0, S = 0 } });
                }
            return list.GroupBy(x =>
            {
                var stamp = SelectedDate + x.Time.ToTimeSpan();
                stamp = stamp.AddMinutes(-(stamp.Minute % minute));
                stamp = stamp.AddMilliseconds(-stamp.Millisecond - 1000 * stamp.Second);
                return stamp;
            })
           .Select(g => new ChartValuesData { Date = g.Key, Value = g.Max(s => s.Vol) });
        }
        /// <summary>
        /// Формирование массива данных для графиков ACC
        /// </summary>
        /// <returns></returns>
        private List<ChartValuesData> GetAccData(string code)
        {
            var data = new List<ChartValuesData>();
            switch (Scale)
            {
                case 1:
                    data.AddRange(GetMinutesAcc(1, code));
                    break;
                case 2:
                    data.AddRange(GetMinutesAcc(15, code));
                    break;
                case 3:
                    data.AddRange(GetMinutesAcc(30, code));
                    break;
                case 4:
                    data.AddRange(GetHourAcc(code));
                    break;
            }
            return data;
        }
        /// <summary>
        /// Часовой график данных ACC
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private IEnumerable<ChartValuesData> GetHourAcc(string code)
        {
            var list = new List<CarAccHistoryModel.AccRow>();
            foreach (var item in _loadedAccData.Where(o => o.Date.Equals(SelectedDate)))
                list.AddRange(item.Data);
            switch (code)
            {
                case "X":
                    return list.GroupBy(x =>
                    {
                        var stamp = x.Date.ToDateTime();
                        stamp = stamp.AddMinutes(-stamp.Minute);
                        stamp = stamp.AddMilliseconds(-stamp.Millisecond - 1000 * stamp.Second);
                        return stamp;
                    })
                   .Select(g => new ChartValuesData { Date = g.Key, Value = g.Max(s => Math.Abs(s.X / 100.0)) });
                case "Y":
                    return list.GroupBy(x =>
                    {
                        var stamp = x.Date.ToDateTime();
                        stamp = stamp.AddMinutes(-stamp.Minute);
                        stamp = stamp.AddMilliseconds(-stamp.Millisecond - 1000 * stamp.Second);
                        return stamp;
                    })
                   .Select(g => new ChartValuesData { Date = g.Key, Value = g.Max(s => Math.Abs(s.Y / 100.0)) });
                case "Z":
                    return list.GroupBy(x =>
                    {
                        var stamp = x.Date.ToDateTime();
                        stamp = stamp.AddMinutes(-stamp.Minute);
                        stamp = stamp.AddMilliseconds(-stamp.Millisecond - 1000 * stamp.Second);
                        return stamp;
                    })
                   .Select(g => new ChartValuesData { Date = g.Key, Value = g.Max(s => Math.Abs(s.Z / 100.0)) });
                default: return new List<ChartValuesData>();
            }
        }
        /// <summary>
        /// Минутные графики данных ACC
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private IEnumerable<ChartValuesData> GetMinutesAcc(int minute, string code)
        {
            var list = new List<CarAccHistoryModel.AccRow>();
            foreach (var item in _loadedAccData.Where(o => o.Date.Equals(SelectedDate)))
                list.AddRange(item.Data);
            switch (code)
            {
                case "X":
                    return list.GroupBy(x =>
                    {
                        var stamp = x.Date.ToDateTime();
                        stamp = stamp.AddMinutes(-(stamp.Minute % minute));
                        stamp = stamp.AddMilliseconds(-stamp.Millisecond - 1000 * stamp.Second);
                        return stamp;
                    })
                   .Select(g => new ChartValuesData { Date = g.Key, Value = g.Max(s => Math.Abs(s.X / 100.0)) });
                case "Y":
                    return list.GroupBy(x =>
                    {
                        var stamp = x.Date.ToDateTime();
                        stamp = stamp.AddMinutes(-(stamp.Minute % minute));
                        stamp = stamp.AddMilliseconds(-stamp.Millisecond - 1000 * stamp.Second);
                        return stamp;
                    })
                   .Select(g => new ChartValuesData { Date = g.Key, Value = g.Max(s => Math.Abs(s.Y / 100.0)) });
                case "Z":
                    return list.GroupBy(x =>
                    {
                        var stamp = x.Date.ToDateTime();
                        stamp = stamp.AddMinutes(-(stamp.Minute % minute));
                        stamp = stamp.AddMilliseconds(-stamp.Millisecond - 1000 * stamp.Second);
                        return stamp;
                    })
                   .Select(g => new ChartValuesData { Date = g.Key, Value = g.Max(s => Math.Abs(s.Z / 100.0)) });
                default: return new List<ChartValuesData>();
            }
        }

        /// <summary>
        /// Формирование массива данных для дня day
        /// </summary>
        /// <param name="data"></param>
        /// <param name="day"></param>
        private void BuildDataRow(List<CarStateModel> data, DateTime day)
        {
            var curentdata = data ?? new List<CarStateModel>();
            var item = new LoadedHistoryRows { Date = day, Data = curentdata };
            var dc = new DistanceCalculator();
            for (var i = 0; i < curentdata.Count() - 1; i++)
            {
                curentdata[i].Mileage = dc.Calculate(curentdata[i], curentdata[i + 1]);
                item.Mileage += curentdata[i].Mileage;
            }
            var el = _loadedData.FirstOrDefault(o => o.Date.Equals(day));
            if (el == null)
                _loadedData.Add(item);
            else
            {
                el.Data = item.Data;
                el.Mileage = item.Mileage;
            }
        }

        #endregion



        #region OnPropertyChanged

        public override void OnPropertyChanged(string propertyName)
        {
            DispatherThreadRun(() => base.OnPropertyChanged(propertyName));
        }

        /// <summary>
        /// Выполнение метода action в потоке приложения
        /// </summary>
        /// <param name="action"></param>
        private void DispatherThreadRun(Action action)
        {
            if (_dispatcher != null)
                _dispatcher.BeginInvoke(action);

        }

        #endregion


    }

    /// <summary>
    /// Модель данных для графика
    /// </summary>
    public class ChartValuesData
    {
        public DateTime Date { get; set; }
        public double Value { get; set; }
    }
    /// <summary>
    /// Модель данных для ChartDataControl
    /// </summary>
    public class ScaleValuesData
    {
        public ScaleValuesData()
        {
            Data = new List<ChartValuesData>();
            UseMaxMin = false;
        }
        public List<ChartValuesData> Data { get; set; }
        public int Scale { get; set; }

        public bool UseMaxMin { get; set; }

        public double MaxVal { get; set; }

        public double MinVal { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }
    }
}
