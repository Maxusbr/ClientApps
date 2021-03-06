﻿using System;
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
    public class HistoryWorkHandler
    {
        private static HistoryWorkHandler _instance;
        private DateTime _startDate;
        private DateTime _endDate;
        private string CarId { get { return CarsHandler.SelectedCar.DID; } }
        public static HistoryWorkHandler Instance
        {
            get { return _instance ?? (_instance = new HistoryWorkHandler()); }
        }

        public HistoryWorkHandler()
        {
            _instance = this;
            _startDate = new DateTime(1, 1, 1);
            _endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        }

        private readonly ObservableCollection<WorkFromHistoryViewModel> _historyWorks = new ObservableCollection<WorkFromHistoryViewModel>();
        private readonly ObservableCollection<WorkFromHistoryViewModel> _historySortedWorks = new ObservableCollection<WorkFromHistoryViewModel>();

        public static ObservableCollection<WorkFromHistoryViewModel> HistorySortedWorks
        {
            get { return Instance._historySortedWorks; }
        }

        public static DateTime StartDate
        {
            get { return Instance._startDate; }
            set { Instance._startDate = value; }
        }

        public static DateTime EndDate
        {
            get { return Instance._endDate; }
            set { Instance._endDate = value; }
        }

        private void UpdateSource()
        {
            HistorySortedWorks.Clear();
            foreach (var el in _historyWorks.Where(o => o.Date >= StartDate && o.Date <= EndDate))
            {
                HistorySortedWorks.Add(el);
            }
        }

        internal static void UpdateDate(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
            Instance.UpdateSource();
        }

        public async static Task UpdateHistory()
        {
            await Instance.UpdateHistoryAsync();
        }

        private async Task UpdateHistoryAsync()
        {
            #region DesignMode

            if (DesignMode.DesignModeEnabled)
            {
                _historyWorks.Add(new WorkFromHistoryViewModel(new CarHistoryWorkReport
                {
                    OrderN = 1,
                    WorkName = "Чистка форсунок",
                    Date = new DateDataModel(DateTime.Now.AddDays(-16)),
                    Cost = 154,
                    Comment = "Comment 2",
                    Distance = 280,
                    Worker = "Бригадир"
                }));
                _historyWorks.Add(new WorkFromHistoryViewModel(new CarHistoryWorkReport
                {
                    OrderN = 1,
                    WorkName = "Замена масла",
                    Date = new DateDataModel(DateTime.Now.AddDays(-52)),
                    Cost = 154,
                    Comment = "Comment 3",
                    Distance = 280,
                    Worker = "Мастер"
                }));
                _historyWorks.Add(new WorkFromHistoryViewModel(new CarHistoryWorkReport
                {
                    OrderN = 1,
                    WorkName = "Замена фильтров",
                    Date = new DateDataModel(DateTime.Now.AddDays(-92)),
                    Cost = 154,
                    Comment = "Comment 4",
                    Distance = 280,
                    Worker = "Пользователь"
                }));
                _historyWorks.Add(new WorkFromHistoryViewModel(new CarHistoryWorkReport
                {
                    OrderN = 1,
                    WorkName = "Чистка форсунок",
                    Date = new DateDataModel(DateTime.Now.AddDays(-125)),
                    Cost = 154,
                    Comment = "Comment 5",
                    Distance = 280,
                    Worker = "Гаражный мастер"
                }));
            }

            #endregion

            _historyWorks.Clear();
            var res = await TcpConnection.Send("BD" + CarId);
            if (!string.IsNullOrEmpty(res.Msg))
                await Split(res.Msg);

            UpdateSource();
        }

        private async Task Split(string msg)
        {
            try
            {
                foreach (var el in JsonConvert.DeserializeObject<CarHistoryWorkReport[]>(msg))
                {
                    _historyWorks.Add(new WorkFromHistoryViewModel(el));
                }
            }
            catch (Exception)
            {

            }
        }

    }
}
