using DTCDev.Client.Cars.Service.Engine.Storage;
using DTCDev.Client.ViewModel;
using DTCDev.Models.CarsSending.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;

namespace DTCDev.Client.Cars.Service.Engine.Controls.ViewModels.Slides
{
    public class SlideHomeViewModel : ViewModelBase
    {
        public SlideHomeViewModel()
        {
            CarStorage.Instance.LoadComplete += Instance_LoadComplete;
            CarStorage.Instance.ErrorsLoaded += Instance_ErrorsLoaded;
            OrderStorage.Instance.OrdersRefreshed += Instance_OrdersRefreshed;
            OrderStorage.Instance.UpdateOrders();
            PersonalStorage.Instance.LoadMastersComplete += Instance_LoadMastersComplete;
            PersonalStorage.Instance.LoadCurrentMasterComplete += Instance_LoadCurrentMasterComplete;
            PersonalStorage.Instance.LoadMasters();
            if (UserSettingsStorage.Instance.FirstLoad == false)
            {
                ProgressVol = 100;
                DisplayOrders();
                DisplayErrors();
                DisplayCars();
                FillCurrentMaster();
                FillMasters();
            }
            else
                UserSettingsStorage.Instance.FirstLoad = false;
            CheckLoadComplete();

            new Thread(StartAddNews).Start();
        }







        private HomeDataModel _carsModel = new HomeDataModel
        {
            Color = HomeDataModel.BGColor.Blue,
            Text = "Авто",
            MainComment1 = "Авто всего",
            SecondComment2 = "Требуют ТО",
            SecondComment1 = "Check engine",
            Image = "checkEngineImage.jpg"
        };
        public HomeDataModel CarsModel
        {
            get { return _carsModel; }
            set
            {
                _carsModel = value;
                this.OnPropertyChanged("CarsModel");
            }
        }

        private HomeDataModel _ordersModel = new HomeDataModel
        {
            Color = HomeDataModel.BGColor.Green,
            Text = "Заказ-наряды",
            MainComment1 = "Всего заказов",
            SecondComment2 = "На сумму",
            AnimatePosition = 1,
            Image = "money.jpg"
        };
        public HomeDataModel OrdersModel
        {
            get { return _ordersModel; }
            set
            {
                _ordersModel = value;
                this.OnPropertyChanged("OrdersModel");
            }
        }





        private int _progressVol = 0;
        public int ProgressVol
        {
            get { return _progressVol; }
            set
            {
                _progressVol = value;
                this.OnPropertyChanged("ProgressVol");
            }
        }

        private string _currentMasterName = "Не указан";
        public string CurrentMasterName
        {
            get { return _currentMasterName; }
            set
            {
                _currentMasterName = value;
                this.OnPropertyChanged("CurrentMasterName");
            }
        }

        private Visibility _visLoading = Visibility.Visible;
        public Visibility VisLoading
        {
            get { return _visLoading; }
            set
            {
                _visLoading = value;
                this.OnPropertyChanged("VisLoading");
            }
        }

        private ObservableCollection<string> _news = new ObservableCollection<string>();
        public ObservableCollection<string> News
        {
            get { return _news; }
            set { _news = value; }
        }

        private readonly ObservableCollection<MasterDataModel> _masters = new ObservableCollection<MasterDataModel>();
        public ObservableCollection<MasterDataModel> Masters
        {
            get { return _masters; }
        }

        private bool _saveMasterEnabled = false;
        public bool SaveMasterEnabled
        {
            get { return _saveMasterEnabled; }
            set
            {
                _saveMasterEnabled = value;
                this.OnPropertyChanged("SaveMasterEnabled");
            }
        }

        private MasterDataModel _selectedMaster;
        public MasterDataModel SelectedMaster
        {
            get { return _selectedMaster; }
            set
            {
                _selectedMaster = value;
                this.OnPropertyChanged("SelectedMaster");
                if (value == null)
                    SaveMasterEnabled = false;
                else
                    SaveMasterEnabled = true;
            }
        }




        private RelayCommand _saveMasterCommand;
        public RelayCommand SaveMasterCommand { 
            get 
            { 
                return _saveMasterCommand ?? (_saveMasterCommand = new RelayCommand(SaveMaster)); 
            } 
        }
        private void SaveMaster(object sender)
        {
            if (SelectedMaster != null)
            {
                PersonalStorage.Instance.SetCurrentMaster(SelectedMaster);
                FillCurrentMaster();
            }
        }






        void Instance_OrdersRefreshed(object sender, EventArgs e)
        {
            ProgressVol += 30;
            DisplayOrders();
            CheckLoadComplete();
        }

        void Instance_ErrorsLoaded(object sender, EventArgs e)
        {
            ProgressVol += 30;
            DisplayErrors();
            CheckLoadComplete();
        }

        void Instance_LoadComplete(object sender, EventArgs e)
        {
            ProgressVol += 28;
            DisplayCars();
            CheckLoadComplete();
        }

        void Instance_LoadMastersComplete(object sender, EventArgs e)
        {
            ProgressVol += 10;
            FillMasters();
            CheckLoadComplete();
        }

        void Instance_LoadCurrentMasterComplete(object sender, EventArgs e)
        {
            ProgressVol += 2;
            FillCurrentMaster();
            CheckLoadComplete();
            
        }

        private void DisplayCars()
        {
            CarsModel.MainText1 = CarStorage.Instance.Cars.Count().ToString();
            CarsModel.SecondText2 = (CarStorage.Instance.Cars.Where(p => p.CarModel.DaysToService < 0).Count()).ToString();
        }

        private void DisplayErrors()
        {
            CarsModel.SecondText1 = (CarStorage.Instance.Cars.Where(p => p.Errors.Count > 0).Count()).ToString();
        }

        private void DisplayOrders()
        {
            OrdersModel.MainText1 = OrderStorage.Instance.CurrnetOrders.Count().ToString();
            OrdersModel.SecondText2 = OrderStorage.Instance.CurrnetOrders.Sum(p => p.Cost).ToString();
        }
        
        private void FillMasters()
        {
            Masters.Clear();
            PersonalStorage.Instance.Masters.ForEach(x => Masters.Add(x));
        }

        private void FillCurrentMaster()
        {
            if (PersonalStorage.Instance.CurrentMaster != null)
            {
                CurrentMasterName = PersonalStorage.Instance.CurrentMaster.Name;
            }
        }






        private void CheckLoadComplete()
        {
            if (ProgressVol >= 100)
                VisLoading = Visibility.Collapsed;
        }

        private void StartAddNews()
        {
            Thread.Sleep(1000);
            AddNews("Добавлена функция гашения ошибок");
            Thread.Sleep(100);
            AddNews("Добавлена работа с пользователями программы");
            Thread.Sleep(100);
            AddNews("Добавлена форма обратной связи и форма заказа оборудования. Подробнее в разделе \"Помощь\"");
            Thread.Sleep(100);
            AddNews("Добавлены мастера приемщики и выбор мастера, находящегося сейчас в смене");
            Thread.Sleep(100);
            AddNews("Добавлена возможность просмотра справки по программе");
            Thread.Sleep(100);
            AddNews("Добавлена функция просмотра технологической карты при работе с заказ-нарядами");
            Thread.Sleep(100);
            AddNews("Добавлено отображение нормо-часов при создании заказ-наряда и автоматический подсчет стоимости работ");
            Thread.Sleep(100);
            AddNews("Добавлен раздел ЧТО НОВОГО :))))");
        }

        private void AddNews(string text)
        {
            if (Application.Current != null)
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        News.Add(text);
                    }));
        }

    }
}
