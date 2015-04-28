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

namespace DTCDev.Client.Cars.Controls.Controls
{
    /// <summary>
    /// Логика взаимодействия для LoginControl.xaml
    /// </summary>
    public partial class LoginControl : UserControl
    {
        public LoginControl()
        {
            InitializeComponent();
        }

        public event EventHandler TryLogin;
        protected virtual void OnTryLogin()
        {
            if (TryLogin != null) TryLogin(this, EventArgs.Empty);
        }

        public event EventHandler CancelLogin;
        protected virtual void OnCancelLogin()
        {
            if (CancelLogin != null) CancelLogin(this, EventArgs.Empty);
        }

        private bool _isLogin = false;
        public bool IsLogin
        {
            get { return _isLogin; }
        }

        private string _lgn;
        private string _pwd;

        public string Login
        {
            get { return _lgn; }
        }

        public string Password
        {
            get { return _pwd; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (txtLogin.Text != null && txtLogin.Text.Length > 1 && txtPassword.Password != null && txtPassword.Password.Length > 1)
            {
                _lgn = txtLogin.Text;
                _pwd = txtPassword.Password;
                OnTryLogin();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OnCancelLogin();
        }

        private void Button_Click3(object sender, RoutedEventArgs e)
        {
            _lgn = "demo";
            _pwd = "demo123";
            OnTryLogin();
        }
    }
}
