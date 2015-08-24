using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using DTCDev.Client.Cars.Engine.AppLogic;
using DTCDev.Client.Cars.Engine.DisplayModels;
using DTCDev.Client.Cars.Engine.Handlers.Cars;
using DTCDev.Client.ViewModel;
using DTCDev.Models.CarsSending.Car;
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
        private ScaleValuesData _listData;
        private DateTime _selectedDate;
        private int _valueNormal = 90;
        private int _valueWarning = 120;

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
        }

        #region Events

        public delegate void AddControlHandler(string name, ScaleValuesData model);

        public event AddControlHandler AddControl;
        public event EventHandler ClearControls;

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

        /// <summary>
        /// Переключатель отображения графика пробег/скорость
        /// </summary>
        public bool IsWayView
        {
            get { return _isWayView; }
            set
            {
                if(_isWayView == value) return;
                _isWayView = value;
                OnPropertyChanged("IsWayView");
                Recalqulate();
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

        public ScaleValuesData ListData
        {
            get { return _listData; }
            set
            {
                _listData = value;
                OnPropertyChanged("ListData");
            }
        }

        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                _selectedDate = value;
                
            }
        }

        #endregion


        #region Handlers
        protected virtual void OnClearControls()
        {
            if (ClearControls != null) ClearControls.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnAddControl(string name, ScaleValuesData model)
        {
            if (AddControl != null) AddControl.Invoke(name, model);
        }

        private void CarSelector_OnCarChanged(DISP_Car car)
        {
            SelectedCar = car;
        }

        private void Instance_AccLoaded(CarAccHistoryModel model)
        {

        }

        private void Instance_OBDLoaded(OBDHistoryDataModel model)
        {

        }

        private void Instance_DayRefreshed(DateTime day, List<CarStateModel> data)
        {
            var f = data.FirstOrDefault();
            if (f == null || f.DevID != SelectedCar.ID) return;
            var list = new List<ChartValuesData>();
            var slowTask = new Task(delegate
            {
                var dt = new DateTime(day.Year, day.Month, day.Day);
                BuildDataRow(data, dt);
                if(dt.Equals(SelectedDate))
                    list = GetData(IsWayView);
            });
            slowTask.ContinueWith(delegate
            {
                DispatherThreadRun(delegate
                {
                    if(list.Any())
                        ListData = new ScaleValuesData {Data = list, Scale = Scale, UseMaxMin = !IsWayView, MinVal = _valueNormal, MaxVal = _valueWarning };
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
                list = GetData(IsWayView);
            });
            slowTask.ContinueWith(delegate
            {
                DispatherThreadRun(delegate
                {
                    ListData = new ScaleValuesData {Data = list, Scale = Scale, UseMaxMin = !IsWayView, MinVal = _valueNormal, MaxVal = _valueWarning };
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

        private void Recalqulate()
        {
            var list = new List<ChartValuesData>();
            var slowTask = new Task(delegate
            {
                list = GetData(IsWayView);
            });
            slowTask.ContinueWith(delegate
            {
                DispatherThreadRun(delegate
                {
                    ListData = new ScaleValuesData {Data = list, Scale = Scale, UseMaxMin = !IsWayView, MinVal = _valueNormal, MaxVal = _valueWarning };
                });
            });
            slowTask.Start();
        }

        /// <summary>
        /// Формирование массива данных для графика
        /// </summary>
        /// <returns></returns>
        private List<ChartValuesData> GetData(bool getway = true)
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
                    data.AddRange(getway ? GetHourMileage(): GetHourSpeed());
                    break;
                case 5:
                    data.AddRange(getway ? GetDayMileage(): GetDaySpeed());
                    break;
            }
            return data;
        }

        private IEnumerable<ChartValuesData> GetDayMileage()
        {
            return _loadedData.OrderByDescending(o => o.Date).
                Select(item => new ChartValuesData { Date = item.Date, Value = item.Mileage });
        }

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

        private IEnumerable<ChartValuesData> GetDaySpeed()
        {
            return _loadedData.OrderByDescending(o => o.Date).
                Select(item => new ChartValuesData { Date = item.Date, Value = item.Data.Any() ? item.Data.Max(a => a.Spd / 10.0) : 0 });
        }

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

    public class ScaleValuesData
    {
        public ScaleValuesData() {
            Data = new List<ChartValuesData>();
            UseMaxMin = false;
        }
        public List<ChartValuesData> Data { get; set; }
        public int Scale { get; set; }

        public bool UseMaxMin { get; set; }

        public int MaxVal { get; set; }

        public int MinVal { get; set; }
    }
}
