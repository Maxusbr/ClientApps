﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using DTCDev.Client.Cars.Engine.DisplayModels;
using DTCDev.Models.CarsSending;
using DTCDev.Models.CarsSending.Car;
using DTCDev.Models.DeviceSender;
using Newtonsoft.Json;
using DTCDev.Models.CarBase.CarStatData;

namespace DTCDev.Client.Cars.Engine.Handlers.Cars
{
    public class CarsHandler
    {
        private static CarsHandler _instance;

        public static CarsHandler Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new CarsHandler();
                return _instance;
            }
        }

        public CarsHandler()
        {
            _instance = this;
        }

        public void Init()
        {
            //TCPConnection.MessageReceived += Instance_MessageReceived;
        }


        private int _interval = 5000;
        private bool _threadAskerIsWorked = false;



        public void Update()
        {
            try
            {
                TCPConnection.Instance.SendData("BA");
            }
            catch (Exception ex)
            {
                string txt = ex.Message;
                Debug.WriteLine(txt);
            }
        }

        public delegate void CarsRefreshedBase(IEnumerable<DISP_Car> data);

        public event CarsRefreshedBase ChangeCarsStatus;
        protected virtual void OnChangeCarsStatus(IEnumerable<DISP_Car> data)
        {
            if (ChangeCarsStatus != null) ChangeCarsStatus(data);
        }

        public event CarsRefreshedBase CarsRefreshed;
        protected virtual void OnCarsRefreshed(IEnumerable<DISP_Car> data)
        {
            if (CarsRefreshed != null) CarsRefreshed(data);
        }

        public event EventHandler StartLoadCarData;
        public event EventHandler SettingsLoaded;

        readonly List<DISP_Car> _cars = new List<DISP_Car>();
        public List<DISP_Car> Cars
        {
            get { return _cars; }
        }

        private ObservableCollection<CarSettings> _settings = new ObservableCollection<CarSettings>();
        public ObservableCollection<CarSettings> Settings
        {
            get { return _settings; }
        }



        public void SetInterval(double interval)
        {
            _interval = (int)interval;
        }



        #region NETWORK

        private void ThreadDataAsker()
        {
            _threadAskerIsWorked = true;

            while (true)
            {
                Thread.Sleep(_interval);
                try
                {
                    TCPConnection.Instance.SendData("BB");
                    if (StartLoadCarData != null)
                        //if (Application.Current != null)
                        //    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        //    {
                                if (StartLoadCarData != null)
                                    StartLoadCarData(this, new EventArgs());
                            //}));
                }
                catch { }
            }

            _threadAskerIsWorked = false;
        }


        /// <summary>
        /// Split data from TCP network
        /// </summary>
        /// <param name="fx"></param>
        /// <param name="row"></param>
        public void Split(char fx, string row)
        {
            switch (fx)
            {
                case 'a':
                case 'A':
                    SplitCarData(row);
                    break;
                case 'b':
                case 'B':
                    FillCarsStatus(row);
                    break;
                case 'c':
                case 'C':
                    FillDeviceData(row);
                    break;
                case 'f':
                case 'F':
                case 'g':
                case 'G':
                    FillCarSettings(row);
                    break;
            }
        }

        private void SplitCarData(string row)
        {
            try
            {
                List<SCarModel> temp = JsonConvert.DeserializeObject<List<SCarModel>>(row);
                if (temp != null)
                {
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                               {
                                   FillCars(temp);
                               }));
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void FillCars(List<SCarModel> temp)
        {
            foreach (var item in temp)
            {
                if (item == null) continue;
                var c = _cars.FirstOrDefault(p => p.Car != null && p.Car.Id == item.Id);
                if (c == null)
                {
                    c = new DISP_Car
                    {
                        Car = item,
                        FuelData = new FuelDataModel
                        {
                            FuelDataPosition = item.FuelPosition,
                            StartFuelValue = item.StartValue,
                            StepPerLiter = ((decimal)item.StepsPerLiter) / 100.0m
                        },
                        VIN = item.VIN
                    };
                    _cars.Add(c);
                }
                else
                {
                    c.Car = item;
                }
            }
            OnCarsRefreshed(_cars);
            if (_threadAskerIsWorked == false)
            {
                Thread tr = new Thread(ThreadDataAsker);
                tr.Start();
            }
            //ask device data
            try
            {
                TCPConnection.Instance.SendData("BC");
            }
            catch (Exception ex)
            {
                string txt = ex.Message;
                Debug.WriteLine(txt);
            }
        }

        private void FillCarsStatus(string row)
        {
            try
            {
                var temp = JsonConvert.DeserializeObject<List<CarStateFullModel>>(row);
                if (temp != null)
                {
                    //Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    //           {
                    //var ids = new List<string>();
                    //ids = temp.Select(p => p.State.ID).Distinct().ToList();
                    var cars = new List<DISP_Car>();
                    foreach (var item in temp)
                    {
                        var excar = _cars.FirstOrDefault(p => p.Car.Id == item.State.ID);
                        if (excar == null) continue;
                        var car = new DISP_Car
                        {
                            Data = item.State,
                            VIN = item.VIN, Car = excar.Car, FuelData = excar.FuelData
                        };
                        //car.Errors.Clear();
                        foreach (var er in item.Errors)
                        {
                            car.Errors.Add(new DISP_Car.EOBDData
                            {
                                Key = er.Key,
                                Value = er.Value
                            });
                        }
                        //car.OBD.Clear();
                        foreach (var obd in item.OBD)
                        {
                            car.OBD.Add(new DISP_Car.EOBDData
                            {
                                Key = obd.Key,
                                Value = obd.Value
                            });
                        }

                        foreach (var sens in car.Device.Sensors)
                        {
                            int pos = sens.Model.Port - 1;
                            int c = item.State.Sensors.Count() - 1;
                            if (c < pos) continue;
                            //if (sens.State == null)
                            //    sens.State = new SensorState();
                            sens.State = new SensorState { Vol = item.State.Sensors[pos] };
                        }
                        cars.Add(car);
                    }
                    OnChangeCarsStatus(cars);
                    //_cars.ForEach(o => o.IsChanged = false);
                    //}));
                }
            }
            catch { }
        }

        private void FillDeviceData(string row)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<List<DeviceDataModel>>(row);
                //int l = row.Length;
                if (data != null)
                {
                    //Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    //              {
                                      foreach (var item in data)
                                      {
                                          var car = _cars.FirstOrDefault(p => p.Car.Id == item.DID);
                                          if (car == null) continue;
                                          if (car.Device == null)
                                          {
                                              car.Device = new Models.DeviceSender.DISP.DevicePresenter
                                              {
                                                  CT = item.CN,
                                                  DID = item.DID,
                                                  id = item.id,
                                                  IsGuard = false,
                                                  IsOnline = false,
                                                  LastUpdate = DateTime.Now,
                                                  Name = item.Name
                                              };
                                          }

                                          var sensors = new List<Models.DeviceSender.DISP.DevicePresenter.Sensor>();
                                          foreach (var s in item.Sensors)
                                          {
                                              sensors.Add(new Models.DeviceSender.DISP.DevicePresenter.Sensor
                                              {
                                                  Model = s
                                              });
                                          }
                                          car.Device.Sensors = sensors;
                                      }
                                  //}));
                }
            }
            catch (Exception ex)
            {
                string txt = ex.Message;
                Debug.WriteLine(txt);
            }
        }

        private void FillCarSettings(string row)
        {
            try
            {
                var settings = JsonConvert.DeserializeObject<List<CarSettings>>(row);
                Settings.Clear();
                foreach (var item in settings)
                {
                    Settings.Add(item);
                    var car = _cars.FirstOrDefault(p => p.ID == item.DID);
                    if (car == null) continue;
                    car.Name = item.CarName;
                    car.VIN = item.VIN;
                }
                if (SettingsLoaded != null)
                    SettingsLoaded(this, new EventArgs());
            }
            catch { }
        }

        #endregion NETWORK



        public void UpdateDrivers()
        {
            foreach (var item in DriverHandler.Instance.ListDriver)
            {
                List<DISP_Car> tempCars = _cars.Where(p => p.Car.driverID == item.Id).ToList();
                foreach (var c in tempCars)
                {
                    c.Driver = item;
                }
            }
        }

        public void SetDriverToCar(string carID, int driverID)
        {
            var driver = DriverHandler.Instance.ListDriver.FirstOrDefault(p => p.Id == driverID);
            if (driver == null)
                return;
            else
            {
                var car = _cars.FirstOrDefault(p => p.Car.Id == carID);
                if (car == null)
                    return;
                else
                {
                    car.Driver = driver;
                    try
                    {
                        TCPConnection.Instance.SendData("BD" + carID + ";" + driverID.ToString());
                    }
                    catch { }
                }
            }
        }

        public void SetFuelInfo(string carID, int position, int startValue, decimal scale)
        {
            var model = new SendFuelSettingsDataModel
            {
                CarID = carID,
                Position = position,
                Scale = (int)(scale * 100.0m),
                StartValue = startValue
            };
            try
            {
                var req = "BE" + JsonConvert.SerializeObject(model);
                TCPConnection.Instance.SendData(req);
            }
            catch { }
        }

        public void SetFuelSettings(string carID, int startValue, decimal scale)
        {
        }

        public void GetCarSettings()
        {
            try
            {
                TCPConnection.Instance.SendData("BF");
            }
            catch { }
        }

        public void SaveCarSettings(CarSettings model)
        {
            try
            {
                TCPConnection.Instance.SendData("BG" + JsonConvert.SerializeObject(model));
            }
            catch { }
        }

        public void SetCarMarkModel(string carNumber, string mark, string model)
        {
            var car = _cars.FirstOrDefault(p => p.Car.CarNumber == carNumber);
            if (car == null) return;
            car.Mark = mark;
            car.Model = model;
        }


    }
}
