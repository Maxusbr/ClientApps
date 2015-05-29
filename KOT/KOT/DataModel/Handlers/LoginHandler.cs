using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KOT.DataModel.Network;

namespace KOT.DataModel.Handlers
{
    public class LoginHandler
    {
        private static LoginHandler _instance;

        public static LoginHandler Instance
        {
            get { return _instance ?? (_instance = new LoginHandler()); }
        }

        public LoginHandler()
        {
            _instance = this;
        }

        private string _login = string.Empty;
        private string _password = string.Empty;
        private bool _isAuth;

        public static async Task<bool> Login(string user, string pass)
        {
            return await Instance.LoginAsync(user, pass);
        }

        private async Task<bool> LoginAsync(string user, string pass)
        {
            _login = user;
            _password = pass;
            if (_login == string.Empty || _password == string.Empty) return false;
            _isAuth = (await TcpConnection.Send(string.Format("AA{0};{1}", _login, _password))).Msg.Equals("OK");

            return _isAuth;
        }
    }
}
