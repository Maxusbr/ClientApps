using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Forms.VisualStyles;
using DTCDev.Client.Cars.Service.Engine.Handlers;
using DTCDev.Client.ViewModel;
using DTCDev.Models.CarBase.CarStatData;
using DTCDev.Models.CarBase.Shops;

namespace DTCDev.Client.Cars.Service.Engine.Controls.ViewModels.Sklad
{
    public class ItemSkladViewModel : ViewModelBase
    {
        private readonly SkladHandler _handler = SkladHandler.Instance;
        private readonly ItemSkladDataModel _model = new ItemSkladDataModel();


        public ItemSkladViewModel()
        {
            _model = new ItemSkladDataModel { ArtNo = "", BaseValue = 1, Division = new KVPBase(), Type = new KVPBase(), Unit = new KVPBase(), Vendor = new KVPBase() };
            Uses = ListUses[0]; EnableButtonSave = false;
        }

        public ItemSkladViewModel(ItemSkladDataModel model)
            : base()
        {
            _model = model; CurentPurchase = model.Purchase;
            EnableButtonSave = true;
        }

        public void Update(ItemSkladDataModel model)
        {
            Price = model.Price;
            Purchase = model.Purchase;
            Quantity = model.Quantity;
            CurentPurchase = model.Purchase;
        }

        public int Id
        {
            get { return _model.id; }
            set { _model.id = value; }
        }

        public string Name
        {
            get { return _model.Name; }
            set
            {
                _model.Name = value;
                OnPropertyChanged("Name");
            }
        }

        public string ArtNo
        {
            get { return _model.ArtNo; }
            set
            {
                _model.ArtNo = value;
                OnPropertyChanged("ArtNo");
            }
        }

        public ObservableCollection<KVPBase> Vendors { get { return _handler.Vendors; } }

        public KVPBase Vendor
        {
            get
            {
                if (Vendors.FirstOrDefault(o => o.Name.Equals(_model.Vendor.Name)) == null)
                    _model.Vendor = _handler.GetVendor(_model.Vendor.Name);
                return _model.Vendor;
            }
            set
            {
                _model.Vendor = _handler.GetVendor(value.Name);
                OnPropertyChanged("Vendor");
            }
        }

        public ObservableCollection<KVPBase> Types { get { return _handler.Types; } }

        public KVPBase Type
        {
            get
            {
                if (Types.FirstOrDefault(o => o.Name.Equals(_model.Type.Name)) == null)
                    _model.Type = _handler.GetTypes(_model.Type.Name);
                return _model.Type;
            }
            set
            {
                _model.Type = _handler.GetTypes(value.Name);
                OnPropertyChanged("Type");
            }
        }

        public string Info
        {
            get { return _model.Info; }
            set
            {
                _model.Info = value;
                OnPropertyChanged("Info");
            }
        }

        public string LinkPhoto
        {
            get { return _model.LinkPhoto; }
            set
            {
                _model.LinkPhoto = value;
                OnPropertyChanged("LinkPhoto");
            }
        }

        public ObservableCollection<KVPBase> Units { get { return _handler.Units; } }
        public KVPBase Unit
        {
            get
            {
                if (Units.FirstOrDefault(o => o.Name.Equals(_model.Unit.Name)) == null)
                    _model.Unit = _handler.GetUnit(_model.Unit.Name);
                return _model.Unit;
            }
            set
            {
                _model.Unit = _handler.GetUnit(value.Name);
                OnPropertyChanged("Unit");
            }
        }

        public double BaseValue
        {
            get { return _model.BaseValue; }
            set
            {
                _model.BaseValue = value;
                OnPropertyChanged("BaseValue");
            }
        }

        public ObservableCollection<KVPBase> ListUses { get { return _handler.Uses; } }

        public KVPBase Uses
        {
            get
            {
                if (ListUses.FirstOrDefault(o => o.Name.Equals(_model.Uses.Name)) == null)
                    _model.Uses = _handler.GetUses(_model.Uses.Name);
                return _model.Uses;
            }
            set
            {
                _model.Uses = _handler.GetUses(value.Name);
                OnPropertyChanged("Uses");
            }
        }

        public ObservableCollection<KVPBase> Divisions { get { return _handler.Divisions; } }

        public KVPBase Division
        {
            get
            {
                if (Divisions.FirstOrDefault(o => o.Name.Equals(_model.Division.Name)) == null)
                    _model.Division = _handler.GetDivision(_model.Division.Name);
                return _model.Division;
            }
            set
            {
                _model.Division = _handler.GetDivision(value.Name);
                OnPropertyChanged("Uses");
            }
        }

        public double Purchase
        {
            get { return _model.Purchase; }
            set
            {
                _model.Purchase = value;
                OnPropertyChanged("Purchase");
            }
        }

        public double Price
        {
            get { return _model.Price; }
            set
            {
                _model.Price = value;
                OnPropertyChanged("Price");
            }
        }

        public int Quantity
        {
            get { return _model.Quantity; }
            set
            {
                _model.Quantity = value;
                OnPropertyChanged("Quantity");
            }
        }

        private int _curentCount = 1;
        private double _curentPurchase;
        private double _curentPurchaseTotal;
        private bool _enableButtonSave;

        public int CurentCount
        {
            get { return _curentCount; }
            set
            {
                _curentCount = value;
                OnPropertyChange("CurentCount");
                if (value <= 0) return;
                if (CurentPurchase > 0)
                {
                    _curentPurchaseTotal = CurentPurchase * value;
                    OnPropertyChange("CurentPurchaseTotal");
                }
                else if (CurentPurchaseTotal > 0)
                {
                    _curentPurchase = CurentPurchaseTotal / value;
                    OnPropertyChange("CurentPurchase");
                }
            }
        }


        public double CurentPurchase
        {
            get { return _curentPurchase; }
            set
            {
                _curentPurchase = value;
                if (value <= 0) return;
                if (CurentCount > 0)
                {
                    _curentPurchaseTotal = Math.Round(CurentCount * value, 2);
                    OnPropertyChange("CurentPurchaseTotal");
                }
                OnPropertyChange("CurentPurchase");
            }
        }

        public double CurentPurchaseTotal
        {
            get { return _curentPurchaseTotal; }
            set
            {
                _curentPurchaseTotal = value;
                if (value <= 0) return;
                if (CurentCount > 0)
                {
                    CurentPurchase = Math.Round(value / CurentCount, 2);
                    OnPropertyChange("CurentPurchase");
                }
                OnPropertyChange("CurentPurchaseTotal");
            }
        }

        public bool EnableButtonSave
        {
            get { return _enableButtonSave; }
            set
            {
                _enableButtonSave = value;
                OnPropertyChanged("EnableButtonSave");
            }
        }

        private void OnPropertyChange(string p)
        {
            EnableButtonSave = true;
            OnPropertyChanged(p);
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", Name, ArtNo);
        }

        public ItemSkladDataModel GetModel
        {
            get
            {
                _model.Purchase = (_model.Purchase * _model.Quantity + CurentPurchase * CurentCount) / (_model.Quantity + CurentCount);
                _model.Quantity += CurentCount;
                return _model;
            }
        }

        public ItemSkladDataModel Model
        {
            get { return _model; }
        }
    }
}
