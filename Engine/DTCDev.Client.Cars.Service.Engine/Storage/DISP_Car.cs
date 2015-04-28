using DTCDev.Client.ViewModel;
using DTCDev.Models.CarBase.CarList;
using DTCDev.Models.CarsSending.Car;
using DTCDev.Models.CarsSending.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace DTCDev.Client.Cars.Service.Engine.Storage
{
    public class DISP_Car : ViewModelBase
    {
        private CarListBaseDataModel _carModel = new CarListBaseDataModel();
        public CarListBaseDataModel CarModel
        {
            get { return _carModel; }
            set { _carModel = value; }
        }

        private List<CarDTCHistoryModel> _errors = new List<CarDTCHistoryModel>();
        public List<CarDTCHistoryModel> Errors
        {
            get { return _errors; }
            set { _errors = value; }
        }

        public int ErrorsCount
        {
            get { return _errors.Count(); }
        }

        private Visibility _errorsVis=Visibility.Collapsed;
        public Visibility ErrorsVis
        {
            get { return _errorsVis; }
            set{
                _errorsVis = value;
                this.OnPropertyChanged("ErrorsVis");
        }
        }

        private CarOrderModel _order;
        public CarOrderModel Order
        {
            get { return _order; }
            set
            {
                _order = value;
                if (value != null)
                {
                    OrderNumber = value.OrderNumer;
                }
            }
        }

        private CarStateOBDModel _obd;
        public CarStateOBDModel CurrentOBD
        {
            get { return _obd; }
            set
            {
                BuildLast(value);
                _obd = value;
                this.OnPropertyChanged("CurrentOBD");
            }
        }

        private void BuildLast(CarStateOBDModel model)
        {
            if(model!=null)
                if(model.OBD!=null)
                    foreach (var item in model.OBD)
                    {
                        string temp = item.Key;
                        if (_obdLast.ContainsKey(temp))
                        {
                            _obdLast[temp].Add(item.Value);
                        }
                        else
                        {
                            _obdLast.Add(temp, new List<string>());
                            _obdLast[temp].Add(item.Value);
                        }
                        if (_obdLast[temp].Count() > 19)
                            _obdLast[temp].RemoveAt(0);
                    }
        }

        private Dictionary<string, List<string>> _obdLast = new Dictionary<string, List<string>>();

        public Dictionary<string, List<string>> OBDLast
        {
            get { return _obdLast; }
        }


        private int _orderNumber = 0;
        public int OrderNumber
        {
            get { return _orderNumber; }
            set
            {
                _orderNumber = value;
                this.OnPropertyChanged("OrderNumber");
            }
        }

        private OBDHistoryDataModel _obdHistory = new OBDHistoryDataModel();
        public OBDHistoryDataModel OBDHistory
        {
            get { return _obdHistory; }
            set { _obdHistory = value; }
        }

        private DateTime _calledOBD;
        public DateTime CalledOBDHistory
        {
            get { return _calledOBD; }
            set { _calledOBD = value; }
        }


        private int _updatedTime = -1;
        public int UpdatedTime
        {
            get { return _updatedTime; }
            set
            {
                _updatedTime = value;
                this.OnPropertyChanged("UpdatedTime");
                if (value < 0)
                    OnLineColor = new SolidColorBrush(Colors.DarkGray);
                else if (value < 100)
                    OnLineColor = new SolidColorBrush(Colors.Green);
                else if(value<4320)
                    OnLineColor = new SolidColorBrush(Colors.Yellow);
                else
                    OnLineColor = new SolidColorBrush(Colors.DarkGray);

            }
        }

        private Brush _bgColor = new SolidColorBrush(Colors.DarkGray);
        public Brush OnLineColor
        {
            get { return _bgColor; }
            set
            {
                _bgColor = value;
                this.OnPropertyChanged("OnLineColor");
            }
        }

        public void UpdateErrors()
        {
            if (Errors.Count() > 0)
                ErrorsVis = Visibility.Visible;
            else
                ErrorsVis = Visibility.Collapsed;
        }
    }
}
