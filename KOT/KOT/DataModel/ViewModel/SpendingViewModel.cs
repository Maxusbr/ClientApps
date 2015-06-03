using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Windows.ApplicationModel;
using KOT.Annotations;
using KOT.DataModel.Model;

namespace KOT.DataModel.ViewModel
{
    public class SpendingViewModel:INotifyPropertyChanged
    {
        private SpendingModel _model;
        private const double H = 200;
        private double _maxvalue;
        private double _rashodCost;
        private double _shopCost;
        private double _carwashCost;
        private double _parkingCost;
        private double _gasCost;

        public SpendingViewModel()
        {
            if (DesignMode.DesignModeEnabled)
            {
                var current = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                _model = new SpendingModel { DID = "1", TotalCost = 6456 };
                _model.Spends.Add(new OneSpendItem
                {
                    Date = new DateDataModel(current.AddDays(-5)),
                    Name = "Comment 1",
                    Sum = 156,
                    idClass = 0
                });
                _model.Spends.Add(new OneSpendItem
                {
                    Date = new DateDataModel(current.AddDays(-4)),
                    Name = "Comment 2",
                    Sum = 268,
                    idClass = 1
                });
                _model.Spends.Add(new OneSpendItem
                {
                    Date = new DateDataModel(current.AddDays(-2)),
                    Name = "Comment 3",
                    Sum = 878,
                    idClass = 2
                });
                _model.TotalCost = _model.Spends.Sum(o => o.Sum);
                Calc();
            }
        }
        public SpendingViewModel(SpendingModel model)
        {
            Update(model);
        }

        public void Update(SpendingModel model)
        {
            _model = model;
            Calc();
            
        }
        private void Calc()
        {
            GasCost = _model.Spends.Where(w => w.idClass == 0).Sum(o => o.Sum);
            ParkingCost = _model.Spends.Where(w => w.idClass == 1).Sum(o => o.Sum);
            CarwashCost = _model.Spends.Where(w => w.idClass == 2).Sum(o => o.Sum);
            ShopCost = _model.Spends.Where(w => w.idClass == 3).Sum(o => o.Sum);
            RashodCost = _model.Spends.Where(w => w.idClass == 4).Sum(o => o.Sum);
            _maxvalue = Math.Max(GasCost, Math.Max(ParkingCost, Math.Max(CarwashCost, Math.Max(ShopCost, RashodCost))));
            UpdateUi();
        }

        private void UpdateUi()
        {
            OnPropertyChanged("GasCostValue");
            OnPropertyChanged("ParkingCostValue");
            OnPropertyChanged("CarwashCostValue");
            OnPropertyChanged("ShopCostValue");
            OnPropertyChanged("RashodCostValue");
        }

        public string TotalCost { get { return _model.TotalCost > 0 ? string.Format("{0} руб.", _model.TotalCost) : ""; } }

        public string GasCostTxt { get { return GasCost > 0 ? string.Format("{0} руб.", GasCost) : ""; } }

        public string ParkingCostTxt { get { return ParkingCost > 0 ? string.Format("{0} руб.", ParkingCost) : ""; } }

        public string CarwashCostTxt { get { return CarwashCost > 0 ? string.Format("{0} руб.", CarwashCost) : ""; } }

        public string ShopCostTxt { get { return ShopCost > 0 ? string.Format("{0} руб.", ShopCost) : ""; } }

        public string RashodCostTxt { get { return RashodCost > 0 ? string.Format("{0} руб.", RashodCost) : ""; } }

        public double GasCostValue { get { return GasCost * H / _maxvalue; } }

        public double ParkingCostValue { get { return ParkingCost * H / _maxvalue; } }

        public double CarwashCostValue { get { return CarwashCost * H / _maxvalue; } }

        public double ShopCostValue { get { return ShopCost * H / _maxvalue; } }

        public double RashodCostValue { get { return RashodCost * H / _maxvalue; } }

        public double GasCost
        {
            get { return _gasCost; }
            set
            {
                if (value.Equals(_gasCost)) return;
                _gasCost = value;
                OnPropertyChanged();
                OnPropertyChanged("GasCostTxt");
            }
        }

        public double ParkingCost
        {
            get { return _parkingCost; }
            set
            {
                if (value.Equals(_parkingCost)) return;
                _parkingCost = value;
                OnPropertyChanged();
                OnPropertyChanged("ParkingCostTxt");
            }
        }

        public double CarwashCost
        {
            get { return _carwashCost; }
            set
            {
                if (value.Equals(_carwashCost)) return;
                _carwashCost = value;
                OnPropertyChanged();
                OnPropertyChanged("CarwashCostTxt");
            }
        }

        public double ShopCost
        {
            get { return _shopCost; }
            set
            {
                if (value.Equals(_shopCost)) return;
                _shopCost = value;
                OnPropertyChanged();
                OnPropertyChanged("ShopCostTxt");
            }
        }

        public double RashodCost
        {
            get { return _rashodCost; }
            set
            {
                if (value.Equals(_rashodCost)) return;
                _rashodCost = value;
                OnPropertyChanged();
                OnPropertyChanged("RashodCostTxt");
            }
        }

        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

    }
}
