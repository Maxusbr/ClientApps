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
using System.Windows.Shapes;

namespace M2B_Cars
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
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
                _isLogin = true;
                this.Close();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
