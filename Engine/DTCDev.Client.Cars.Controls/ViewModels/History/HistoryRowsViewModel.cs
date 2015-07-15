using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using DTCDev.Client.ViewModel;
using DTCDev.Models.CarsSending.Car;

namespace DTCDev.Client.Cars.Controls.ViewModels.History
{
    public class HistoryRowsViewModel : ViewModelBase
    {
        private readonly ObservableCollection<HistoryRow> _rowSList = new ObservableCollection<HistoryRow>();
        public ObservableCollection<HistoryRow> RowsList
        {
            get { return _rowSList; }
        }

        internal void Update(OBDHistoryDataModel model)
        {
            for (var i = 0; i < RowsList.Count; i++)
            {
                var item = RowsList[i];
                var next = i + 1 < RowsList.Count ? RowsList[i + 1].Date : DateTime.Now;
                var dt = new DateTime(item.Date.Year, item.Date.Month, item.Date.Day);
                var list = model.Data.Where(o => dt + o.Time.ToTimeSpan() >= item.Date && dt + o.Time.ToTimeSpan() < next);

                foreach (var el in list)
                {
                    item.Update(el);
                }
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
        }
    }

    public class HistoryRow : ViewModelBase
    {
        private readonly CarStateModel _model;
        private readonly List<OBDHistoryDataModel.OBDParam> _data = new List<OBDHistoryDataModel.OBDParam>();
        public int MaxSpeed = 120;
        public int MinSpeed = 80;

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
