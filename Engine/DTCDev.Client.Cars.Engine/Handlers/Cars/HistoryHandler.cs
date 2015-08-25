using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using DTCDev.Convertors;
using DTCDev.Models.CarsSending.Car;
using Newtonsoft.Json;
using DTCDev.Models;

namespace DTCDev.Client.Cars.Engine.Handlers.Cars
{
    public class HistoryHandler
    {
        private static HistoryHandler _instance;

        public static HistoryHandler Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new HistoryHandler();
                return _instance;
            }
        }

        public HistoryHandler()
        {
            _instance = this;
        }

        private string currentDeviceID;
        DateTime startDate;
        DateTime stopDate;
        DateTime currentAsked;
        StringToDateTime convertor = new StringToDateTime();
        private List<CarStateModel> historyMessages = new List<CarStateModel>();


        public List<CarStateModel> HistoryMessages
        {
            get { return historyMessages; }
        }
        public delegate void DayChanged(DateTime date);
        public event DayChanged DayChange;

        public delegate void DayStateChanged(List<CarStateModel> list);
        public event DayStateChanged DayStateChange;

        public delegate void dayRefreshed(DateTime day, List<CarStateModel> data);
        public event dayRefreshed DayRefreshed;

        public delegate void LinesLoadedHandler(LinesDataModel model);
        public event LinesLoadedHandler LinesLoaded;

        public delegate void OBDLoadedHandler(OBDHistoryDataModel model);
        public event OBDLoadedHandler OBDLoaded;

        public delegate void AccLoadedHandler(CarAccHistoryModel model);
        public event AccLoadedHandler AccLoaded;

        public event EventHandler LoadCompleted;

        public delegate void DateTimePositionHandler(DateTime position);
        public event DateTimePositionHandler SetDateTimePosition;
        public virtual void OnSetDateTimePosition(DateTime position)
        {
            if (SetDateTimePosition != null) SetDateTimePosition(position);
        }

        public void Split(char fx, string row)
        {
            switch (fx)
            {
                case 'a':
                case 'A':
                    FillHistoryDay(row);
                    break;
                case 'b':
                case 'B':
                    FillLinesData(row);
                    break;
                case 'c':
                case 'C':
                    FillOBDData(row);
                    break;
                case 'd':
                case 'D':
                    FillAccData(row);
                    break;
            }
        }


        private void FillHistoryDay(string row)
        {
            try
            {
                List<CarStateModel> data = JsonConvert.DeserializeObject<List<CarStateModel>>(row);
                if (data == null)
                    return;

                //filter emty points
                int i = 0;
                int count = data.Count();
                while(i<count-2)
                {
                    if (data[i].Lt == data[i + 1].Lt && data[i].Ln == data[i + 1].Ln)
                    {
                        data.RemoveAt(i + 1);
                        count--;
                    }
                    else
                        i++;
                }

                //filter droped points
                i = 0;
                count = data.Count();
                while (i < count - 2)
                {
                    if (data[i].Lt > 1800000 || data[i].Lt < -1800000 || data[i].Ln > 1800000 || data[i].Ln < -1800000)
                    {
                        data.RemoveAt(i);
                        count--;
                    }
                    else
                        i++;
                }

                //filter bad data
                i = 0;
                count = data.Count();
                while(i<count-2)
                {
                    try
                    {
                        double deltaLatitude = data[i].Lt / 10000.0 - data[i + 1].Lt / 10000.0;
                        double deltaLongitude = data[i].Ln / 10000.0 - data[i + 1].Ln / 10000.0;
                        double kmLat = Math.Round(deltaLatitude * 111.1111, 1);
                        double kmLon = Math.Round(deltaLongitude * 111.1111 * GetCos((int)data[i].Ln), 1);
                        //km x and lm y

                        //Find distance:
                        double dist = Math.Sqrt(kmLat * kmLat + kmLon * kmLon);
                        if (dist > 0)
                        {
                            TimeSpan ts = new DateTime(data[i+1].yy, data[i+1].Mnth, data[i+1].dd, data[i+1].hh, data[i+1].mm, data[i+1].ss) - new DateTime(data[i].yy, data[i].Mnth, data[i].dd, data[i].hh, data[i].mm, data[i].ss);
                            double speed = dist / ts.TotalHours;
                            if (speed > 220 || speed < -20)
                            {
                                data.RemoveAt(i + 1);
                                count--;
                            }
                            else
                                i++;
                        }
                        else
                            i++;
                    }
                    catch
                    {
                        data.RemoveAt(i);
                        count--;
                    }
                }
                historyMessages.AddRange(data);
                DateTime dateDisplayed = currentAsked + TimeSpan.FromDays(1);
                //if (Application.Current != null)
                //    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                //        {
                            
                            if (DayRefreshed != null)
                                //добавляем сюда один день, так как прошло вычитание на этапе завершения запроса
                                //к серверу
                                //TODO: Костыль, подумать как лучше
                                DayRefreshed(dateDisplayed, data);
                        //}));

                ThreadGetter();
                //tr.Start();
            }
            catch { }
        }

        private double GetCos(int angle)
        {
            switch (angle)
            {
                case 1:
                    return 0.9998;
                case 2: return 0.9994;
                case 3: return 0.9986;
                case 4: return 0.9976;
                case 5: return 0.9962;
                case 6: return 0.9945;
                case 7: return 0.9925;
                case 8: return 0.9903;
                case 9: return 0.9877;
                case 10: return 0.9848;
                case 11: return 0.9816;
                case 12: return 0.9781;
                case 13: return 0.9744;
                case 14: return 0.9703;
                case 15: return 0.9659;
                case 16: return 0.9613;
                case 17: return 0.9563;
                case 18: return 0.9511;
                case 19: return 0.9455;
                case 20: return 0.9397;
                case 21: return 0.9336;
                case 22: return 0.9272;
                case 23: return 0.9205;
                case 24: return 0.9135;
                case 25: return 0.9063;
                case 26: return 0.8988;
                case 27: return 0.891;
                case 28: return 0.8829;
                case 29: return 0.8746;
                case 30: return 0.866;
                case 31: return 0.8572;
                case 32: return 0.848;
                case 33: return 0.8387;
                case 34: return 0.829;
                case 35: return 0.8192;
                case 36: return 0.809;
                case 37: return 0.7986;
                case 38: return 0.788;
                case 39: return 0.7771;
                case 40: return 0.766;
                case 41: return 0.7547;
                case 42: return 0.7431;
                case 43: return 0.7314;
                case 44: return 0.7193;
                case 45: return 0.7071;
                case 46: return 0.6947;
                case 47: return 0.682;
                case 48: return 0.6691;
                case 49: return 0.6561;
                case 50: return 0.6428;
                case 51: return 0.6293;
                case 52: return 0.6157;
                case 53: return 0.6018;
                case 54: return 0.5878;
                case 55: return 0.5736;
                case 56: return 0.5592;
                case 57: return 0.5446;
                case 58: return 0.5299;
                case 59: return 0.515;
                case 60: return 0.5;
                case 61: return 0.4848;
                case 62: return 0.4695;
                case 63: return 0.454;
                case 64: return 0.4384;
                case 65: return 0.4226;
                case 66: return 0.4067;
                case 67: return 0.3907;
                case 68: return 0.3746;
                case 69: return 0.3584;
                case 70: return 0.342;
                case 71: return 0.3256;
                case 72: return 0.309;
                case 73: return 0.2924;
                case 74: return 0.2756;
                case 75: return 0.2588;
                case 76: return 0.2419;
                case 77: return 0.225;
                case 78: return 0.2079;
                case 79: return 0.1908;
                case 80: return 0.1736;
                case 81: return 0.1564;
                case 82: return 0.1392;
                case 83: return 0.1219;
                case 84: return 0.1045;
                case 85: return 0.0872;
                case 86: return 0.0698;
                case 87: return 0.0523;
                case 88: return 0.0349;
                case 89: return 0.0175;
                case 90: return 0;
                default: return 1;
            }
        }

        private void FillLinesData(string row)
        {
            try
            {
                LinesDataModel model = JsonConvert.DeserializeObject<LinesDataModel>(row);
                if (model != null)
                    //if (Application.Current != null)
                    //    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    //        {
                                if (LinesLoaded != null)
                                    LinesLoaded(model);
                            //}));
            }
            catch { }
        }

        private void FillOBDData(string row)
        {
            try
            {
                OBDHistoryDataModel model = JsonConvert.DeserializeObject<OBDHistoryDataModel>(row);
                if (model != null)
                    //if (Application.Current != null)
                    //    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    //    {
                            if (OBDLoaded != null)
                                OBDLoaded(model);
                        //}));
            }
            catch 
            {
                if (OBDLoaded != null)
                    OBDLoaded(new OBDHistoryDataModel());
            }
        }

        private void FillAccData(string row)
        {
            try
            {
                CarAccHistoryModel accHist = JsonConvert.DeserializeObject<CarAccHistoryModel>(row);
                if(accHist!=null)
                {
                    if (AccLoaded != null)
                        //if (Application.Current != null)
                        //    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        //    {
                                AccLoaded(accHist);
                            //}));
                }
            }
            catch 
            {
            
            }
        }






        public void StartLoadHistory(string deviceID, DateTime start, DateTime stop, bool optim)
        {
            historyMessages.Clear();
            optimization = optim;
            if (start > stop)
                return;

            currentDeviceID = deviceID;
            currentAsked = stop;
            startDate = start;
            stopDate = stop;
            ThreadGetter();
        }

        bool optimization = false;

        private void ThreadGetter()
        {
            try
            {
                if (currentAsked < startDate)
                {
                    historyMessages.ForEach(o => o.DevID = currentDeviceID);
                    //if (Application.Current != null)
                    //    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    //    {
                            if (LoadCompleted != null)
                                LoadCompleted(this, new EventArgs());
                        //}));
                    return;
                }
                string request = currentDeviceID + ";" + currentAsked.Year.ToString() + ";" + currentAsked.Month.ToString() + ";" + currentAsked.Day.ToString() + ";";
                if (optimization)
                {
                    request += "1";
                    TCPConnection.Instance.SendData("CA" + request);
                }
                else
                {
                    request += "0";
                    TCPConnection.Instance.SendData("CA" + request);
                }
                currentAsked -= TimeSpan.FromDays(1);
            }
            catch { }
        }


        public void StartLoadDayLines(string deviceID, DateTime dtView)
        {
            string req="CB"+deviceID+";"+dtView.Year.ToString()+";"+dtView.Month.ToString()+";"+dtView.Day.ToString();
            try
            {
                TCPConnection.Instance.SendData(req);
            }
            catch { }
        }

        public void StartLoadOBD(string deviceID, DateTime dtView)
        {
            string req = "CC" + deviceID + ";" + dtView.Year.ToString() + ";" + dtView.Month.ToString() + ";" + dtView.Day.ToString();
            try
            {
                TCPConnection.Instance.SendData(req);
            }
            catch { }
        }

        public void StartLoadAcc(string deviceID, DateTime dtView)
        {
            string req = "CD" + deviceID + ";" + dtView.Year.ToString() + ";" + dtView.Month.ToString() + ";" + dtView.Day.ToString();
            try
            {
                TCPConnection.Instance.SendData(req);
            }
            catch { }
        }




        public int GetColRows()
        {
            return historyMessages.Count;
        }

        public virtual void OnDayStateChange(List<CarStateModel> list)
        {
            if (DayStateChange != null) DayStateChange(list);
        }

        public virtual void OnDayChange(DateTime date)
        {
            if (DayChange != null) DayChange.Invoke(date);
        }
    }
}
