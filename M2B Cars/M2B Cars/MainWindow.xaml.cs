﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DTCDev.Client.Cars.Controls.Controls;
using DTCDev.Client.Cars.Controls.Controls.Car;
using DTCDev.Client.Cars.Controls.Controls.Driver;
using DTCDev.Client.Cars.Controls.Controls.History;
using DTCDev.Client.Cars.Controls.Controls.Reports;
using DTCDev.Client.Cars.Controls.Controls.Settings;
using DTCDev.Client.Cars.Engine.Handlers;
using DTCDev.Client.Cars.Engine.Handlers.Cars;
using DTCDev.Client.Window;
using System.IO;
using DTCDev.Client.Cars.Engine.AppLogic;
using DTCDev.Client.Busy;

namespace M2B_Cars
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Thread tr = new Thread(threadDisconnect);
            tr.Start();
            _login = new LoginControl();
            _login.TryLogin += _login_TryLogin;
            _login.CancelLogin+=_login_CancelLogin;
            CarsHandler.Instance.Init();
            CarBaseHandler.Instance.Init();
            //CarsHandler.Instance.CarsRefreshed += Instance_CarsRefreshed;
            ZonesHandler.Instance.Init();
            DTCDev.Client.Cars.Engine.Handlers.UpdateDriver.Instance.Start();
            LoginHandler.Instance.LoginError += Instance_LoginError;
            LoginHandler.Instance.LoginComplete += Instance_LoginComplete;
            CarSelector.OnCarChanged += CarSelector_OnCarChanged;
            CarSelector.ViewCarDetails += CarSelector_ViewCarDetails;
            DTCDev.Client.Cars.Manager.Data.CarDataStorage.Instance.Update();
            FolderPrecreate();

            if (DTCDev.Client.Cars.Engine.Handlers.LoginHandler.Instance.IsParamsAdded == false)
            {
                DisplayLogin();
            }
        }

        void CarSelector_ViewCarDetails(DTCDev.Client.Cars.Engine.DisplayModels.DISP_Car car)
        {
            if (_details == null)
            {
                _details = new CarDetailsView();
                _details.CloseMe += _details_CloseMe;
                grdDetails.Children.Add(_details);
            }

            _details.UpdateCarData(car);
        }

        private void _login_CancelLogin(object sender, EventArgs e)
        {

            MessageBox.Show("Перед работой с приложением необходимо авторизоваться. Приложение будет закрыто, попробуйте еще раз");
            this.Close();
        }

        void _login_TryLogin(object sender, EventArgs e)
        {
            LayoutRoot.Content = new MacBusyControl { IsWaiting = true };
            LoginHandler.Instance.SetLogin(_login.Login, _login.Password);
            LoginHandler.Instance.StartAuth();            
        }

        CarDetailsView _details;

        void CarSelector_OnCarChanged(DTCDev.Client.Cars.Engine.DisplayModels.DISP_Car car)
        {
            if (_details != null)
            {
                _details.UpdateCarData(car);
            }
        }

        void _details_CloseMe(object sender, EventArgs e)
        {
            _details.CloseMe -= _details_CloseMe;
            grdDetails.Children.Clear();
            _details = null;
        }

        private void FolderPrecreate()
        {
            string myDocs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (Directory.Exists(myDocs + "\\M2B") == false)
                Directory.CreateDirectory(myDocs + "\\M2B");
            if (Directory.Exists(myDocs + "\\M2B\\Cars") == false)
                Directory.CreateDirectory(myDocs + "\\M2B\\Cars");
            //windowsManger.SettingsPath = string.Format(@"{0}\{1}",
            //    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\M2B\\Cars", "windows.m2bs");
        }

        private void DisplayLogin()
        {
            LayoutRoot.Content = _login;
        }

        void Instance_LoginError(object sender, EventArgs e)
        {
            MessageBox.Show("Ошибка ввода логина/пароля");
            DisplayLogin();
        }

        private bool _firstOpenWidowsCompleted = false;
        private LoginControl _login;
        void Instance_LoginComplete(object sender, EventArgs e)
        {
            LayoutRoot.Visibility = Visibility.Collapsed;
            if (_firstOpenWidowsCompleted == true)
                return;
        }


        private void Window_Closed(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
            Environment.Exit(0);
        }

        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var set = sender as SettingsModel;
            DTCDev.Client.Cars.Controls.Controls.Car.CarsView view = new DTCDev.Client.Cars.Controls.Controls.Car.CarsView();
            //ShowWindow(600, 500, view, "Автомобили", true, "", set);
        }

        private void Image_MouseLeftButtonUp1(object sender, MouseButtonEventArgs e)
        {
            var set = sender as SettingsModel;
            DTCDev.Client.Cars.Controls.Controls.Map.MapView view = new DTCDev.Client.Cars.Controls.Controls.Map.MapView();
            var mvm = view.DataContext as DTCDev.Client.Cars.Controls.ViewModels.Map.MapViewModel;
            if (mvm != null) mvm.PropertyChanged += MapView_PropertyChanged;
            if (ErrorLog.DataContext == null)
                ErrorLog.DataContext = DTCDev.Client.Cars.Controls.ViewModels.Car.CarZonesErrorViewModel.Instance;
            //ShowWindow(600, 500, view, "Карта", true);
        }

        private void MapView_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("ShowErrorLog"))
            {
                ErrorLog.Visibility = Visibility.Visible;
            }
            if (e.PropertyName.Equals("HideErrorLog"))
            {
                ErrorLog.Visibility = Visibility.Collapsed;
            }
        }



        private void threadDisconnect()
        {
            while (true)
            {
                Thread.Sleep(1000);
                if (Application.Current != null)
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            if (TCPConnection.Instance.IsConnected == false)
                                brdrDisconnect.Visibility = Visibility.Visible;
                            else
                                brdrDisconnect.Visibility = Visibility.Collapsed;
                        }));
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            CarSelector.OnClearSelection();
            ContentGrid.Visibility = Visibility.Collapsed;
            ccContent.Content = null;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var set = sender as SettingsModel;
            ccContent.Content = new ReportsList();
            ContentGrid.Visibility = Visibility.Visible;
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            var set = sender as SettingsModel;
            ccContent.Content = new DriversControl();
            ContentGrid.Visibility = Visibility.Visible;
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            var set = sender as SettingsModel;
            ccContent.Content = new SettingsBase();
            ContentGrid.Visibility = Visibility.Visible;
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            MapView.Visibility = MapView.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            HistoryControl.Visibility = HistoryControl.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            HistoryControl.HistoryButton_Click();
        }
    }
}
