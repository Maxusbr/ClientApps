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
    public class SpendingViewModel
    {
        private readonly SpendingModel _model;
        private const double H = 200;
        private double _maxvalue;
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

        public double GasCost { get; set; }

        public double ParkingCost { get; set; }

        public double CarwashCost { get; set; }

        public double ShopCost { get; set; }

        public double RashodCost { get; set; }

    }
}
