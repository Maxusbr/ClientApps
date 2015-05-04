using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCDev.Client.Cars.Service.Engine.Network;
using DTCDev.Models.CarBase.CarStatData;
using Newtonsoft.Json;
using System.Windows;

namespace DTCDev.Client.Cars.Service.Engine.Handlers
{
    public class CarBaseHandler
    {
        private static CarBaseHandler _instance;
        public static CarBaseHandler Instance
        {
            get { return _instance ?? (_instance = new CarBaseHandler()); }
        }

        public CarBaseHandler()
        {
            _instance = this;
        }

        #region Request handlers


        public void GetMarks()
        {
            SendRequest("TA");
        }

        public void GetModels(int idPearent)
        {
            SendRequest("TB" + idPearent.ToString());
        }

        public void GetBodies(int idPearent)
        {
            SendRequest("TC" + idPearent.ToString());
        }

        public void GetEngineTypes(int idModel, int idPearent)
        {
            SendRequest("TD" + idModel.ToString() + ";" + idPearent.ToString());
        }

        public void GetEngines(int idModel, int idPearent)
        {
            SendRequest("TE" + idModel.ToString() + ";" + idPearent.ToString());
        }

        public void GetTransTypes(int idModel, int idPearent)
        {
            SendRequest("TF" + idModel.ToString() + ";" + idPearent.ToString());
        }

        private int _idModel;
        private int _idEngine;
        private int _idBody;
        private int _idEngineType;
        private int _idTransmission;

        public void GetCarWorksEvent(int idModel, int idEngine, int idBody, int idEngineType, int idTransmission)
        {
            _idModel = idModel;
            _idEngine = idEngine;
            _idBody = idBody;
            _idEngineType = idEngineType;
            _idTransmission = idTransmission;
            CallLoadCarWorks();
        }

        private void CallLoadCarWorks()
        {
                RequestWorksDataModel req = new RequestWorksDataModel
                {
                    idBody = _idBody,
                    idEngine = _idEngine,
                    idEngineType = _idEngineType,
                    idModel = _idModel,
                    idTransmission = _idTransmission
                };
                string row = JsonConvert.SerializeObject(req);
                SendRequest("TG" + row);
        }

        public void GetWorkTypes(object sender, EventArgs e)
        {
           SendRequest("TI");
        }

        public void GetWorksList(object sender, EventArgs e)
        {
            SendRequest("TH");
        }

        public void AddWorkNameEvent(int idType, string name)
        {
                AddingWorkNameModel model = new AddingWorkNameModel
                {
                    idType = idType,
                    Name = name
                };
                string req = JsonConvert.SerializeObject(model);
                SendRequest("TJ" + req);
        }

        public void GetCarStatInfo(int idModel, int idPearent)
        {
            SendRequest("TK" + idModel.ToString()+";"+idPearent.ToString());
        }


        public void GetEnginesByModel(int idModel, int idPearent)
        {
                SendRequest("TL"+idModel.ToString());
        }

        public void AddWorkEvent(AddWorkTocarModel model)
        {
                SendRequest("TM" + JsonConvert.SerializeObject(model));
        }

        public void UpdateOtherWorks()
        {
                SendRequest("TN");
        }

        public void AddOtherWorkName(string name, int idType)
        {
                AddingWorkNameModel model = new AddingWorkNameModel
                {
                    idType = idType,
                    Name = name
                };
                string req = JsonConvert.SerializeObject(model);
                SendRequest("TO" + req);
        }

        public void UpdatePartsWorks()
        {
                SendRequest("TP");
        }

        public void AddPartWorkName(string name, int idType)
        {
            AddingWorkNameModel model = new AddingWorkNameModel
            {
                idType = idType,
                Name = name
            };
            string req = JsonConvert.SerializeObject(model);
            SendRequest("TQ" + req);
        }

        public void AddPartsList(CarPartsWorkModel model)
        {
            string req = JsonConvert.SerializeObject(model);
            SendRequest("TS" + req);
        }

        /// <summary>
        /// Получить состав работ для автомобиля
        /// </summary>
        /// <param name="idWork">идетификатор работы</param>
        /// <param name="carNumber">номер автомобиля</param>
        /// <param name="periodic">является ли работа периодической</param>
        public void GetWorkParts(int idWork, string carNumber, bool periodic)
        {
            WorksWithFlagDataModel model = new WorksWithFlagDataModel
            {
                id = idWork,
                Name = carNumber
            };
            if (periodic)
                model.IsPeriodic = 1;
            else
                model.IsPeriodic = 0;
            SendRequest("TT" + JsonConvert.SerializeObject(model));
        }

        /// <summary>
        /// Получить состав работы для автомобиля
        /// </summary>
        /// <param name="idWork">идентификатор работы</param>
        /// <param name="carNumber">номер автомобиля</param>
        public void GetWorkParts(int idWork, string carNumber)
        {
            WorksWithFlagDataModel model = new WorksWithFlagDataModel
            {
                id = idWork,
                Name = carNumber,
                IsPeriodic = 1
            };
            SendRequest("TT" + JsonConvert.SerializeObject(model));
        }

        /// <summary>
        /// получить состав работы
        /// </summary>
        /// <param name="idWork">идентификатор работы</param>
        public void GetWorkParts(int idWork)
        {
            WorksWithFlagDataModel model = new WorksWithFlagDataModel
            {
                id = idWork
            };
            SendRequest("TT" + JsonConvert.SerializeObject(model));
        }

        /// <summary>
        /// Получить состав работы
        /// </summary>
        /// <param name="idWork">идентификатор работы</param>
        /// <param name="periodic">является работа периодичной или нет</param>
        public void GetWorkParts(int idWork, bool periodic)
        {
            WorksWithFlagDataModel model = new WorksWithFlagDataModel
            {
                id = idWork
            };
            SendRequest("TU" + JsonConvert.SerializeObject(model));
        }


        private void SendRequest(string req)
        {
            try
            {
                TCPConnection.Instance.SendData(req);
            }
            catch { }
        }

        #endregion Request handlers



        #region TCP

        public void Split(char fx, string row)
        {
            switch (fx)
            {
                case 'a':
                case 'A':
                    FillMarks(row);
                    break;
                case 'b':
                case 'B':
                    FillModels(row);
                    break;
                case 'c':
                case 'C':
                    FillBodyTypes(row);
                    break;
                case 'd':
                case 'D':
                    FillEngineTypes(row);
                    break;
                case 'e':
                case 'E':
                    FillEngineVolume(row);
                    break;
                case 'f':
                case 'F':
                    FillTransType(row);
                    break;
                case 'g':
                case 'G':
                    FillCarsWorks(row);
                    break;
                case 'h':
                case 'H':
                    FillWorksList(row);
                    break;
                case 'i':
                case 'I':
                    FillWorkTypes(row);
                    break;
                case 'j':
                case 'J':
                    FillWorksList(row);
                    break;
                case 'k':
                case'K':
                    FillCarStatInfo(row);
                    break;
                case 'l':
                case 'L':
                    FillEnginesByModel(row);
                    break;
                case 'm':
                case 'M':
                    CallLoadCarWorks();
                    break;
                case'n':
                case'N':
                    FillOtherWorksList(row);
                    break;
                case 'o':
                case 'O':
                    FillOtherWorksList(row);
                    break;
                case 'p':
                case'P':
                    FillWorkParts(row);
                    break;
                case 'q':
                case 'Q':
                    FillWorkParts(row);
                    break;
                case 's':
                case 'S':

                    break;
                case 't':
                case 'T':
                    FillWorkPartsList(row);
                    break;
            }
        }

        private void FillMarks(string row)
        {
            try
            {
                var temp = JsonConvert.DeserializeObject<List<KVPBase>>(row);
                if (temp == null) return;
                if (Application.Current != null)
                    Application.Current.Dispatcher.BeginInvoke(new Action(() => 
                        SpecificationDataStorage.Instance.SetMarks(temp)));
            }
            catch { }
        }

        private void FillModels(string row)
        {
            try
            {
                var temp = JsonConvert.DeserializeObject<List<KVPBase>>(row);
                if (temp == null) return;
                if (Application.Current != null)
                    Application.Current.Dispatcher.BeginInvoke(new Action(() => 
                        SpecificationDataStorage.Instance.SetModels(temp)));
            }
            catch { }
        }

        private void FillBodyTypes(string row)
        {
            try
            {
                var temp = JsonConvert.DeserializeObject<List<KVPBase>>(row);
                if (temp == null) return;
                if (Application.Current != null)
                    Application.Current.Dispatcher.BeginInvoke(new Action(() => 
                        SpecificationDataStorage.Instance.SetBodyTypes(temp)));
            }
            catch { }
        }

        private void FillEngineTypes(string row)
        {
            try
            {
                var temp = JsonConvert.DeserializeObject<List<KVPBase>>(row);
                if (temp == null) return;
                if (Application.Current != null)
                    Application.Current.Dispatcher.BeginInvoke(new Action(() => 
                        SpecificationDataStorage.Instance.SetEngineTypes(temp)));
            }
            catch { }
        }

        private void FillEngineVolume(string row)
        {
            try
            {
                var temp = JsonConvert.DeserializeObject<List<KVPBase>>(row);
                if (temp == null) return;
                if (Application.Current != null)
                    Application.Current.Dispatcher.BeginInvoke(new Action(() => 
                        SpecificationDataStorage.Instance.SetEngineVolumes(temp)));
            }
            catch { }
        }

        private void FillTransType(string row)
        {
            try
            {
                var temp = JsonConvert.DeserializeObject<List<KVPBase>>(row);
                if (temp == null) return;
                if (Application.Current != null)
                    Application.Current.Dispatcher.BeginInvoke(new Action(() => 
                        SpecificationDataStorage.Instance.SetTransTypes(temp)));
            }
            catch { }
        }

        private void FillCarsWorks(string row)
        {
            try
            {
                List<WorksInfoExemplarDataModel> data = JsonConvert.DeserializeObject<List<WorksInfoExemplarDataModel>>(row);
                if (data == null) return;
                if (Application.Current != null)
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        SpecificationDataStorage.Instance.SetCarWorks(data)));
            }
            catch { }
        }

        private void FillWorksList(string row)
        {
            try
            {
                UpdateOtherWorks();
                List<WorksInfoDataModel> data = JsonConvert.DeserializeObject<List<WorksInfoDataModel>>(row);
                if (data == null) return;
                if (Application.Current != null)
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        SpecificationDataStorage.Instance.SetWorksList(data)));
            }
            catch { }
        }

        private void FillWorkTypes(string row)
        {
            try
            {
                List<KVPBase> data = JsonConvert.DeserializeObject<List<KVPBase>>(row);
                if (data == null) return;
                if (Application.Current != null)
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        SpecificationDataStorage.Instance.SetWorkTypes(data)));
            }
            catch { }
        }

        private void FillCarStatInfo(string row)
        {
            try
            {
                CarStatInfoModel model = JsonConvert.DeserializeObject<CarStatInfoModel>(row);
                if (model == null) return;
                if (Application.Current != null)
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            SpecificationDataStorage.Instance.SetCarStatInfo(model);
                        }));
            }
            catch { }
        }

        private void FillEnginesByModel(string row)
        {
            try
            {
                var temp = JsonConvert.DeserializeObject<List<KVPBase>>(row);
                if (temp == null) return;
                if (Application.Current != null)
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        SpecificationDataStorage.Instance.SetEngineVolumes(temp)));
            }
            catch { }
        }

        private void FillOtherWorksList(string row)
        {
            try
            {
                List<WorksInfoDataModel> data = JsonConvert.DeserializeObject<List<WorksInfoDataModel>>(row);
                if (data == null) return;
                if (Application.Current != null)
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        SpecificationDataStorage.Instance.SetOtherCarWorks(data)));
            }
            catch { }
        }

        private void FillWorkParts(string row)
        {
            try
            {
                List<WorksInfoDataModel> data = JsonConvert.DeserializeObject<List<WorksInfoDataModel>>(row);
                if (data == null) return;
                if (Application.Current != null)
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        SpecificationDataStorage.Instance.SetPartsWorks(data)));
            }
            catch { }
        }

        private void FillWorkPartsList(string row)
        {
            try
            {
                List<WorksInfoDataModel> data = JsonConvert.DeserializeObject<List<WorksInfoDataModel>>(row);
                if (data == null) return;
                if (Application.Current != null)
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        SpecificationDataStorage.Instance.SetWorkPartsList(data)));
            }
            catch { }
        }

        #endregion TCP
    }
}
