using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Connectivity;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using KOT.DataModel.Model;

namespace KOT.DataModel.Network
{
    public class TcpConnection
    {
        private const string Adress = "195.208.184.73";
        private readonly HostName _hostName;
        private const int Port = 4570;
        readonly StreamSocket _socket = new StreamSocket();

        private static TcpConnection _instance;
        public static TcpConnection Instance
        {
            get { return _instance ?? (_instance = new TcpConnection()); }
        }

        public TcpConnection()
        {
            _instance = this;
            _hostName = new HostName(Adress);
            IsError += TcpConnection_IsError;
            _querys.CollectionChanged += _querys_CollectionChanged;
        }

        void _querys_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {

        }

        async void TcpConnection_IsError(string msg)
        {

        }

        private readonly ObservableCollection<string> _querys = new ObservableCollection<string>();

        public delegate void ErrorEvent(string msg);
        private event ErrorEvent IsError;
        private DataWriter _writer;
        private DataReader _reader;
        private static int _cunt;

        protected virtual void OnErrorRead(string msg)
        {
            if (IsError != null) IsError(msg);
        }

        private async Task Reconect()
        {
            _socket.Dispose();
            await Connect();
        }

        public async Task Connect()
        {
            try
            {
                await _socket.ConnectAsync(_hostName, Port.ToString(), SocketProtectionLevel.PlainSocket);
                _writer = new DataWriter(_socket.OutputStream);
                _reader = new DataReader(_socket.InputStream);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        private async Task<string> _listener_ConnectionReceived()
        {

            try
            {
                var sizeFieldCount = await _reader.LoadAsync(sizeof(uint));
                if (sizeFieldCount != sizeof(uint))
                {
                    return "";
                }
                var bLenght = new byte[sizeFieldCount];
                _reader.ReadBytes(bLenght);

                var lenght = BitConverter.ToUInt32(bLenght, 0);

                var actualStringLength = await _reader.LoadAsync(lenght);
                return lenght != actualStringLength ? "" : _reader.ReadString(actualStringLength);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return "ER" + e.Message;
                if (SocketError.GetStatus(e.HResult) == SocketErrorStatus.Unknown)
                {
                    //throw;
                }
            }
        }

        public async static Task<ReciveMessageModel> Send(string msg, bool waitresult = true)
        {
            return await Instance.SendAsync(msg, waitresult);
        }

        private async Task<ReciveMessageModel> SendAsync(string msg, bool waitresult )
        {
            await Loop(_cunt);
            _cunt++;
            await SendPacket(Encoding.UTF8.GetBytes(msg));
            if (!waitresult ) return new ReciveMessageModel();
            var res = Split(await _listener_ConnectionReceived());
            if (res.Px != msg[0] || res.Fx != msg[1])
            {
                OnErrorRead(msg);
            }
            _cunt--;
            return res;
        }

        async Task Loop(int cnt)
        {
            var maxt = 0;
            while (_cunt > 0)
            {
                await Task.Delay(cnt + 53);
                maxt++;
                if (maxt < 500) continue;
                _cunt--; break;
            }
        }

        protected async Task SendPacket(byte[] p)
        {
            try
            {
                var header = BitConverter.GetBytes(p.Length);
                var packet = new byte[p.Length + sizeof(uint)];
                Array.Copy(header, packet, sizeof(uint));
                Array.Copy(p, 0, packet, sizeof(uint), p.Length);

                _writer.WriteBytes(packet);
                await _writer.StoreAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        ReciveMessageModel Split(string msg)
        {
            return msg.Length < 2 ? new ReciveMessageModel { Msg = "error" } :
                new ReciveMessageModel { Px = msg[0], Fx = msg[1], Msg = msg.Substring(2) };
        }
    }
}
