using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using DTCDev.Client.Cars.Service.Engine.Controls.View;
using DTCDev.Client.Cars.Service.Engine.Controls.View.Reports;
using DTCDev.Client.Cars.Service.Engine.Controls.View.Settings;
using DTCDev.Client.Cars.Service.Engine.Handlers;
using DTCDev.Client.Cars.Service.Engine.Network;
using DTCDev.Client.ViewModel;
using DTCDev.Client.Cars.Service.Engine.Storage;

namespace DTCDev.Client.Cars.Service.Engine.Controls.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        //private CarListViewModel CarVM;
        private RelayCommand _addCarCommand;
        private object _mainContent;
        private CarButtonsViewModel _buttonsvm;
        private readonly CarButtonsView _buttonsform = new CarButtonsView();
        private DISP_Car _selectedCar = null;
        private readonly ObservableCollection<TabItem> _tabItems = new ObservableCollection<TabItem>();
        private TabItem _selectedTab;
        private object _infoBlock;
        private RelayCommand _showReportsCommand;
        private RelayCommand _backCommand;
        private RelayCommand _showSettingsCommand;

        public MainWindowViewModel()
        {
            LoginHandler.Instance.LoginComplete += Instance_LoginComplete;
            LoginHandler.Instance.LoginError += Instance_LoginError;
            LoginViewModel.Instance.LoginCancel += Instance_LoginCancel;
            CarsHandler.Instance.OnGetCarDetailComplete += Instance_OnLoadDetailComplete;
            //CarVM = new CarListViewModel();
            //CarVM.OnSelectedCarChange += Handler_OnSelectedCarChange;
            TabItems.Add(SelectedTab = new TabItem
            {
                Header = "Список автомобилей",
                Content = new CarListView(),
                DataContext = CarVM
            });
            //InitButton();
            CancelHandler(this, EventArgs.Empty);
        }

        private void InitButton()
        {
            _buttonsvm = _buttonsform.DataContext as CarButtonsViewModel;
            if (_buttonsvm == null) return;
            _buttonsvm.AddWorkHandler += AddWorkHandler;
            _buttonsvm.ShowHistoryHandler += ShowHistoryHandler;
        }

        private void ShowHistoryHandler(object sender, EventArgs e)
        {
            //if (_selectedCar == null) return;
            var sendvm = sender as CarDetailViewModel;
            if(sendvm == null) return;
            var cntrl = new ShowHistoryView();
            var vm = cntrl.DataContext as ShowHistoryViewModel;
            if (vm != null)
            {
                vm.CancelHandler += CancelHandler;
                //vm.GetListService(sendvm.CarNumber);
            }
            InfoBlock = cntrl;
        }

        private void AddWorkHandler(object sender, EventArgs e)
        {
            //if (_selectedCar == null) return;
            var sendvm = sender as CarDetailViewModel;
            if (sendvm == null) return;
            var cntrl = new AddWorkView();
            var vm = cntrl.DataContext as AddWorkViewModel;
            if (vm != null)
            {
                vm.CancelHandler += CancelHandler;
                //vm.GetListService(sendvm.CarNumber);
            }
            InfoBlock = cntrl;
        }

        private void AddCarHandler()
        {
            var cntrl = new AddNewCarView();
            var vm = cntrl.DataContext as AddNewCarViewModel;
            if (vm != null)
                vm.CancelHandler += CancelHandler;
            InfoBlock = cntrl;
        }

        private void CancelHandler(object sender, EventArgs e)
        {
            var cntrl = sender as ICancelHandler;
            if (cntrl != null)
                cntrl.CancelHandler -= CancelHandler;
            InfoBlock = null;
            CarVM.SelectedCar = null;
        }

        private void Instance_OnLoadDetailComplete(Models.CarBase.CarList.CarListDetailsDataModel carDetail)
        {
            if (carDetail == null) return;
            DISP_Car car = CarStorage.Instance.Cars.Where(p=>p.CarModel.CarNumber==carDetail.CarNumber).FirstOrDefault();
            if(car==null)
                return;
            var vm = new CarDetailViewModel(carDetail, car.ErrorsCount);
            vm.CancelHandler += TabItemCancelHandler;
            //vm.AddWorkHandler += AddWorkHandler;
            //vm.ShowHistoryHandler += ShowHistoryHandler;
            TabItems.Add(SelectedTab = new TabItem
            {
                Header = carDetail.CarNumber,
                //Content = new CarDetailView(),
                DataContext = vm
            });
        }

        private void TabItemCancelHandler(object sender, EventArgs e)
        {
            var cntrl = sender as TabItem;
            if(cntrl == null) return;
            var vm = cntrl.DataContext as CarDetailViewModel;
            if (vm != null)
            {
                vm.CancelHandler -= TabItemCancelHandler;
                vm.AddWorkHandler -= AddWorkHandler;
                vm.ShowHistoryHandler -= ShowHistoryHandler;
            }
            TabItems.Remove(cntrl);
        }

        private void Handler_OnSelectedCarChange(object sender, EventArgs e)
        {
            _selectedCar = sender as DISP_Car;
            if (_selectedCar == null) return;
            CreateNewTabItem(_selectedCar);
            if (_buttonsvm != null)
                _buttonsvm.IsSelectedCar = true;
        }

        private void CreateNewTabItem(DISP_Car car)
        {
            var excar = TabItems.FirstOrDefault(o => o.Header.Equals(car.CarModel.CarNumber));
            if (excar == null)
                CarsHandler.Instance.GetCarDetails(car.CarModel.CarNumber);
            else
                SelectedTab = excar;
            //InfoBlock = _buttonsform;
        }

        private void Instance_LoginCancel(object sender, EventArgs e)
        {
            MessageBox.Show("Перед работой с приложением необходимо авторизоваться. Приложение будет закрыто, попробуйте еще раз");
            OnLoginCancel();
        }

        public event EventHandler LoginCancel;
        protected virtual void OnLoginCancel()
        {
            if (LoginCancel != null) LoginCancel(this, EventArgs.Empty);
        }

        private void Instance_LoginError(object sender, EventArgs e)
        {
            MessageBox.Show("Ошибка авторизации, попробуйте еще раз...");
        }

        void Instance_LoginComplete(object sender, EventArgs e)
        {
            LoginViewModel.Instance.VisableLoginView = false;
        }

        public ObservableCollection<TabItem> TabItems
        {
            get { return _tabItems; }
        }

        public TabItem SelectedTab
        {
            get { return _selectedTab; }
            set
            {
                if (Equals(_selectedTab, value)) return;
                _selectedTab = value;
                OnPropertyChanged("SelectedTab");
            }
        }
        
        public object InfoBlock
        {
            get { return _infoBlock; }
            set
            {
                if (Equals(_infoBlock, value)) return;
                _infoBlock = value;
                OnPropertyChanged("InfoBlock");
            }
        }

        public object MainContent
        {
            get { return _mainContent; }
            set
            {
                if (Equals(_mainContent, value)) return;
                _mainContent = value;
                OnPropertyChanged("MainContent");
                OnPropertyChanged("VisableMainContent");
            }
        }

        public bool VisableMainContent
        {
            get { return MainContent != null; }
        }

        #region Commands

        

        public RelayCommand AddCarCommand
        {
            get { return _addCarCommand ?? (_addCarCommand = new RelayCommand(a => AddCarHandler())); }
        }

        public RelayCommand ShowReportsCommand
        {
            get { return _showReportsCommand ?? (_showReportsCommand = new RelayCommand(ShowReports)); }
        }

        public RelayCommand ShowSettingsCommand
        {
            get { return _showSettingsCommand ?? (_showSettingsCommand = new RelayCommand(ShowSettings)); }
        }

        public RelayCommand BackCommand
        {
            get { return _backCommand ?? (_backCommand = new RelayCommand(ToBack)); }
        }

        private void ShowSettings(object obj)
        {
            MainContent = new SettingsView();
        }

        private void ShowReports(object obj)
        {
            MainContent = new ReportsView();
        }

        private void ToBack(object obj)
        {
            MainContent = null;            
        }
        #endregion
    }
}
