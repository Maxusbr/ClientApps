using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace DTCDev.Client.Cars.Service.Engine.Network
{
    public class ClientCl : SocketClient
    {
        public ClientCl(string addr, int port) : base(addr, port)
        {
        }

        protected override void OnConnected()
        {
            //todo: do something when connection success
        }

        protected override void OnPacketReceived(byte[] packet)
        {
            string row = Encoding.UTF8.GetString(packet);
            if (MessageReceived != null)
                MessageReceived(row);
        }

        public void YourFuckingRequest(string msg)
        {
            SendPacket(Encoding.UTF8.GetBytes(msg));
        }

        public delegate void MessageHandler(string msg);
        public event MessageHandler MessageReceived;
    }
}

