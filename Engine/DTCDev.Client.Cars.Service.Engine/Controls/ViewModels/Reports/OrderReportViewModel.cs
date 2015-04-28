using DTCDev.Client.Cars.Service.Engine.Handlers;
using DTCDev.Client.ViewModel;
using DTCDev.Models.CarsSending.Order;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace DTCDev.Client.Cars.Service.Engine.Controls.ViewModels.Reports
{
    public class OrderReportViewModel : ViewModelBase
    {
        public OrderReportViewModel()
        {
            Monthes.Add("Январь");
            Monthes.Add("Февраль");
            Monthes.Add("Март");
            Monthes.Add("Апрель");
            Monthes.Add("Май");
            Monthes.Add("Июнь");
            Monthes.Add("Июль");
            Monthes.Add("Август");
            Monthes.Add("Сентябрь");
            Monthes.Add("Октябрь");
            Monthes.Add("Ноябрь");
            Monthes.Add("Декабрь");
            ReportsHandler.Instance.OrderReportLoaded += Instance_OrderReportLoaded;
        }

        private bool _dateChecked = true;
        public bool DateChecked
        {
            get { return _dateChecked; }
            set
            {
                _dateChecked = value;
                this.OnPropertyChanged("DateChecked");
            }
        }

        private bool _monthChecked = false;
        public bool MonthChecked
        {
            get { return _monthChecked; }
            set
            {
                _monthChecked = value;
                this.OnPropertyChanged("MonthChecked");
            }
        }


        private DateTime _dtStart = DateTime.Now - TimeSpan.FromDays(3);
        public DateTime DTStart
        {
            get { return _dtStart; }
            set
            {
                _dtStart = value;
                this.OnPropertyChanged("DTStart");
            }
        }

        private DateTime _dtStop = DateTime.Now;
        public DateTime DTStop
        {
            get { return _dtStop; }
            set
            {
                _dtStop = value;
                this.OnPropertyChanged("DTStop");
            }
        }

        private int _ordersCount = 0;
        public int OrdersCount
        {
            get { return _ordersCount; }
            set
            {
                _ordersCount = value;
                this.OnPropertyChanged("OrdersCount");
            }
        }

        private int _ordersSumm = 0;
        public int OrdersSumm
        {
            get { return _ordersSumm; }
            set
            {
                _ordersSumm = value;
                this.OnPropertyChanged("OrdersSumm");
            }
        }


        private readonly ObservableCollection<string> _monthes = new ObservableCollection<string>();
        public ObservableCollection<string> Monthes
        {
            get { return _monthes; }
        }

        private int _selectedMonth=-1;
        public int SelectedMonth
        {
            get { return _selectedMonth; }
            set
            {
                _selectedMonth = value;
                this.OnPropertyChanged("SelectedMonth");
            }
        }

        private readonly ObservableCollection<OrderModel> _orders = new ObservableCollection<OrderModel>();
        public ObservableCollection<OrderModel> Orders
        {
            get { return _orders; }
        }







        private RelayCommand _loadCommand;
        public RelayCommand LoadCommand { get { return _loadCommand ?? (_loadCommand = new RelayCommand(LoadData)); } }

        private void LoadData(object sender)
        {
            StartLoad();
        }







        void Instance_OrderReportLoaded(List<OrderModel> data)
        {
            Orders.Clear();
            data.ForEach(x => Orders.Add(x));
            OrdersCount = data.Count();
            OrdersSumm = data.Sum(p => p.Cost);
        }




        private void StartLoad()
        {
            if (DateChecked)
            {
                ReportsHandler.Instance.GetOrdersReport(DTStart, DTStop);
            }
            else
            {
                DateTime start = new DateTime(DateTime.Now.Year,SelectedMonth, 1);
                if (SelectedMonth < 11)
                {
                    DateTime stop = new DateTime(DateTime.Now.Year, SelectedMonth + 1, 1);
                    ReportsHandler.Instance.GetOrdersReport(start, stop);
                }
                else
                {
                    DateTime stop = new DateTime(DateTime.Now.Year + 1, 1, 1);
                    ReportsHandler.Instance.GetOrdersReport(start, stop);
                }
            }
        }
    }
}
