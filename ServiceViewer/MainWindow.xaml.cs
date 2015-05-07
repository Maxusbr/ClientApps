using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DTCDev.Client.Cars.Service;
using DTCDev.Client.Cars.Service.Controls;
using DTCDev.Client.Cars.Service.Controls.Car;
using DTCDev.Client.Cars.Service.Engine.Controls.View;
using DTCDev.Client.Cars.Service.Engine.Controls.ViewModels;
using DTCDev.Client.Cars.Service.Engine.Handlers;
using DTCDev.Client.Cars.Service.Engine.Network;
using DTCDev.Client.Cars.Service.Slides;
using DTCDev.Client.Window.SlideNavigation;
using DTCDev.Models.CarBase.CarList;
using DTCDev.Client.Cars.Service.Slides.Settings;
using DTCDev.Client.Cars.Service.Engine.Storage;
using DTCDev.Client.Cars.Service.Slides.Clients;
using DTCDev.Client.Cars.Service.SideMenu;
using MahApps.Metro.Controls;
using DTCDev.Client.Cars.Service.Slides.Reports;

namespace CarServiceViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            CarsHandler.Instance.OnGetCarDetailComplete += Instance_OnGetCarDetailComplete;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.ExceptionObject.ToString() + " " + e.ToString());
        }

        void Instance_OnGetCarDetailComplete(DTCDev.Models.CarBase.CarList.CarListDetailsDataModel carDetail)
        {
            VM_SelectedCarDetailsComplete(carDetail);
        }

        void MainWindow_LoginCancel(object sender, EventArgs e)
        {
            Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            StorageController.Stop();
            Environment.Exit(0);
        }

        LoginView lgnView = new LoginView();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            menu.Visibility = Visibility.Collapsed;
            StorageController.Start();
            TCPConnection.Instance.Start();
            LoginHandler.Instance.LoginComplete += Instance_LoginComplete;
            LoginHandler.Instance.LoginError += Instance_LoginError;
            LoginViewModel.Instance.LoginCancel += Instance_LoginCancel;
            grdLogin.Children.Add(lgnView);
        }











        void Instance_LoginCancel(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        void Instance_LoginError(object sender, EventArgs e)
        {
            MessageBox.Show("Ошибка ввода логина или пароля. Попробуйте еще раз.");
        }

        void Instance_LoginComplete(object sender, EventArgs e)
        {
            DisplayStartPage();
            menu.Visibility = Visibility.Visible;
            CarStorage.Instance.Start();
            PersonalHandler.Instance.GetUserData();
            grdLogin.Visibility = Visibility.Collapsed;
            //DisplayCarList();
            ApplySettings();
        }

        private void DisplayStartPage()
        {
            SlideHome slide = new SlideHome();
            slide.ClickCars += slide_ClickCars;
            slide.ClickOrders += slide_ClickOrders;
            grdContent.Children.Clear();
            grdContent.Children.Add(slide);
        }

        void slide_ClickOrders(object sender, EventArgs e)
        {
            DisplayOrders();
        }

        void slide_ClickCars(object sender, EventArgs e)
        {
            DisplayCarList();
        }

        private void ApplySettings()
        {
            ManageLastCarsView();
            if (UserSettingsStorage.Instance.UserSettings.OpenedHelp)
                DisplayHelp();
        }




        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            DisplayCarList();
        }


        void VM_SelectedCarDetailsComplete(DTCDev.Models.CarBase.CarList.CarListDetailsDataModel carDetail)
        {
            DISP_Car car = CarStorage.Instance.Cars.Where(p => p.CarModel.CarNumber == carDetail.CarNumber).FirstOrDefault();
            if (car == null)
                return;
            CarStorage.Instance.SelectedCar = car;
            CarStorage.Instance.SelectedCarDetails = carDetail;
            CarDetailView view = new CarDetailView();

            CarDetailViewModel vm = new CarDetailViewModel(carDetail, car.ErrorsCount);
            view.DataContext = vm;
            vm.ShowWorksList += vm_ShowWorksList;
            vm.CloseViewClick += vm_CloseViewClick;
            grdContent.Children.Clear();
            grdContent.Children.Add(view);
        }

        void vm_CloseViewClick(object sender, EventArgs e)
        {
            DisplayCarList();
        }

        void vm_ShowWorksList(object sender, EventArgs e)
        {
            CarDetailViewModel vm = (CarDetailViewModel)sender;
            SlideSettingsWorks ssw = new SlideSettingsWorks(vm.CarNumber);
            grdContent.Children.Clear();
            grdContent.Children.Add(ssw);
            //naviPanel.AddFrame(control);
        }

        private void menuItem_DicCars(object sender, RoutedEventArgs e)
        {
            SlideCarsInfo view = new SlideCarsInfo();
            grdContent.Children.Clear();
            grdContent.Children.Add(view);
        }

        private void menuItem_ClickCalendar(object sender, RoutedEventArgs e)
        {
            SlideCalendar view = new SlideCalendar();
            grdContent.Children.Clear();
            grdContent.Children.Add(view);
        }

        private void menuItem_ClickaddCar(object sender, RoutedEventArgs e)
        {
            SlideAddCar view = new SlideAddCar();
            grdContent.Children.Clear();
            grdContent.Children.Add(view);
        }

        private void menuItem_ClickWorkList(object sender, RoutedEventArgs e)
        {
            SlideSettingsWorks view = new SlideSettingsWorks();
            grdContent.Children.Clear();
            grdContent.Children.Add(view);
        }

        private void menuItem_ClickPersonalSettings(object sender, RoutedEventArgs e)
        {
            SlideSettingsPersonal view = new SlideSettingsPersonal();
            grdContent.Children.Clear();
            grdContent.Children.Add(view);
        }

        private void menuItem_ClickWorkDic(object sender, RoutedEventArgs e)
        {
            SlideSettingsWorkDic view = new SlideSettingsWorkDic();
            grdContent.Children.Clear();
            grdContent.Children.Add(view);
        }

        private void MenuItem_ClickClientsSend(object sender, RoutedEventArgs e)
        {
            SlideClientsConnect view = new SlideClientsConnect();
            grdContent.Children.Clear();
            grdContent.Children.Add(view);
        }

        private void MenuItem_ClickClientList(object sender, RoutedEventArgs e)
        {
            SlideClientsList view = new SlideClientsList();
            grdContent.Children.Clear();
            grdContent.Children.Add(view);
        }

        private void MenuItem_ClickClientsStat(object sender, RoutedEventArgs e)
        {
            SlideClientsStat view = new SlideClientsStat();
            grdContent.Children.Clear();
            grdContent.Children.Add(view);
        }

        private void MenuItem_ClickViewLastCars(object sender, RoutedEventArgs e)
        {
            UserSettingsStorage.Instance.UserSettings.OpenedLastCars = !UserSettingsStorage.Instance.UserSettings.OpenedLastCars;
            ManageLastCarsView();
        }

        private void ManageLastCarsView()
        {
            if (UserSettingsStorage.Instance.UserSettings.OpenedLastCars==false)
                grd1_1.Children.Clear();
            else
            {
                SM_LastCars view = new SM_LastCars();
                view.ClickClose += view_ClickClose;
                grd1_1.Children.Add(view);
            }
        }

        void view_ClickClose(object sender, EventArgs e)
        {
            grd1_1.Children.Clear(); 
            UserSettingsStorage.Instance.UserSettings.OpenedLastCars = !UserSettingsStorage.Instance.UserSettings.OpenedLastCars;
        }

        private void menuItem_Orders(object sender, RoutedEventArgs e)
        {
            DisplayOrders();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            DisplayStartPage();
        }



        private void DisplayOrders()
        {
            SlideOrdersList view = new SlideOrdersList();
            grdContent.Children.Clear();
            grdContent.Children.Add(view);
        }

        private void DisplayCarList()
        {
            try
            {
                SlideCarsList sv = new SlideCarsList();
                grdContent.Children.Clear();
                grdContent.Children.Add(sv);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MenuItem_ClickHelpChange(object sender, RoutedEventArgs e)
        {
            if (UserSettingsStorage.Instance.UserSettings.OpenedLastCars == false)
            {
                DisplayHelp();
                UserSettingsStorage.Instance.UserSettings.OpenedHelp = true;
            }
            else
            {
                RemoveHelp();
            }
        }

        private void DisplayHelp()
        {
            SM_Help help = new SM_Help();
            help.Width = 250;
            help.CloseClick += help_CloseClick;
            grd2_1.Children.Clear();
            grd2_1.Children.Add(help);
        }

        void help_CloseClick(object sender, EventArgs e)
        {
            RemoveHelp();
        }

        private void RemoveHelp()
        {
            grd2_1.Children.Clear();
            UserSettingsStorage.Instance.UserSettings.OpenedHelp = false;
        }

        private void menuItem_ReportOrders(object sender, RoutedEventArgs e)
        {
            SlideReportOrders view = new SlideReportOrders();
            grdContent.Children.Clear();
            grdContent.Children.Add(view);
        }

        private void MenuItem_ClickFeedback(object sender, RoutedEventArgs e)
        {
            var feedback = new SM_Feedback { Width = 250 };
            feedback.CloseClick += help_CloseClick;
            grd2_1.Children.Clear();
            grd2_1.Children.Add(feedback);
        }

        private void MenuItem_ClickControllers(object sender, RoutedEventArgs e)
        {
            var controllers = new SM_Controllers { Width = 250 };
            controllers.CloseClick += help_CloseClick;
            grd2_1.Children.Clear();
            grd2_1.Children.Add(controllers);
        }

        private void menuItem_ClickSlideWorksSettings(object sender, RoutedEventArgs e)
        {
            var view = new SlideWorksSettings();
            grdContent.Children.Clear();
            grdContent.Children.Add(view);
        }
    }
}
