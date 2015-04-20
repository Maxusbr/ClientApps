using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace DTCDev.Client.Cars.Engine.Handlers
{
    public abstract class SocketClient
    {
        Socket clientSocket;
        Thread recThread;

		public SocketClient(string ip, int port)
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                clientSocket.Connect(ip, port);
				//Program.WriteMessage("Network connected to " + ip + ":" + port);
            }
            catch { }

			if (clientSocket.Connected)
			{
                recThread = new Thread(BeginReceive);
                recThread.Start();

                OnConnected();
			}
        }

         public bool IsConnected
         {
             get
             {
                 return clientSocket.Connected;
             }
         }

        private void BeginReceive()
        {
            while (clientSocket.Connected)
            {
                try
                {
                    byte[] b_lenght = ReceiveWithFixedLenght(4);
                    if (b_lenght == null)
                        return;
                    int lenght = BitConverter.ToInt32(b_lenght, 0);
                    byte[] packet = ReceiveWithFixedLenght(lenght);
                    if (packet == null)
                        return;

                    OnPacketReceived(packet);
                }
                catch (Exception ex)
                {
                    string exMsg = ex.Message;
                    Debug.WriteLine(exMsg);
                }
            }
        }

        byte[] ReceiveWithFixedLenght(int requstLenght)
        {
            try
            {
                byte[] b_packet = new byte[requstLenght];
                int received = 0;
                while (received < requstLenght)
                {
                    received += clientSocket.Receive(b_packet, received, requstLenght - received, SocketFlags.None);
                }
                
                return b_packet;
            }
            catch
            {
                return new byte[0];
            }
        }

        protected void SendPacket(byte[] p)
        {
            byte[] header = BitConverter.GetBytes(p.Length);
            byte[] packet = new byte[p.Length + 4];
            Array.Copy(header, packet, 4);
            Array.Copy(p, 0, packet, 4, p.Length);
            clientSocket.Send(packet);
        }

        protected abstract void OnConnected();
        protected abstract void OnPacketReceived(byte[] packet);
    }
}
