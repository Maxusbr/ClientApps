using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using DTCDev.Client.Cars.Service.Engine.Handlers;

namespace DTCDev.Client.Cars.Service.Engine.Network
{
    public class TCPConnection
    {
        ClientCl _socket;
#if DEBUG
        private const string _address = "127.0.0.1";
#else
        private const string _address = "195.208.184.73";
#endif
        private const int _port = 4557;
        private bool _isLogined = false;

        List<string> sendQuery = new List<string>();

        private static TCPConnection _instance;

        public static TCPConnection Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new TCPConnection();
                return _instance;
            }
        }

        public TCPConnection()
        {
            _instance = this;
        }

        public bool IsConnected
        {
            get { return _socket.IsConnected; }
        }

        public bool IsLogined
        {
            get { return _isLogined; }
        }

        public void Start()
        {
            try
            {
                if (_socket == null)
                {
                    _socket = new ClientCl(_address, _port);
                    _socket.MessageReceived += _socket_MessageReceived;
                }
            }
            catch { }
            Thread tr = new Thread(ThreadConnectChecker);
            tr.Name = "TCPCHECKER";
            tr.Start();
        }

        public void SetLogined()
        {
            _isLogined = true;
            _loginRow = _tempLoginRow;
        }

        private void ThreadConnectChecker()
        {
            while (true)
            {
                try
                {
                    if (_socket == null)
                    {
                        _socket = new ClientCl(_address, _port);
                        _socket.MessageReceived += _socket_MessageReceived;
                    }
                    if (_socket.IsConnected == false)
                    {
                        _isLogined = false;
                        if (_socket != null)
                            _socket.MessageReceived -= _socket_MessageReceived;
                        _socket = new ClientCl(_address, _port);
                        _socket.MessageReceived += _socket_MessageReceived;
                        if (_loginRow != "")
                            SendLogin(_loginRow);
                    }
                    else if(_isLogined)
                    {
                        if (sendQuery.Count() > 0)
                        {
                            for (int i = sendQuery.Count()-1; i > -1; i--)
                            {
                                SendData(sendQuery[i]);
                                sendQuery.RemoveAt(i);
                                Thread.Sleep(100);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
                Thread.Sleep(1000);
            }
        }

        private string _loginRow = "";
        private string _tempLoginRow = "";

        public void SendLogin(string row)
        {
            _loginRow = _tempLoginRow = row;
            if (_socket == null)
                return;
            if (_socket.IsConnected == false)
                return;
            _socket.YourFuckingRequest(_loginRow);
            _loginRow = "";
        }

        public void SendData(string row)
        {
            if (_socket == null)
                return;
            if (_socket.IsConnected == false)
                sendQuery.Add(row);
            if (_isLogined == false)
                sendQuery.Add(row);
            _socket.YourFuckingRequest(row);
        }

        public delegate void MessageHandler(char px, char fx, string row);
        public static event MessageHandler MessageReceived;

        void _socket_MessageReceived(string msg)
        {
            if (msg.Length < 2)
                return;
            else
            {
                char px = msg[0];
                char fx = msg[1];
                string row = msg.Substring(2);
                if (px == 'a' || px == 'A')
                    LoginHandler.Instance.Split(fx, row);
                if (px == 'b' || px == 'B')
                    CarsHandler.Instance.Split(fx, row);
                if (px == 'c' || px == 'C')
                    ClientsHandler.Instance.Split(fx, row);
                if (px == 'd' || px == 'D')
                    OrdersHandler.Instance.Split(fx, row);
                if (px == 'r' || px == 'R')
                    ReportsHandler.Instance.Split(fx, row);
                if (px == 'u' || px == 'U')
                    PersonalHandler.Instance.Split(fx, row);
                if (px == 't' || px == 'T')
                    CarBaseHandler.Instance.Split(fx, row);
            }
        }


    }
}
