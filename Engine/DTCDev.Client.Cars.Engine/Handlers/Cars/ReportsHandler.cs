using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using DTCDev.Client.Cars.Controls.Helpers;
using DTCDev.Models.CarsSending.Report;
using Newtonsoft.Json;

namespace DTCDev.Client.Cars.Engine.Handlers.Cars
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

        public event EventHandler MialageReportLoaded;
        public event EventHandler SpeedReportLoaded;
        public event EventHandler SpeedOveralDataPreLoaded;
        public event EventHandler SpeedOveralLoaded;
        public event EventHandler FuelReportLoaded;


        private MialageReportModel _mialageReport;
        public MialageReportModel MialageReport
        {
            get { return _mialageReport; }
            set
            {
                _mialageReport = value;
                if (MialageReportLoaded != null)
                    MialageReportLoaded(this, new EventArgs());
            }
        }

        private SpeedReportModel _speedReport;
        public SpeedReportModel SpeedReport
        {
            get { return _speedReport; }
            set
            {
                _speedReport = value;
                if (SpeedReportLoaded != null)
                    SpeedReportLoaded(this, new EventArgs());
            }
        }

        private ObservableCollection<WorkTimeReportModel> _worktimeReport = new ObservableCollection<WorkTimeReportModel>();
        public ObservableCollection<WorkTimeReportModel> WorktimeReport
        {
            get { return _worktimeReport; }
            set { _worktimeReport = value; }
        }

        private SpeedOveralReportModel _speedOveral = new SpeedOveralReportModel();
        public SpeedOveralReportModel SpeedOveral
        {
            get { return _speedOveral; }
            set
            {
                _speedOveral = value;
                if (SpeedOveralLoaded != null)
                    SpeedOveralLoaded(this, new EventArgs());
            }
        }

        private ReportFuelModel _fuel;
        public ReportFuelModel Fuel
        {
            get { return _fuel; }
        }



        public void Split(char fx, string row)
        {
            switch (fx)
            {
                case 'a':
                case 'A':
                    FillDistanceReport(row);
                    break;
                case 'b':
                case 'B':
                    FillSpeedReport(row);
                    break;
                case 'c':
                case 'C':
                    FillWorkTimeReport(row);
                    break;
                case 'd':
                case 'D':
                    FillSpeedOveral(row);
                    break;
                case 'e':
                case 'E':
                    FillFuelReport(row);
                    break;
                case 'f':
                case 'F':
                    FillCompilateReport(row);
                    break;
            }
        }

        private void FillDistanceReport(string row)
        {
            try
            {
                MialageReportModel model = JsonConvert.DeserializeObject<MialageReportModel>(row);
                if (Application.Current != null)
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            MialageReport = model;
                        }));
            }
            catch { }
        }

        private void FillSpeedReport(string row)
        {
            try
            {
                SpeedReportModel model = JsonConvert.DeserializeObject<SpeedReportModel>(row);
                if (Application.Current != null)
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            SpeedReport = model;
                        }));
            }
            catch { }
        }

        private void FillWorkTimeReport(string row)
        {
            try
            {
                List<WorkTimeReportModel> model = JsonConvert.DeserializeObject<List<WorkTimeReportModel>>(row);
                if (Application.Current != null)
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            WorktimeReport.Clear();
                            foreach (var item in model)
                            {
                                item.DistUnworkTime = item.DistUnworkTime / 1000;
                                item.DistWorkTime = item.DistWorkTime / 1000;
                                WorktimeReport.Add(item);
                            }
                        }));
            }
            catch { }
        }

        private void FillSpeedOveral(string row)
        {
            try
            {
                SpeedOveralReportModel model = JsonConvert.DeserializeObject<SpeedOveralReportModel>(row);
                if (Application.Current != null)
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            if (SpeedOveralDataPreLoaded != null)
                                SpeedOveralDataPreLoaded(this, new EventArgs());
                        }));
                foreach (var item in model.Data)
                {
                    item.Points.ForEach(o =>
                    {
                        o.Speed /= 10;
                        o.StartAdress = "загрузка...";
                    });
                }
                if (Application.Current != null)
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        SpeedOveral = model;
                    }));
            }
            catch { }
        }

        private void FillFuelReport(string row)
        {
            try
            {
                ReportFuelModel model = JsonConvert.DeserializeObject<ReportFuelModel>(row);
                if (Application.Current != null)
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        _fuel = model;
                    }));
            }
            catch { }
            if (Application.Current != null && FuelReportLoaded!=null)
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    FuelReportLoaded(this, new EventArgs());
                }));
        }

        private void FillCompilateReport(string row)
        {

        }








        public void GetDistanceReport(string devID, DateTime dtStart, DateTime dtStop)
        {
            GetReportBase('A', devID, dtStart, dtStop);
        }

        public void GetSpeedReport(string devID, DateTime dtStart, DateTime dtStop)
        {
            GetReportBase('B', devID, dtStart, dtStop);
        }

        public void GetWorkTimeReport(string devID, DateTime dtStart, DateTime dtStop, TimeSpan tsStart, TimeSpan tsStop)
        {
            try
            {
                ReportsRequestWorkTime model = new ReportsRequestWorkTime();
                model.DID = devID;
                model.Start = new Models.Date.DateDataModel
                {
                    D = dtStart.Day,
                    M = dtStart.Month,
                    Y = dtStart.Year
                };
                model.Stop = new Models.Date.DateDataModel
                {
                    D = dtStop.Day,
                    M = dtStop.Month,
                    Y = dtStop.Year
                };
                model.StartWork = new Models.Date.TimeModel
                {
                    H = tsStart.Hours,
                    M = tsStart.Minutes,
                    S = tsStart.Seconds
                };
                model.StopWork = new Models.Date.TimeModel
                {
                    H = tsStop.Hours,
                    M = tsStop.Minutes,
                    S = tsStop.Seconds
                };
                string request = JsonConvert.SerializeObject(model);
                TCPConnection.Instance.SendData("EC" + request);
            }
            catch { }
        }

        public void GetSpeedOveralReport(string devID, DateTime dtStart, DateTime dtStop, int maxSpeed)
        {
            try
            {
                ReportRequestSpeedOveral model = new ReportRequestSpeedOveral();
                model.DID = devID;
                model.Start = new Models.Date.DateDataModel
                {
                    D = dtStart.Day,
                    M = dtStart.Month,
                    Y = dtStart.Year
                };
                model.Stop = new Models.Date.DateDataModel
                {
                    D = dtStop.Day,
                    M = dtStop.Month,
                    Y = dtStop.Year
                };
                model.SpeedMax = maxSpeed;
                string request = JsonConvert.SerializeObject(model);
                TCPConnection.Instance.SendData("ED" + request);
            }
            catch { }
        }

        /// <summary>
        /// Start calling fuel report. Async operation
        /// </summary>
        /// <param name="devID"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtStop"></param>
        public void GetFuelReport(string devID, DateTime dtStart, DateTime dtStop)
        {
            GetReportBase('E', devID, dtStart, dtStop);
        }

        public void GetCompilateReport(string devID, DateTime dtStart, DateTime dtStop)
        {
            GetReportBase('F', devID, dtStart, dtStop);
        }


        /// <summary>
        /// Base method to create reauest with ReportRequestBase model
        /// </summary>
        /// <param name="fx"></param>
        /// <param name="devID"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtStop"></param>
        private void GetReportBase(char fx, string devID, DateTime dtStart, DateTime dtStop)
        {
            try
            {
                ReportRequestBase model = new ReportRequestBase();
                model.DID = devID;
                model.Start = new Models.Date.DateDataModel
                {
                    D = dtStart.Day,
                    M = dtStart.Month,
                    Y = dtStart.Year
                };
                model.Stop = new Models.Date.DateDataModel
                {
                    D = dtStop.Day,
                    M = dtStop.Month,
                    Y = dtStop.Year
                };
                string request = JsonConvert.SerializeObject(model);
                TCPConnection.Instance.SendData("E" + fx.ToString() + request);
            }
            catch { }
        }
    }
}
