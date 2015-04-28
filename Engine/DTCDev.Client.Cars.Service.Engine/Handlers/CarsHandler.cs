﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using DTCDev.Client.Cars.Service.Engine.Controls.ViewModels;
using DTCDev.Client.Cars.Service.Engine.Network;
using DTCDev.Models.CarBase.CarList;
using DTCDev.Models.CarBase.CarStatData;
using Newtonsoft.Json;
using DTCDev.Models.CarsSending.Car;
using DTCDev.Client.Cars.Service.Engine.Storage;
using DTCDev.Models;
using DTCDev.Models.CarsSending.Order;

namespace DTCDev.Client.Cars.Service.Engine.Handlers
{
    public class CarsHandler
    {
        private static CarsHandler _instance;
        public static CarsHandler Instance
        {
            get { return _instance ?? (_instance = new CarsHandler()); }
        }

        public CarsHandler()
        {
            _instance = this;
        }

        public delegate void GetCarDetailHandler(CarListDetailsDataModel carDetail);
        public event GetCarDetailHandler OnGetCarDetailComplete;
        
        public delegate void HistoryWorksHandler(List<CarHistoryWorkReport> works);
        public event HistoryWorksHandler HistoryWorksLoaded;
        public delegate void CarDTCMonthHandler(List<CarDTCHistoryModel> data);
        public event CarDTCMonthHandler CarDTCMonthLoaded;
        public delegate void PIDSLoadedHandler(List<CarEnabledPIDs> pids);
        public event PIDSLoadedHandler PIDSLoaded;
        public delegate void OnLineUpdatedHandler(List<DicDataModel> data);
        public event OnLineUpdatedHandler OnLineUpdated;
        public event EventHandler OrderDataLoaded;
        public delegate void ServiceControllersRefreshedHandler(List<CarWithSettingDevice> data);
        public event ServiceControllersRefreshedHandler ServiceControllerRefreshed;
        public delegate void GetCarRecomendationsCompleteHandler(List<KVPBase> data);
        public event GetCarRecomendationsCompleteHandler GetCarRecomendationsComplete;
        public delegate void OrderRecomendationsLoadHandler(string text);
        public event OrderRecomendationsLoadHandler OrderRecomendationsLoad;


        private void GetCarDetailComplete(CarListDetailsDataModel cardetail)
        {
            if (OnGetCarDetailComplete != null) OnGetCarDetailComplete(cardetail);
        }

        /// <summary>
        /// Запрос списка автомобилей
        /// </summary>
        public void GetCars()
        {
            try
            {
                TCPConnection.Instance.SendData("BA");
            }
            catch { }
        }

        /// <summary>
        /// Запрос детальной информации по одному автомобилю
        /// </summary>
        /// <param name="carNumber"></param>
        public void GetCarDetails(string carNumber)
        {
            try
            {
                TCPConnection.Instance.SendData("BB" + carNumber);
            }
            catch { }
        }

        /// <summary>
        /// Добавить новый перечень выполненных работ
        /// </summary>
        /// <param name="service"></param>
        /// <param name="CarNumber"></param>
        public void AddWorks(ServiceViewModel service, string CarNumber, int orderN, int distance, DateTime date)
        {
            if (service == null)
                return;
            try
            {
                CarWorksCompleteModel model = new CarWorksCompleteModel();
                model.CarNumber = CarNumber;
                model.Comment = service.Comment;
                model.DistanceMake = distance;
                model.OrderNo = orderN;
                model.Date = new Models.Date.DateDataModel
                {
                    D = date.Day,
                    M = date.Month,
                    Y = date.Year
                };
                foreach (var item in service.Works)
                {
                    model.WorkIds.Add(new CarWorksCompleteModel.WorkItemModel
                    {
                        Comment = item.Comment,
                        Cost = (int)item.Price,
                        //id = item.ID
                    });
                }
                TCPConnection.Instance.SendData("BD" + JsonConvert.SerializeObject(model));
            }
            catch { }
        }

        public void AddWorks(CarWorksCompleteModel model)
        {
            try
            {
                TCPConnection.Instance.SendData("BD" + JsonConvert.SerializeObject(model));
            }
            catch { }
        }

        public void AddWorksAndCloseOrder(CarWorksCompleteModel model)
        {
            try
            {

            }
            catch { }
        }

        /// <summary>
        /// добавить новый автомобиль
        /// </summary>
        /// <param name="model"></param>
        public void AddCar(CarServiceDataModel model)
        {
            try
            {
                string req = JsonConvert.SerializeObject(model);
                TCPConnection.Instance.SendData("BC" + req);
            }
            catch
            {

            }
        }

        /// <summary>
        /// Запрос данных о истории обслуживания автомобиля
        /// </summary>
        /// <param name="carNumber">номер автомобиля, для которого выполняется запрос</param>
        public void GetCarHistory(string carNumber)
        {
            try
            {
                TCPConnection.Instance.SendData("BE" + carNumber);
            }
            catch { }
        }

        public void GetDTCErrors(string carNumber, int year, int month)
        {
            try
            {
                TCPConnection.Instance.SendData("BG" + carNumber + ";" + year.ToString() + ";" + month.ToString());
            }
            catch { }
        }

        public void GetOBDHistory(string did, int year, int month, int day)
        {
            try
            {
                TimeSpan ts = DateTime.Now - DateTime.UtcNow;
                int h = (int)ts.TotalHours;
                TCPConnection.Instance.SendData("BH" + did + ";" + year.ToString() + ";" + month.ToString()+";"+day.ToString()+";"+h.ToString());
            }
            catch { }
        }

        public void GetCarPidList(string carNumber)
        {
            try
            {
                TCPConnection.Instance.SendData("BI" + carNumber);
            }
            catch { }
        }

        public void GetLastUpdatedTime()
        {
            try
            {
                TCPConnection.Instance.SendData("BJ");
            }
            catch { }

        }

        private void GetCarsErrors()
        {
            try
            {
                TCPConnection.Instance.SendData("BF");
            }
            catch { }
        }

        public void SendCarNewPids(SetCarPidsModel model)
        {
            try
            {
                TCPConnection.Instance.SendData("BK" + JsonConvert.SerializeObject(model));
            }
            catch { }
        }

        public void UpdateCarWorks(string carNumber)
        {
            try
            {
                TCPConnection.Instance.SendData("BL" + carNumber);
            }
            catch { }
        }

        public void GetCarOrder(string carNumber)
        {
            try
            {
                TCPConnection.Instance.SendData("BM" + carNumber);
            }
            catch { }
        }

        public void UpdateServiceControllers()
        {
            try
            {
                TCPConnection.Instance.SendData("BN");
            }
            catch { }
        }

        public void GetCarRecomendations(int carID)
        {
            try
            {
                TCPConnection.Instance.SendData("BO" + carID.ToString());
            }
            catch { }
        }

        public void UpdateRecomendations(int orderID, string text)
        {
            try
            {
                TCPConnection.Instance.SendData("BP" + orderID + ";" + text);
            }
            catch { }
        }

        public void GetOrderRecomendation(int orderID)
        {
            try
            {
                TCPConnection.Instance.SendData("BQ" + orderID.ToString());
            }
            catch { }
        }

        public void GetCurrentOBD()
        {
            try
            {
                TCPConnection.Instance.SendData("BS");
            }
            catch { }
        }





        public void Split(char fx, string row)
        {
            switch (fx)
            {
                case 'a':
                case 'A':
                    FillCarData(row);
                    break;
                case 'b':
                case 'B':
                    FillDetailData(row);
                    break;
                case 'c':
                case 'C':
                    GetCars();
                    break;
                case 'd':
                case 'D':
                    AddWorksComplete(row);
                    break;
                case 'e':
                case 'E':
                    LoadHistoryWorksComplete(row);
                    break;
                case 'f':
                case 'F':
                    LoadCarsErrorsComplete(row);
                    break;
                case 'g':
                case 'G':
                    LoadCarMonthDTCHistoryComplete(row);
                    break;
                case 'h':
                case 'H':
                    LoadOBD(row);
                    break;
                case 'i':
                case 'I':
                    LoadPidList(row);
                    break;
                case 'j':
                case 'J':
                    LoadTimeRefreshed(row);
                    break;
                case 'l':
                case 'L':
                    FillOneCarWorks(row);
                    break;
                case 'm':
                case 'M':
                    FillCarOrder(row);
                    break;
                case 'n':
                case 'N':
                    FillServiceControllers(row);
                    break;
                case 'o':
                case 'O':
                    FillRecomendations(row);
                    break;
                case 'q':
                case 'Q':
                    FillOrderRecomendation(row);
                    break;
                case 's':
                case 'S':
                    FillCurrentOBD(row);
                    break;

            }

        }

        /// <summary>
        /// Обработка данных о списке авто
        /// </summary>
        /// <param name="row"></param>
        private void FillCarData(string row)
        {
            try
            {
                CarStorage.Instance.Cars.Clear();
                var temp = JsonConvert.DeserializeObject<List<CarListBaseDataModel>>(row);
                if (Application.Current != null)
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            if (temp != null)
                                CarStorage.Instance.SetCarData(temp);
                            else
                                CarStorage.Instance.SetLoadError();
                        }));
                GetCarsErrors();
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Обработка получения подробных данных об конкретном автомобиле
        /// </summary>
        /// <param name="row"></param>
        private void FillDetailData(string row)
        {
            try
            {
                var temp = JsonConvert.DeserializeObject<CarListDetailsDataModel>(row);
                if (Application.Current != null)
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            if (temp != null)
                                GetCarDetailComplete(temp);
                        }));
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Обработка данных завершения добавления работ
        /// </summary>
        /// <param name="row"></param>
        private void AddWorksComplete(string row)
        {
            if (row == "OK")
                GetCars();
        }

        /// <summary>
        /// обрабортка полученных данных о истории автомобиля
        /// </summary>
        /// <param name="row"></param>
        private void LoadHistoryWorksComplete(string row)
        {
            try
            {
                List<CarHistoryWorkReport> temp = JsonConvert.DeserializeObject<List<CarHistoryWorkReport>>(row);
                if (Application.Current != null)
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            if (HistoryWorksLoaded != null)
                                HistoryWorksLoaded(temp);
                        }));
            }
            catch { }
        }

        private void LoadCarsErrorsComplete(string row)
        {
            try
            {
                List<CarDTCHistoryModel> temp = JsonConvert.DeserializeObject<List<CarDTCHistoryModel>>(row);
                if (Application.Current != null)
                {
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            if (temp != null)
                            {

                                for (int i = 0; i < CarStorage.Instance.Cars.Count(); i++)
                                {
                                    CarStorage.Instance.Cars[i].Errors.Clear();
                                    CarStorage.Instance.Cars[i].UpdateErrors();
                                }
                                foreach (var item in temp)
                                {
                                    DISP_Car model = CarStorage.Instance.Cars.Where(p => p.CarModel.DID == item.DID).FirstOrDefault();
                                    if (model != null)
                                    {
                                        model.Errors.Add(item);
                                        model.UpdateErrors();
                                    }
                                }
                            }
                            CarStorage.Instance.SetErrorsLoaded();
                        }));
                }
            }
            catch { }
        }

        private void LoadCarMonthDTCHistoryComplete(string row)
        {
            try
            {
                List<CarDTCHistoryModel> temp = JsonConvert.DeserializeObject<List<CarDTCHistoryModel>>(row);
                if (temp != null)
                {
                    if (Application.Current != null)
                        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                if (CarDTCMonthLoaded != null)
                                    CarDTCMonthLoaded(temp);
                            }));
                }
            }
            catch { }
        }

        private void LoadOBD(string row)
        {
            try
            {
                OBDHistoryDataModel model = JsonConvert.DeserializeObject<OBDHistoryDataModel>(row);
                if (model != null)
                    if (Application.Current != null)
                        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                CarStorage.Instance.SetOBDHistoryToCar(model);
                            }));
            }
            catch { }
        }

        private void LoadPidList(string row)
        {
            try
            {
                List<CarEnabledPIDs> data = JsonConvert.DeserializeObject<List<CarEnabledPIDs>>(row);
                if (data != null)
                    if (Application.Current != null)
                        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                if (PIDSLoaded != null)
                                    PIDSLoaded(data);
                            }));
            }
            catch { }
        }

        private void LoadTimeRefreshed(string row)
        {
            try
            {
                List<DicDataModel> data = JsonConvert.DeserializeObject<List<DicDataModel>>(row);
                if (data != null)
                    if (Application.Current != null)
                        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            if (OnLineUpdated != null)
                                OnLineUpdated(data);
                        }));
            }
            catch { }
        }

        private void FillOneCarWorks(string row)
        {
            try
            {
                List<WorksWithFlagDataModel> temp = JsonConvert.DeserializeObject<List<WorksWithFlagDataModel>>(row);
                if (temp != null) if (Application.Current != null)
                        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            CarStorage.Instance.SetCarTotalWorks(temp);
                        }));
            }
            catch { }
        }

        private void FillCarOrder(string row)
        {
            try
            {
                CarOrderModel model = JsonConvert.DeserializeObject<CarOrderModel>(row);
                if (model != null)
                    if (Application.Current != null)
                        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                CarStorage.Instance.SetOrderData(model);
                                if (OrderDataLoaded != null)
                                    OrderDataLoaded(this, new EventArgs());
                            }));
            }
            catch { }
        }

        private void FillServiceControllers(string row)
        {
            try
            {
                List<CarWithSettingDevice> data = JsonConvert.DeserializeObject<List<CarWithSettingDevice>>(row);

                if (data != null)
                {
                    data = data.OrderBy(p => p.DID).ToList();
                    if (Application.Current != null)
                        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                if (ServiceControllerRefreshed != null)
                                    ServiceControllerRefreshed(data);
                            }));
                }
            }
            catch { }
        }

        private void FillRecomendations(string row)
        {
            try
            {
                List<KVPBase> data = JsonConvert.DeserializeObject<List<KVPBase>>(row);
                if (data != null)
                    if (GetCarRecomendationsComplete != null)
                        if (Application.Current != null)
                            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                {
                                    GetCarRecomendationsComplete(data);
                                }));
            }
            catch { }
        }

        private void FillOrderRecomendation(string row)
        {
            if (Application.Current != null)
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        if (OrderRecomendationsLoad != null)
                            OrderRecomendationsLoad(row);
                    }));
        }

        private void FillCurrentOBD(string row)
        {
            try
            {
                List<CarStateOBDModel> temp = JsonConvert.DeserializeObject<List<CarStateOBDModel>>(row);
                if (Application.Current != null)
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            CarStorage.Instance.SetCurrentOBD(temp);
                        }));
            }
            catch { }
        }
    }
}