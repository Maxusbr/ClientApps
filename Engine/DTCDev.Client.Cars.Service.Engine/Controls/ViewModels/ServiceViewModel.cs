using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using DTCDev.Client.ViewModel;
using DTCDev.Models.CarBase.CarStatData;
using DTCDev.Models.Date;

namespace DTCDev.Client.Cars.Service.Engine.Controls.ViewModels
{
    /// <summary>
    /// Модель данных для работы с списком услуг
    /// </summary>
    public class ServiceViewModel : ViewModelBase
    {
        private string _id;
        private string _name;
        private string _curentDate;
        private int _basePeriod;
        private string _periodDimension;
        private int _distanceToMake;
        private double _price;
        private string _comment;
        private string _duration;
        private readonly ObservableCollection<ServiceViewModel> _works = new ObservableCollection<ServiceViewModel>();
        private string _period;
       

        public ServiceViewModel() { }

        public ServiceViewModel(ServiceBaseDataModel model)
        {
            _id = model.ID;
            Name = model.Name;
            CurentDate = model.CurentDate.ToDateTime().ToShortDateString();
            BasePeriod = model.BasePeriod;
            PeriodDimension = model.PeriodDimension;
            Period = string.Format("{0} {1}", model.BasePeriod, model.PeriodDimension);
            Duration = model.Duration.ToTime().ToString("c");
            DistanceToMake = model.DistanceToMake;
            Price = model.Price;
            Comment = model.Comment;
            Works.Clear();
            model.Works.ForEach(o => Works.Add(new ServiceViewModel(o)));
        }

        public string ID
        {
            get
            {
                return _id;
            }
            set { _id = value; }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name == value) return;
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        public string Period
        {
            get { return _period; }
            set
            {
                if (_period == value) return;
                _period = value;
                OnPropertyChanged("Period");
            }
        }

        public string CurentDate
        {
            get { return _curentDate; }
            set
            {
                if (_curentDate == value) return;
                _curentDate = value;
                OnPropertyChanged("CurentDate");
            }
        }

        public int BasePeriod
        {
            get { return _basePeriod; }
            set
            {
                if (_basePeriod == value) return;
                _basePeriod = value;
                OnPropertyChanged("BasePeriod");
            }
        }

        public int OrderN { get; set; }

        public string PeriodDimension
        {
            get { return _periodDimension; }
            set
            {
                if (_periodDimension == value) return;
                _periodDimension = value;
                OnPropertyChanged("PeriodDimension");
            }
        }

        public int DistanceToMake
        {
            get { return _distanceToMake; }
            set
            {
                if (_distanceToMake == value) return;
                _distanceToMake = value;
                OnPropertyChanged("DistanceToMake");
            }
        }

        public double Price
        {
            get { return _price; }
            set
            {
                if (_price == value) return;
                _price = value;
                OnPropertyChanged("Price");
            }
        }

        public string Comment
        {
            get { return _comment; }
            set
            {
                if (_comment == value) return;
                _comment = value;
                OnPropertyChanged("Comment");
            }
        }

        public string Duration
        {
            get { return _duration; }
            set
            {
                if (_duration == value) return;
                _duration = value;
                OnPropertyChanged("Duration");
            }
        }

        public ObservableCollection<ServiceViewModel> Works
        {
            get { return _works; }
        }

        public ServiceBaseDataModel GetServiceModel(ServiceViewModel item)
        {
            DateTime dt, dr;
            DateTime.TryParse(item.CurentDate, out dt);
            DateTime.TryParse(item.Duration, out dr);
            return new ServiceBaseDataModel
            {
                ID=item.ID,
                BasePeriod = item.BasePeriod,
                CurentDate = new DateTimeDataModel(dt),
                Name = item.Name,
                Duration = new DateTimeDataModel(dr),
                Comment = item.Comment,
                DistanceToMake = item.DistanceToMake,
                PeriodDimension = item.PeriodDimension,
                Price = item.Price,
                Works = item.Works.Select(GetServiceModel).ToList()
            };
        }

        public ServiceBaseDataModel GetServiceModel()
        {
            DateTime dt, dr;
            DateTime.TryParse(CurentDate, out dt);
            DateTime.TryParse(Duration, out dr);
            return new ServiceBaseDataModel
            {
                ID = ID,
                BasePeriod = BasePeriod,
                CurentDate = new DateTimeDataModel(dt),
                Name = Name,
                Duration = new DateTimeDataModel(dr),
                Comment = Comment,
                DistanceToMake = DistanceToMake,
                PeriodDimension = PeriodDimension,
                Price = Price,
                Works = Works.Select(GetServiceModel).ToList()
            };
        }
        public override string ToString()
        {
            return Name + " " + ((int)Price).ToString()+ " руб.";
        }
    }
}
