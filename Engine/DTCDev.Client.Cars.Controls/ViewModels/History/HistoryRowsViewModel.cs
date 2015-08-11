using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using DTCDev.Client.ViewModel;
using DTCDev.Models.CarsSending.Car;
using DTCDev.Models.Date;

namespace DTCDev.Client.Cars.Controls.ViewModels.History
{
    public class HistoryRowsViewModel : ViewModelBase
    {
        private readonly System.Windows.Threading.Dispatcher _dispatcher;
        private readonly ObservableCollection<HistoryRow> _rowSList = new ObservableCollection<HistoryRow>();
        private HistoryRow _selectedRow;

        public ObservableCollection<HistoryRow> RowsList
        {
            get { return _rowSList; }
        }

        private CollectionViewSource _listHistoryRows;
        public ICollectionView ListHistoryRows
        {
            get
            {
                if (_listHistoryRows != null) return _listHistoryRows.View;
                _listHistoryRows = new CollectionViewSource { Source = RowsList };
                _listHistoryRows.Filter += (o, e) =>
                {
                    var acc = !IsNotNull;
                    var item = (HistoryRow) e.Item;
                    if(item != null && !acc)
                    {
                        acc = !string.IsNullOrEmpty(item.Speed) || item.Data.Any(el => el.Vol > 0);
                    }
                    e.Accepted = acc;
                };
                return _listHistoryRows.View;
            }
        }

        private bool _isNotNull = true;

        public HistoryRowsViewModel(System.Windows.Threading.Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public bool IsNotNull
        {
            get { return _isNotNull; }
            set
            {
                _isNotNull = value;
                ListHistoryRows.Refresh();
                OnPropertyChanged("IsNotNull");
            }
        }

        internal void Update(OBDHistoryDataModel model)
        {
            var slowTask = new Task(delegate
            {
                for (var i = 0; i < RowsList.Count; i++)
                {
                    var item = RowsList[i];
                    var next = i + 1 < RowsList.Count ? RowsList[i + 1].Date : DateTime.Now;
                    var dt = new DateTime(item.Date.Year, item.Date.Month, item.Date.Day);
                    var list =
                        model.Data.Where(o => dt + o.Time.ToTimeSpan() >= item.Date && dt + o.Time.ToTimeSpan() < next);

                    foreach (var el in list)
                    {
                        item.Update(el);
                    }
                }
            });
            slowTask.ContinueWith(delegate
            {
                if (_dispatcher != null)
                    _dispatcher.BeginInvoke(new Action(() =>
                    {
                        ListHistoryRows.Refresh();
                    }));
                
            });
            slowTask.Start();
        }

        public HistoryRow SelectedRow
        {
            get { return _selectedRow; }
            set
            {
                _selectedRow = value;
                OnPropertyChanged("SelectedRow");
            }
        }

        internal void Update(LoadedHistoryRows value)
        {
            if (value == null) return;
            RowsList.Clear();
            foreach (var item in value.Data)
            {
                RowsList.Add(new HistoryRow(item));
            }
            ListHistoryRows.Refresh();
        }
    }

    public class HistoryRow : ViewModelBase
    {
        private readonly CarStateModel _model;
        private readonly List<OBDHistoryDataModel.OBDParam> _data = new List<OBDHistoryDataModel.OBDParam>();
        public int MaxSpeed = 120;
        public int MinSpeed = 90;

        public HistoryRow(CarStateModel item)
        {
            _model = item;
        }

        public double CurentSpeed { get { return _model.Spd / 10.0; } }
        public string Speed { get { return _model.Spd != 0 ? (_model.Spd / 10.0).ToString("##.#") : ""; } }

        public string Time { get { return _model.Date.ToLongTimeString(); } }

        public string Satelite { get { return _model.St != 0 ? _model.St.ToString() : ""; } }
        public DateTime Date { get { return _model.Date; } }
        internal void Update(OBDHistoryDataModel.OBDParam el)
        {
            Data.Add(el);
        }

        public List<OBDHistoryDataModel.OBDParam> Data { get { return _data; } }
    }
}
