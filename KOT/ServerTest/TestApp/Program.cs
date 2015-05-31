using KOTServerTester.Handlers;
using KOTServerTester.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KOTServerTester
{
    class Program
    {
        static void Main(string[] args)
        {
            TCPConnection.Instance.Start();
            Login();
            while (true)
            {
                string cmd = Console.ReadLine();
                HandleCMD(cmd);
            }
        }

        private static void Login()
        {
            LoginHandler.Instance.LoginComplete += Instance_LoginComplete;
            LoginHandler.Instance.SetLogin("test", "test123");
            LoginHandler.Instance.StartAuth();
        }

        static void Instance_LoginComplete(object sender, EventArgs e)
        {
            WriteM("Login complete");
            WriteM("Start load cars");
            CarsHandler.Instance.GetCars();
        }

        public static void WriteM(string message, MessageTypes type = MessageTypes.Message)
        {
            string prefix = "";
            switch(type)
            {
                case  MessageTypes.Message:
                    Console.ForegroundColor = ConsoleColor.Green;
                    prefix = " SYSTEM MSG: ";
                    break;
                case MessageTypes.Send:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    prefix = " SENDING: ";
                    break;
                case MessageTypes.Receive:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    prefix = " RECEIVED: ";
                    break;
                case MessageTypes.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    prefix = " ERROR: ";
                    break;
            }
            Console.WriteLine(DateTime.Now.ToString("dd.MM.yy hh:mm:ss") + prefix + message);
        }

        public enum MessageTypes
        {
            Error,
            Send,
            Receive,
            Message
        }


        private static void HandleCMD(string cmd)
        {
            switch(cmd)
            {
                case "help":
                    PrintHelp();
                    break;
                case "BB":
                    CarsHandler.Instance.GetCarState();
                    break;
                case "BC":
                    {
                        Console.WriteLine("Pls device ID:");
                        string devID = Console.ReadLine();
                        CarsHandler.Instance.GetCarWorks(devID);
                    }
                    break;
                case "BD":
                    {
                        Console.WriteLine("Pls device ID:");
                        string devID = Console.ReadLine();
                        CarsHandler.Instance.GetCarWorksHistory(devID);
                    }
                    break;
                case "DC":
                    {
                        GeoHandler.Instance.GetGeoTypes();
                    }
                    break;
                case "BE":
                    CarsHandler.Instance.GetAlarmsState();
                    break;
                default:
                    WriteM("Unknown command. Pls help to view commands list");
                    break;
            }
        }

        private static void PrintHelp()
        {
            Console.WriteLine("*************************************");
            Console.WriteLine("BB - get car state (BB request)");
            Console.WriteLine("BC - get works for one car(BC request)");
            Console.WriteLine("BD - get work history (BD request)");
            Console.WriteLine("BE - get alarms list (BE request)");
            Console.WriteLine("DC - get geo object classes");
            Console.WriteLine("*************************************");
        }
    }
}
