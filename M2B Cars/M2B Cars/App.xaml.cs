using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using DTCDev.Client.Cars.Engine.Handlers;

namespace M2B_Cars
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            TCPConnection.Instance.Start();
            if (e.Args.Length == 0)
                DTCDev.Client.Cars.Engine.Handlers.LoginHandler.Instance.IsParamsAdded = false;
            else
            {
                //try
                //{
                //    string startRow = e.Args[0];
                //    string[] temp = startRow.Split(';');
                //    if (temp.Length < 2)
                //        return;
                //    else
                //    {
                //        string pwd = String.Empty;
                //        string lgn = String.Empty;
                //        for (int i = 0; i < temp.Length; i++)
                //        {
                //            string[] one = temp[i].Split('=');
                //            if (one[0] == "lg" && one.Length > 1)
                //            {
                //                lgn = one[1];
                //            }
                //            if (one[0] == "pwd" && one.Length > 1)
                //            {
                //                pwd = one[1];
                //            }
                //        }
                //        if (lgn != String.Empty && pwd != String.Empty)
                //        {
                //            DTCDev.Client.Cars.Engine.Handlers.LoginHandler.Instance.IsParamsAdded = true;
                //            DTCDev.Client.Cars.Engine.Handlers.LoginHandler.Instance.SetLogin(lgn, pwd);
                //            DTCDev.Client.Cars.Engine.Handlers.LoginHandler.Instance.StartAuth();
                //        }
                //    }
                //}
                //catch (Exception ex)
                //{
                //    MessageBox.Show(ex.Message.ToString());
                //}
            }
        }
    }
}
