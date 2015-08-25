using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTCDev.Client.Cars.Engine.AppLogic;
using DTCDev.Client.Cars.Engine.DisplayModels;
using DTCDev.Client.Cars.Engine.Handlers.Cars;
using DTCDev.Client.ViewModel;
using DTCDev.Models;
using DTCDev.Models.CarsSending.Car;
using Newtonsoft.Json;

namespace DTCDev.Client.Cars.Controls.ViewModels.History
{
    public class HistoryWorkViewModel : ViewModelBase
    {
        private readonly System.Windows.Threading.Dispatcher _dispatcher;
        private string _loadedText;
        private readonly HistoryHandler _handler;
        private readonly ObservableCollection<LoadedHistoryRows> _historyRows = new ObservableCollection<LoadedHistoryRows>();
        private LoadedHistoryRows _selectedRow;
        private readonly HistoryRowsViewModel _tableHistory;
        private double _distanceAll;
        private List<CarStateModel> _dayStates = new List<CarStateModel>();
        private DISP_Car _position;
        private bool _useAccelleration = true;
        private CarStateModel _selectedState;

        /// <summary>
        /// Дата, на каоторой остановилась загрузка
        /// </summary>
        private DateTime _lastLoadedDate = DateTime.Now;

        public HistoryWorkViewModel() { }

        public HistoryWorkViewModel(System.Windows.Threading.Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
            _tableHistory = new HistoryRowsViewModel(dispatcher);
            _tableHistory.PropertyChanged += _tableHistory_PropertyChanged;
            _handler = HistoryHandler.Instance;
            CarSelector.OnCarChanged += CarSelector_OnCarChanged;
            _handler.LoadCompleted += Instance_LoadCompleted;
            _handler.DayRefreshed += Instance_DayRefreshed;
            _handler.LinesLoaded += Instance_LinesLoaded;
            _handler.OBDLoaded += Instance_OBDLoaded;
            _handler.AccLoaded += Instance_AccLoaded;
            _handler.DayChange += _handler_DayChange;
        }

        private void _handler_DayChange(DateTime date)
        {
            var pos = HistoryRows.FirstOrDefault(o => o.Date.Year == date.Year && o.Date.Month == date.Month &&  o.Date.Day == date.Day);
            if (pos != null)
                SelectedHistoryRow = pos;
        }

        void _tableHistory_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!e.PropertyName.Equals("SelectedRow") || _tableHistory.SelectedRow == null) return;
            _handler.OnSetDateTimePosition(_tableHistory.SelectedRow.Date);
        }

        private bool _iswaiting = false;
        public bool Iswaiting
        {
            get { return _iswaiting; }
            set
            {
                if (_iswaiting == value) return;
                _iswaiting = value;
                OnPropertyChanged("Iswaiting");
            }
        }

        public string LoadedText
        {
            get { return _loadedText; }
            set
            {
                _loadedText = value;
                this.OnPropertyChanged("LoadedText");
            }
        }

        /// <summary>
        /// Список загруженных дат
        /// </summary>
        public ObservableCollection<LoadedHistoryRows> HistoryRows
        {
            get { return _historyRows; }
        }

        /// <summary>
        /// Выбранная дата истории
        /// </summary>
        public LoadedHistoryRows SelectedHistoryRow
        {
            get { return _selectedRow; }
            set
            {
                if(_selectedRow == value) return;
                _selectedRow = value;
                Iswaiting = true;
                TableHistory.Update(value);

                OnPropertyChanged("SelectedHistoryRow");
                OnPropertyChanged("TableHistory");

                SortData();
                if (_distanceCheckActive)
                    DistanceSelectedDayChanged();
            }
        }

        /// <summary>
        /// Данные по выбранному дню
        /// </summary>
        public HistoryRowsViewModel TableHistory
        {
            get { return _tableHistory; }
        }

        /// <summary>
        /// Пройденная дистанция
        /// </summary>
        public double DistanceAll
        {
            get { return Math.Round(_distanceAll, 2); }
            set
            {
                _distanceAll = value;
                OnPropertyChanged("DistanceAll");
            }
        }

        /// <summary>
        /// Список навигационных данных
        /// </summary>
        public List<CarStateModel> DayStates
        {
            get { return _dayStates; }
            set
            {
                _dayStates = value;
                OnPropertyChanged("DayStates");
                //if (TablePackageView)
                //    OnPropertyChanged("PackagesList");
            }
        }

        /// <summary>
        /// Текущий авто
        /// </summary>
        public DISP_Car Position
        {

            get { return _position ?? (_position = new DISP_Car()); }
            set
            {
                if (_position == null) return;
                _position = value;
                OnPropertyChanged("Position");
            }
        }

        public CarStateModel SelectedState
        {
            get { return _selectedState; }
            set
            {
                _selectedState = value;
                OnPropertyChanged("SelectedState");
            }
        }

        /// <summary>
        /// Использовать оптимизацию?
        /// </summary>
        public bool UseAccelleration
        {
            get { return _useAccelleration; }
            set
            {
                _useAccelleration = value;
                OnPropertyChanged("UseAccelleration");
            }
        }

        /// <summary>
        /// Модель набора данных траектории
        /// </summary>
        private CarAccHistoryModel _accHistory = new CarAccHistoryModel();
        public CarAccHistoryModel AccHistory
        {
            get { return _accHistory; }
            set
            {
                _accHistory = value;
                OnPropertyChanged("AccHistory");
            }
        }

        /// <summary>
        /// Модель набора данных ускорения
        /// </summary>
        private OBDHistoryDataModel _obdHistory = new OBDHistoryDataModel();
        public OBDHistoryDataModel OBDHistory
        {
            get { return _obdHistory; }
            set
            {
                _obdHistory = value;
                OnPropertyChanged("OBDHistory");
            }
        }

        private LinesDataModel _lines = new LinesDataModel();
        public LinesDataModel Lines
        {
            get { return _lines; }
            set
            {
                _lines = value;
                OnPropertyChanged("Lines");
            }
        }

        private void CarSelector_OnCarChanged(DISP_Car car)
        {
            if (car == null) return;
            Position = new DISP_Car
            {
                Car = new SCarModel
                {
                    CarNumber = car.Car.CarNumber,
                    Id = car.Car.Id
                }
            };
            LoadData();
        }

        /// <summary>
        /// Обновление данных трактории
        /// </summary>
        /// <param name="model"></param>
        private void Instance_AccLoaded(CarAccHistoryModel model)
        {
            var acc = new CarAccHistoryModel { DevID = model.DevID };
            var slowTask = new Task(delegate
            {
                for (var i = 0; i < DayStates.Count; i++)
                {
                    var item = DayStates[i];
                    var next = i + 1 < DayStates.Count ? DayStates[i + 1].Date : DateTime.Now;
                    var list = model.Data.Where(o => o.Date.ToDateTime() >= item.Date && o.Date.ToDateTime() < next);
                    foreach (var el in list)
                        acc.Data.Add(el);
                }
            });
            slowTask.ContinueWith(obj =>
            {
                if (_dispatcher != null)
                    _dispatcher.BeginInvoke(new Action(() =>
                    {
                        AccHistory = acc; Iswaiting = false;
                    }));
            });
            slowTask.Start();
        }

        /// <summary>
        /// Обновление данных OBDII
        /// </summary>
        /// <param name="model"></param>
        private void Instance_OBDLoaded(OBDHistoryDataModel model)
        {
            var obd = new OBDHistoryDataModel { DevID = model.DevID, DT = model.DT };
            var slowTask = new Task(delegate
            {
                for (var i = 0; i < DayStates.Count; i++)
                {
                    var item = DayStates[i];
                    var next = i + 1 < DayStates.Count ? DayStates[i + 1].Date : DateTime.Now;
                    var dt = new DateTime(item.Date.Year, item.Date.Month, item.Date.Day);
                    var list =
                        model.Data.Where(o => dt + o.Time.ToTimeSpan() >= item.Date && dt + o.Time.ToTimeSpan() < next);
                    foreach (var el in list)
                        obd.Data.Add(el);
                }
            });
            slowTask.ContinueWith(obj =>
            {
                if (_dispatcher != null)
                    _dispatcher.BeginInvoke(new Action(() =>
                    {
                        OBDHistory = obd;
                        TableHistory.Update(obd);
                        OnPropertyChanged("TableHistory");
                    }));
            });
            slowTask.Start();
        }


        private void Instance_LinesLoaded(LinesDataModel model)
        {
            Lines = model;
        }

        /// <summary>
        /// Хендлер обновления навигационных данных
        /// </summary>
        /// <param name="day"></param>
        /// <param name="data"></param>
        private void Instance_DayRefreshed(DateTime day, List<CarStateModel> data)
        {
            var f = data.FirstOrDefault();
            if (f == null || f.DevID != Position.ID) return;
            BuildHistoryRow(data, day);
            if (CarSelector.SelectedCar != null)
                CacheRoute(string.Format("[{0}]-{1}-{2}-{3}", CarSelector.SelectedCar.ID, day.Day, day.Month, day.Year), data);
            _lastLoadedDate = day;
            LoadedText = "Обновляю " + day.ToString("dd.MM.yy");
        }

        ////loading data of history completed
        void Instance_LoadCompleted(object sender, EventArgs e)
        {
            LoadedText = "Обновление завершено";
        }

        #region DISTANCE_CHECKER

        private bool _distanceCheckActive = false;
        private int _distanceStart = -1;
        private int _distanceStop = -1;

        private RelayCommand _clickDistanceCommand;
        public RelayCommand ClickDistanceCommand
        {
            get { return _clickDistanceCommand ?? (_clickDistanceCommand = new RelayCommand(a => ClickDistance())); }
        }

        /// <summary>
        /// пользователь выбрал инструмент подсчета пробега
        /// </summary>
        private void ClickDistance()
        {
            _distanceCheckActive = !_distanceCheckActive;
            if (!_distanceCheckActive) return;
            CDStartDay = "Укажите начало";
            CDStopDay = "Укажите окончание";
            VisCDStart = true;
            VisCDStop = false;
            _distanceStart = -1;
            _distanceStop = -1;
        }

        private string _cdStartDay = "Укажите начало";
        private string _cdStopDay = "Укажите окончание";

        public string CDStartDay
        {
            get { return _cdStartDay; }
            set
            {
                _cdStartDay = value;
                this.OnPropertyChanged("CDStartDay");
            }
        }

        public string CDStopDay
        {
            get { return _cdStopDay; }
            set
            {
                _cdStopDay = value;
                this.OnPropertyChanged("CDStopDay");
            }
        }

        private string _totalDistance = "";
        public string TotalDistance
        {
            get { return _totalDistance; }
            set
            {
                _totalDistance = value;
                this.OnPropertyChanged("TotalDistance");
            }
        }

        private bool _visCDStart = true;
        private bool _visCDStop = false;

        public bool VisCDStart
        {
            get { return _visCDStart; }
            set
            {
                _visCDStart = value;
                this.OnPropertyChanged("VisCDStart");
            }
        }

        public bool VisCDStop
        {
            get { return _visCDStop; }
            set
            {
                _visCDStop = value;
                this.OnPropertyChanged("VisCDStop");
            }
        }


        private void DistanceSelectedDayChanged()
        {
            if (SelectedHistoryRow == null) return;
            if (VisCDStart)
            {
                _distanceStart = HistoryRows.IndexOf(SelectedHistoryRow);
                CDStartDay = SelectedHistoryRow.Date.ToString("dd.MM.yy");
            }
            else
            {
                _distanceStop = HistoryRows.IndexOf(SelectedHistoryRow);
                CDStopDay = SelectedHistoryRow.Date.ToString("dd.MM.yy");
            }
            VisCDStart = !VisCDStart;
            VisCDStop = !VisCDStop;
            if (_distanceStart <= -1 || _distanceStop <= -1 ||
                _distanceStart >= HistoryRows.Count() || _distanceStop >= HistoryRows.Count())
                return;
            double dist = 0;
            for (var i = _distanceStart; i <= _distanceStop; i++)
            {
                dist += HistoryRows[i].Mileage;
            }
            TotalDistance = Math.Round(dist, 1).ToString();
        }

        #endregion DISTANCE_CHECKER

        private RelayCommand _loadNext10Command;
        public RelayCommand LoadNext10Command
        {
            get { return _loadNext10Command ?? (_loadNext10Command = new RelayCommand(a => LoadNext10Days())); }
        }

        /// <summary>
        /// Метод запроса истории еще за 10 дней
        /// </summary>
        private void LoadNext10Days()
        {
            if (Position == null || string.IsNullOrEmpty(Position.Car.Id)) return;
            const int days = 10;
            for (var i = 0; i < days; i++)
            {
                GetCache(_lastLoadedDate - TimeSpan.FromDays(i + 1));
            }
            HistoryHandler.Instance.StartLoadHistory(CarSelector.SelectedCar.Car.Id,
                _lastLoadedDate - TimeSpan.FromDays(days + 1), _lastLoadedDate - TimeSpan.FromDays(1), UseAccelleration);
        }

        /// <summary>
        /// Create request for history data
        /// </summary>
        public void LoadData()
        {
            if (Position == null || string.IsNullOrEmpty(Position.Car.Id)) return;
            const int days = 30;
            for (var i = 0; i < 30; i++)
            {
                GetCache(DateTime.Now - TimeSpan.FromDays(i));
            }
            //Загружаем историю за последние 30 дней
            HistoryHandler.Instance.StartLoadHistory(Position.Car.Id,
                DateTime.Now - TimeSpan.FromDays(days), DateTime.Now, UseAccelleration);
        }

        /// <summary>
        /// Кэширование навигационных данных
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        private void CacheRoute(string name, List<CarStateModel> data)
        {
            var slowTask = new Task(delegate
            {
                var myDocs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\M2B\\Cache\\";
                try
                {
                    if (Directory.Exists(myDocs) == false)
                        Directory.CreateDirectory(myDocs);
                    if (File.Exists(myDocs + name))
                        using (var lockFile = new FileStream(myDocs + name,
                                                            FileMode.OpenOrCreate,
                                                            FileAccess.ReadWrite,
                                                            FileShare.Delete))
                        {
                            File.Delete(myDocs + name);
                        }

                    using (var writer = new StreamWriter(myDocs + name))
                    {
                        lock (writer)
                        {
                            var row = JsonConvert.SerializeObject(data);
                            writer.WriteLine(row);
                        }
                    }
                }
                catch (Exception) { }
            });
            slowTask.Start();
        }

        /// <summary>
        /// Получение навигационных данных из кэша
        /// </summary>
        /// <param name="date"></param>
        private void GetCache(DateTime date)
        {
            var slowTask = new Task(delegate
            {
                try
                {
                    var myDocs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\M2B\\Cache\\";
                    var name = string.Format("[{0}]-{1}-{2}-{3}", Position.ID, date.Day, date.Month,
                            date.Year);
                    if (!File.Exists(myDocs + name)) return;
                    List<CarStateModel> cr;
                    using (var reader = new StreamReader(myDocs + name))
                    {
                        lock (reader)
                           cr = JsonConvert.DeserializeObject<List<CarStateModel>>(reader.ReadToEnd());
                    }
                    if (cr == null) return;
                    BuildHistoryRow(cr, date);
                }
                catch { }
            }); slowTask.Start();
        }

        /// <summary>
        /// Обновление навигационных данных
        /// </summary>
        /// <param name="data"></param>
        /// <param name="date"></param>
        private void BuildHistoryRow(List<CarStateModel> data, DateTime date)
        {
            var r = new LoadedHistoryRows { Date = date, Data = data };
            var slowTask = new Task(delegate
            {
                try
                {
                    var dc = new DistanceCalculator();
                    for (var i = 0; i < data.Count() - 1; i++)
                    {
                        r.Mileage += dc.Calculate(data[i], data[i + 1]);
                    }
                }
                catch
                {
                }
                if (!data.Any()) return;
                try
                {
                    r.MiddleSpeed = data.Sum(p => p.Spd) / 10 / data.Count();
                    var moving = data.Where(p => p.Spd > 10).ToList();
                    if (!moving.Any()) return;
                    var hStart = moving.Min(p => p.hh);
                    var minStart = moving.Where(p => p.hh == hStart).Min(p => p.mm);
                    var hStop = moving.Max(p => p.hh);
                    var mStop = moving.Where(p => p.hh == hStop).Max(p => p.mm);

                    r.Start = hStart + ":" + minStart;
                    r.Stop = hStop + ":" + mStop;
                }
                catch
                {
                }
            });
            slowTask.ContinueWith(delegate
            {
                if (_dispatcher != null)
                    _dispatcher.BeginInvoke(new Action(() =>
                    {
                        var item = HistoryRows.OrderBy(s => s.Date)
                                .LastOrDefault(o => o.StringDate.Equals(r.StringDate) || o.Date < r.Date);
                        if (item != null)
                        {
                            var indx = HistoryRows.IndexOf(item);
                            //var selected = HistoryRows[indx].Equals(SelectedHistoryRow);
                            DistanceAll -= HistoryRows[indx].Mileage;
                            HistoryRows[indx] = r;
                            //if (selected) SelectedHistoryRow = r;
                            DistanceAll += r.Mileage;
                        }
                        else
                        {
                            HistoryRows.Add(r);
                            DistanceAll += r.Mileage;
                        }
                    }));
            });
            slowTask.Start();
        }

        /// <summary>
        /// Запрос данных для выбранного дня
        /// </summary>
        private void SortData()
        {
            if (SelectedHistoryRow == null) return;
            DayStates = SelectedHistoryRow.Data;
            _handler.OnDayStateChange(SelectedHistoryRow.Data);
            //SortDataByDate(true);
            AccHistory = null;
            OBDHistory = null;
            _handler.StartLoadDayLines(Position.Car.Id, SelectedHistoryRow.Date);
            _handler.StartLoadOBD(Position.Car.Id, SelectedHistoryRow.Date);
            _handler.StartLoadAcc(Position.Car.Id, SelectedHistoryRow.Date);

        }

        public override void OnPropertyChanged(string propertyName)
        {
            if (_dispatcher != null)
                _dispatcher.BeginInvoke(new Action(() =>
                {
                    base.OnPropertyChanged(propertyName);
                }));
        }
    }
}
