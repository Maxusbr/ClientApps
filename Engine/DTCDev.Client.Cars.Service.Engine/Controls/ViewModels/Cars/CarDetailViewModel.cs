using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using DTCDev.Client.Cars.Service.Engine.Handlers;
using DTCDev.Client.ViewModel;
using DTCDev.Models.CarBase.CarList;
using DTCDev.Models.Date;
using DTCDev.Client.Cars.Service.Engine.Controls.View;
using System.Windows;
using DTCDev.Models.CarBase.CarStatData;
using System.Windows.Media.Imaging;
using DTCDev.Client.Cars.Service.Engine.Storage;

namespace DTCDev.Client.Cars.Service.Engine.Controls.ViewModels
{
    public class CarDetailViewModel : CarViewModel, ICancelHandler
    {
        private DateDataModel _dateProduce;
        private string _companyId;
        private string _engineType;
        private string _engineVolume;
        private string _transmissionType;
        private readonly ObservableCollection<CarListDetailsDataModel.WorkType> _works =
            new ObservableCollection<CarListDetailsDataModel.WorkType>();
        private readonly ObservableCollection<KVPBase> _recomendations = new ObservableCollection<KVPBase>();
        private string _companyName;
        private string _companyPhone;
        private RelayCommand _closeCommand;
        CarListDetailsDataModel _model = new CarListDetailsDataModel();
        private bool _visableCloseCommand = true;
        private RelayCommand _newWorkCommand;
        private CarListDetailsDataModel.WorkType _selectedWork;
        private DateDataModel _datePurchase;

        private ServiceViewModel _service = new ServiceViewModel();
        private ObservableCollection<ServiceViewModel> _services = new ObservableCollection<ServiceViewModel>();
        private RelayCommand _saveCommand;
        private RelayCommand _addWorkCommand;
        private readonly ObservableCollection<ServiceViewModel> _filteredServices = new ObservableCollection<ServiceViewModel>();
        private string _serviceName;
        private ServiceViewModel _filteredService = new ServiceViewModel();
        private string _filteredServiceName;
        private RelayCommand _cancelCommand;
        private bool _enableControls = true;
        private bool _enableSaveButton = true;
        private int _errorsCount = 0;
        private Visibility _visErrors = Visibility.Collapsed;


        public CarDetailViewModel(CarListDetailsDataModel model, int errorsCount)
            : base(model, errorsCount)
        {
            CompanyID = model.CompanyID;
            DateProduce = model.DateProduce;
            DatePurchase = model.DatePurchase;
            EngineType = model.EngineType;
            EngineVolume = model.EngineVolume;
            CompanyName = model.UserName;
            CompanyPhone = model.UserPhone;
            TransmissionType = model.TransmissionType;
            model.Works.ForEach(o => Works.Add(o));
            CompanyHandler.Instance.LoadComplete += Instance_LoadComplete;
            CompanyHandler.Instance.GetCompany(model.CompanyID);
            _model = model;
            _errorsCount = errorsCount;
            if (_errorsCount > 0)
                _visErrors = Visibility.Visible;
            DisplayLogoImage();
            CarsHandler.Instance.OrderDataLoaded += Instance_OrderDataLoaded;
            CarsHandler.Instance.GetCarOrder(CarStorage.Instance.SelectedCar.CarModel.CarNumber);
            CarsHandler.Instance.GetCarRecomendationsComplete += Instance_GetCarRecomendationsComplete;
            CarsHandler.Instance.GetCarRecomendations(_model.ID);
        }

        void Instance_GetCarRecomendationsComplete(List<KVPBase> data)
        {
            Recomendations.Clear();
            data.ForEach(x => Recomendations.Add(x));
        }

        void Instance_OrderDataLoaded(object sender, EventArgs e)
        {
            if (CarStorage.Instance.SelectedCar.Order == null)
                return;
            OrderNumber = CarStorage.Instance.SelectedCar.Order.OrderNumer.ToString();
            CreateDate = CarStorage.Instance.SelectedCar.Order.DTCreate.ToString();
            Cost = CarStorage.Instance.SelectedCar.Order.Cost.ToString();
        }

        public CarDetailViewModel()
        {
            Recomendations.Add(new KVPBase
            {
                id = 0,
                Name = "Рекомендаций для автомобиля нет"
            });
        }

        public event EventHandler CancelHandler;
        protected virtual void OnCancelHandler(object obj)
        {
            if (CancelHandler != null) CancelHandler(obj, EventArgs.Empty);
        }

        public event EventHandler CloseViewClick;

        void Instance_LoadComplete(object sender, EventArgs e)
        {
            // TODO Заменить на модель компании
            var company = sender as string;
            if(company == null) return;
            CompanyName = company;
            CompanyPhone = company;
        }

        public string CompanyID
        {
            get { return _companyId; }
            set
            {
                if (_companyId == value) return;
                _companyId = value;
                OnPropertyChanged("CompanyID");
            }
        }

        public DateDataModel DateProduce
        {
            get { return _dateProduce; }
            set
            {
                if (_dateProduce == value) return;
                _dateProduce = value;
                OnPropertyChanged("DateProduce");
            }
        }

        public DateDataModel DatePurchase
        {
            get { return _datePurchase; }
            set
            {
                if (_datePurchase == value) return;
                _datePurchase = value;
                OnPropertyChanged("DatePurchase");
            }
        }

        public string EngineType
        {
            get { return _engineType; }
            set
            {
                if (_engineType == value) return;
                _engineType = value;
                OnPropertyChanged("EngineType");
            }
        }

        public string EngineVolume
        {
            get { return _engineVolume; }
            set
            {
                if (_engineVolume == value) return;
                _engineVolume = value;
                OnPropertyChanged("EngineVolume");
            }
        }

        public string TransmissionType
        {
            get { return _transmissionType; }
            set
            {
                if (_transmissionType == value) return;
                _transmissionType = value;
                OnPropertyChanged("TransmissionType");
            }
        }

        public ObservableCollection<CarListDetailsDataModel.WorkType> Works
        {
            get { return _works; }
        }

        public ObservableCollection<KVPBase> Recomendations { get { return _recomendations; } }

        public string CompanyName
        {
            get { return _companyName; }
            set
            {
                if (_companyName == value) return;
                _companyName = value;
                OnPropertyChanged("CompanyName");
            }
        }

        public string CompanyPhone
        {
            get { return _companyPhone; }
            set
            {
                if (_companyPhone == value) return;
                _companyPhone = value;
                OnPropertyChanged("CompanyPhone");
            }
        }

        private string _orderNumber = "Нет";
        public string OrderNumber
        {
            get { return _orderNumber; }
            set
            {
                _orderNumber = value;
                this.OnPropertyChanged("OrderNumber");
            }
        }

        private string _createDate = "";
        public string CreateDate
        {
            get { return _createDate; }
            set
            {
                _createDate = value;
                this.OnPropertyChanged("CreateDate");
            }
        }

        private string _cost = "";
        public string Cost
        {
            get { return _cost; }
            set
            {
                _cost = value;
                this.OnPropertyChanged("Cost");
            }
        }

        private BitmapImage _logoImage;
        public BitmapImage LogoImage
        {
            get { return _logoImage; }
            set
            {
                _logoImage = value;
                this.OnPropertyChanged("LogoImage");
            }
        }

        public CarListDetailsDataModel.WorkType SelectedWork
        {
            get { return _selectedWork; }
            set
            {
                if (Equals(_selectedWork, value)) return;
                _selectedWork = value;
                OnPropertyChanged("SelectedWork");
            }
        }

        public event EventHandler AddWorkHandler;
        protected virtual void OnAddWorkHandler()
        {
            if (AddWorkHandler != null) AddWorkHandler(this, EventArgs.Empty);
        }

        public event EventHandler ShowHistoryHandler;
        protected virtual void OnShowHistoryHandler()
        {
            if (ShowHistoryHandler != null) ShowHistoryHandler(this, EventArgs.Empty);
        }

        public bool VisableCloseCommand
        {
            get { return _visableCloseCommand; }
            protected set
            {
                if (_visableCloseCommand == value) return;
                _visableCloseCommand = value;
                OnPropertyChanged("VisableCloseCommand");
            }
        }

        public int ErrorsCount
        {
            get { return _errorsCount; }
            set
            {
                _errorsCount = value;
                this.OnPropertyChanged("ErrorsCount");
            }
        }

        public Visibility VisErrors
        {
            get { return _visErrors; }
            set
            {
                _visErrors = value;
                this.OnPropertyChanged("VisErrors");
            }
        }

        private RelayCommand _showWorksCommand;
        public RelayCommand ShowWorksCommand
        {
            get { return _showWorksCommand ?? (_showWorksCommand = new RelayCommand(ShowWorks)); }
        }

        public event EventHandler ShowWorksList;

        private void ShowWorks(object sender)
        {
            if (ShowWorksList != null)
                ShowWorksList(this, new EventArgs());
        }

        private RelayCommand _closeViewCommand;
        public RelayCommand CloseViewCommand
        {
            get
            {
                return _closeViewCommand ?? (_closeViewCommand = new RelayCommand(CloseView));
            }
        }


        private void CloseView(object sender)
        {
            if (CloseViewClick != null)
                CloseViewClick(this, new EventArgs());
        }

        private void DisplayLogoImage()
        {
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri("pack://application:,,,/DTCDev.Client.Cars.Service;component/Assets/CarMarks/"+_model.Mark+".png");
            logo.EndInit();
            LogoImage = logo;
        }
    }
}
