using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace DTCDev.Client.Cars.Engine.Handlers
{
    public class LoginHandler
    {
        private static LoginHandler _instance;

        public static LoginHandler Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new LoginHandler();
                return _instance;
            }
        }

        public LoginHandler()
        {
            _instance = this;
        }


        private string _login = string.Empty;
        private string _password = string.Empty;
        private bool _isAuth = false;

        private bool _isParamsAdded = false;
        public bool IsParamsAdded
        {
            get { return _isParamsAdded; }
            set { _isParamsAdded = value; }
        }

        public event EventHandler LoginError;

        public void SetLogin(string l, string p)
        {
            _login = l;
            _password = p;
        }

        public void StartAuth()
        {
            if (_login != string.Empty && _password != string.Empty)
            {
                try
                {
                    string req = "A0" + _login + ":" + _password;
                    TCPConnection.Instance.SendLogin(req);
                }
                catch { }
            }
        }

        public event EventHandler LoginComplete;


        public void Split(char fx, string row)
        {
            if (row == "OK")
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    _isAuth = true;
                    if (LoginComplete != null)
                        LoginComplete(this, new EventArgs());
                    Cars.CarsHandler.Instance.Update();
                    TCPConnection.Instance.SetLogined();
                }));
            }
            else
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (LoginError != null) LoginError(this, new EventArgs());
                }));
            }
        }
    }
}
