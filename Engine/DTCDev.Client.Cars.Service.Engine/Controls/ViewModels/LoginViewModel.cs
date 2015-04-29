using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using DTCDev.Client.Cars.Service.Engine.Network;
using DTCDev.Client.ViewModel;

namespace DTCDev.Client.Cars.Service.Engine.Controls.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private static LoginViewModel _instance;
        public static LoginViewModel Instance
        {
            get { return _instance ?? (_instance = new LoginViewModel()); }
        }

        public LoginViewModel()
        {
            _instance = this;
        }

        public event EventHandler LoginCancel;
        protected virtual void OnLoginCancel()
        {
            if (LoginCancel != null) LoginCancel(this, EventArgs.Empty);
        }

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set
            {
                if (_userName == value) return;
                _userName = value;
                OnPropertyChanged("UserName");
            }
        }

        private bool _visableLoginView = true;
        public bool VisableLoginView
        {
            get { return _visableLoginView; }
            set
            {
                if (_visableLoginView == value) return;
                _visableLoginView = value;
                OnPropertyChanged("VisableLoginView");
            }
        }

        private RelayCommand _loginCommand;
        public RelayCommand LoginCommand
        {
            get { return _loginCommand ?? (_loginCommand = new RelayCommand(OnLogin)); }
        }

        private RelayCommand _cancelComand;
        public RelayCommand CancelComand
        {
            get { return _cancelComand ?? (_cancelComand = new RelayCommand(OnCancel)); }
        }

        private RelayCommand _demologinCommand;
        public RelayCommand DemoLoginCommand
        {
            get { return _demologinCommand ?? (_demologinCommand = new RelayCommand(OnDemoLogin)); }
        }

        private void OnDemoLogin(object obj)
        {
            LoginHandler.Instance.SetLogin("demo", "demo123");
            LoginHandler.Instance.StartAuth();
        }

        private void OnCancel(object obj)
        {
            OnLoginCancel();
        }


        private void OnLogin(object obj)
        {
            var passbox = (System.Windows.Controls.PasswordBox)obj;
            if (passbox == null) return;
            if(string.IsNullOrEmpty(UserName))
            {
                MessageBox.Show("Введите имя пользователя");
                return;
            }
            if (string.IsNullOrEmpty(passbox.Password))
            {
                MessageBox.Show("Введите пароль");
                return;
            }
            LoginHandler.Instance.SetLogin(UserName, passbox.Password);
            LoginHandler.Instance.StartAuth();
        }
    }
}
