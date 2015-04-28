using DTCDev.Client.Cars.Service.Engine.Handlers;
using DTCDev.Client.Cars.Service.Engine.Storage;
using DTCDev.Client.ViewModel;
using DTCDev.Models.CarBase.CarStatData;
using DTCDev.Models.CarsSending.Order;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;

namespace DTCDev.Client.Cars.Service.Engine.Controls.ViewModels.Slides
{
    public class SlideOrdersListViewModel :ViewModelBase
    {
        public SlideOrdersListViewModel()
        {
            Orders = new ObservableCollection<CarOrderModel>();
            Works = new ObservableCollection<WorksListModel>();
            OrderStorage.Instance.OrdersRefreshed += Instance_OrdersRefreshed;
            OrderStorage.Instance.UpdateOrders();
            CarsHandler.Instance.OrderRecomendationsLoad += Instance_OrderRecomendationsLoad;
            SpecificationDataStorage.Instance.LoadWorkPartsListComplete += Instance_LoadWorkPartsListComplete;
            FillOrders();
        }

        void Instance_LoadWorkPartsListComplete(List<WorksInfoDataModel> data)
        {
            FillWorks(data);
        }

        void Instance_OrderRecomendationsLoad(string text)
        {
            _orderRecomendation = OrderRecomendationEdited = text;
        }

        void Instance_OrdersRefreshed(object sender, EventArgs e)
        {
            FillOrders();
        }




        public ObservableCollection<CarOrderModel> Orders { get; set; }
        public ObservableCollection<WorksListModel> Works { get; set; }

        private CarOrderModel _selectedOrder;
        public CarOrderModel SelectedOrder
        {
            get { return _selectedOrder; }
            set
            {
                Works.Clear();
                _selectedOrder = value;
                this.OnPropertyChanged("SelectedOrder");
                if (value == null)
                    EndOrderEnabled = false;
                else
                {
                    EndOrderEnabled = true;
                    value.Works.ForEach(a => Works.Add(new WorksListModel
                    {
                        Model = a,
                        Comment = "",
                        Cost = a.Cost,
                        NewCost = a.Cost
                    }));
                    CarsHandler.Instance.GetOrderRecomendation(SelectedOrder.OrderNumer);
                }
            }
        }

        private WorksListModel _selectedWork;
        public WorksListModel SelectedWork
        {
            get { return _selectedWork; }
            set
            {
                _selectedWork = value;
                this.OnPropertyChanged("SelectedWork");
                if (value == null)
                    EditOrderEnabled = false;
                else
                    EditOrderEnabled = true;
            }
        }

        private bool _editOrderEnabled = false;
        public bool EditOrderEnabled
        {
            get { return _editOrderEnabled; }
            set
            {
                _editOrderEnabled = value;
                this.OnPropertyChanged("EditOrderEnabled");
            }
        }

        private bool _endOrderEnabled = false;
        public bool EndOrderEnabled
        {
            get { return _endOrderEnabled; }
            set
            {
                _endOrderEnabled = value;
                this.OnPropertyChanged("EndOrderEnabled");
            }
        }

        private string _orderRecomendation = "";
        private string _orderRecomendationEdited = "";
        public string OrderRecomendationEdited
        {
            get { return _orderRecomendationEdited; }
            set
            {
                _orderRecomendationEdited = value;
                this.OnPropertyChanged("OrderRecomendationEdited");
            }
        }




        private RelayCommand _saveWorkChangeCommand;
        public RelayCommand SaveWorkChangeCommand { get { return _saveWorkChangeCommand ?? (_saveWorkChangeCommand = new RelayCommand(SaveWorkChange)); } }
        private void SaveWorkChange(object sender)
        {
            if (SelectedWork == null)
                return;
            if (SelectedOrder == null)
                return;
            if (SelectedWork.Cost < 1)
                return;
            SelectedWork.Cost = SelectedWork.NewCost;
            WorkChangeModel model = new WorkChangeModel();
            model.Coment = SelectedWork.Comment;
            model.ID = SelectedWork.Model.id;
            model.NewCost = SelectedWork.NewCost;
            OrderStorage.Instance.UpdateWork(model, SelectedOrder);

            FillOrders();
        }

        private RelayCommand _closeOrderCommand;
        public RelayCommand CloseOrderCommand { get { return _closeOrderCommand ?? (_closeOrderCommand = new RelayCommand(CloseOrder)); } }
        private void CloseOrder(object sender)
        {
            if (SelectedOrder == null)
                return;
            OrderStorage.Instance.CloseOrder(SelectedOrder.OrderNumer);
        }

        private RelayCommand _saveOrderRecomendationCommand;
        public RelayCommand SaveOrderRecomendationCommand { get { return _saveOrderRecomendationCommand ?? (_saveOrderRecomendationCommand = new RelayCommand(SaveOrderRecomendation)); } }
        private void SaveOrderRecomendation(object sender)
        {
            _orderRecomendation = OrderRecomendationEdited;
            CarsHandler.Instance.UpdateRecomendations(SelectedOrder.OrderNumer, OrderRecomendationEdited);
        }

        private RelayCommand _undoOrderRecomendationCommand;
        public RelayCommand UndoOrderRecomendationCommand { get { return _undoOrderRecomendationCommand ?? (_undoOrderRecomendationCommand = new RelayCommand(UndoOrderRecomendation)); } }
        private void UndoOrderRecomendation(object sender)
        {
            OrderRecomendationEdited = _orderRecomendation;
        }



        private void FillOrders()
        {
            Orders.Clear();
            foreach (var item in OrderStorage.Instance.CurrnetOrders)
            {
                Orders.Add(item);
            }
        }




        public void StartLoadInfo()
        {
            if (SelectedOrder == null)
                return;
            new Thread(TrLoad).Start();
        }

        private void TrLoad()
        {
            for (int i = SelectedOrder.Works.Count()-1; i >= 0; i--)
			{
                WorksInfoDataModel item = SelectedOrder.Works[i];
                WorksListModel model = Works.Where(p => p.Model.id == item.id).FirstOrDefault();
                if (model != null)
                    if (model.WorksInfo.Count() < 1)
                        SpecificationDataStorage.Instance.GetWorkParts(item.idWork, SelectedOrder.CarNumber);
                Thread.Sleep(50);
            }
        }

        private void FillWorks(List<WorksInfoDataModel> data)
        {
            if(data!=null)
                if (data.Count() > 0)
                {
                    int id = data[0].idWork;
                    WorksListModel model = Works.Where(p => p.Model.idWork == id).FirstOrDefault();
                    if (model != null)
                    {
                        data.ForEach(x => x.NHD = x.NH / 10.0m);
                        model.WorksInfo = data;
                    }
                }
        }



        public class WorksListModel : ViewModelBase
        {
            private WorksInfoDataModel _model;
            public WorksInfoDataModel Model
            {
                get { return _model; }
                set { _model = value; }
            }

            private int _cost = 0;
            public int Cost
            {
                get { return _cost; }
                set
                {
                    _cost = value;
                    this.OnPropertyChanged("Cost");
                }
            }

            private int _newCost = 0;
            public int NewCost
            {
                get { return _newCost; }
                set
                {
                    _newCost = value;
                    this.OnPropertyChanged("NewCost");
                }
            }

            private string _comment;
            public string Comment
            {
                get { return _comment; }
                set
                {
                    _comment = value;
                    this.OnPropertyChanged("Comment");
                }
            }

            private List<WorksInfoDataModel> _worksInfo = new List<WorksInfoDataModel>();
            public List<WorksInfoDataModel> WorksInfo
            {
                get { return _worksInfo; }
                set
                {
                    _worksInfo = value;
                    this.OnPropertyChanged("WorksInfo");
                }
            }
        }
    }
}
