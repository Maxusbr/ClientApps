using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
        private readonly Task _listener;

        public static TcpConnection Instance
        {
            get { return _instance ?? (_instance = new TcpConnection()); }
        }

        public TcpConnection()
        {
            _instance = this;
            _hostName = new HostName(Adress);
            //_listener = (new Task(_listener_ConnectionReceived));
        }

        public async Task Connect()
        {
            try
            {
                await _socket.ConnectAsync(_hostName, Port.ToString(), SocketProtectionLevel.PlainSocket);
                //_listener.Start();
            }
            catch (Exception e)
            {

            }
        }

        private async Task<string> _listener_ConnectionReceived()
        {
            var reader = new DataReader(_socket.InputStream);
            try
            {
                var sizeFieldCount = await reader.LoadAsync(sizeof(uint));
                if (sizeFieldCount != sizeof(uint))
                {
                    return "";
                }
                var bLenght = new byte[sizeFieldCount];
                reader.ReadBytes(bLenght);

                var lenght = BitConverter.ToUInt32(bLenght, 0);

                var actualStringLength = await reader.LoadAsync(lenght);
                return lenght != actualStringLength ? "" : reader.ReadString(actualStringLength);
            }
            catch (Exception exception)
            {
                return exception.Message;
                if (SocketError.GetStatus(exception.HResult) == SocketErrorStatus.Unknown)
                {
                    //throw;
                }
            }
        }

        public async static Task<ReciveMessageModel> Send(string msg)
        {
            return await Instance.SendAsync(msg);
        }

        private async Task<ReciveMessageModel> SendAsync(string msg)
        {
            await SendPacket(Encoding.UTF8.GetBytes(msg));
            return Split(await _listener_ConnectionReceived());
        }

        protected async Task SendPacket(byte[] p)
        {
            try
            {
                var header = BitConverter.GetBytes(p.Length);
                var packet = new byte[p.Length + sizeof(uint)];
                Array.Copy(header, packet, sizeof(uint));
                Array.Copy(p, 0, packet, sizeof(uint), p.Length);
                var writer = new DataWriter(_socket.OutputStream);
                writer.WriteBytes(packet);
                await writer.StoreAsync();
            }
            catch (Exception e)
            {

            }
        }

        ReciveMessageModel Split(string msg)
        {
            return msg.Length < 2 ? new ReciveMessageModel{Msg = "error"} : 
                new ReciveMessageModel { Px = msg[0], Fx = msg[1], Msg = msg.Substring(2) };
        }
    }
}
