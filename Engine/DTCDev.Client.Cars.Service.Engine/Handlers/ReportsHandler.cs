using DTCDev.Client.Cars.Service.Engine.Network;
using DTCDev.Models.CarsSending.Order;
using DTCDev.Models.CarsSending.Report;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace DTCDev.Client.Cars.Service.Engine.Handlers
{
    public class ReportsHandler
    {
        private static ReportsHandler _instance;

        public static ReportsHandler Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ReportsHandler();
                return _instance;
            }
        }

        public ReportsHandler()
        {
            _instance = this;
        }





        public delegate void OrdersReportLoadedHandler(List<OrderModel> data);
        public event OrdersReportLoadedHandler OrderReportLoaded;





        public void GetOrdersReport(DateTime dtStart, DateTime dtStop)
        {
            ReportRequestBase req = new ReportRequestBase();
            req.Start = new Models.Date.DateDataModel
            {
                D = dtStart.Day,
                M = dtStart.Month,
                Y = dtStart.Year
            };
            req.Stop = new Models.Date.DateDataModel
            {
                D = dtStop.Day,
                M = dtStop.Month,
                Y = dtStop.Year
            };
            string row = JsonConvert.SerializeObject(req);
            SendRequest("RA"+row);
        }




        private void SendRequest(string req)
        {
            try
            {
                TCPConnection.Instance.SendData(req);
            }
            catch { }
        }




        public void Split(char fx, string row)
        {
            switch (fx)
            {
                case 'a':
                case 'A':
                    FillOrderReport(row);
                    break;
            }
        }

        private void FillOrderReport(string row)
        {
            try
            {
                List<OrderModel> data = JsonConvert.DeserializeObject<List<OrderModel>>(row);
                if (data != null)
                    if (Application.Current != null)
                        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                if (OrderReportLoaded != null)
                                    OrderReportLoaded(data);
                            }));
            }
            catch { }
        }
    }
}
