using DTCDev.Client.Cars.Service.Engine.Network;
using DTCDev.Client.Cars.Service.Engine.Storage;
using DTCDev.Models.CarsSending.Service;
using DTCDev.Models.Service;
using DTCDev.Models.User;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace DTCDev.Client.Cars.Service.Engine.Handlers
{
    public class PersonalHandler
    {
        private static PersonalHandler _instance;
        public static PersonalHandler Instance
        {
            get { return _instance ?? (_instance = new PersonalHandler()); }
        }

        public PersonalHandler()
        {
            _instance = this;
        }


        public string Login { get; set; }

        public string CompanyName { get; set; }

        public string CompanyPhone { get; set; }

        public string Adress { get; set; }

        private PersonalDataModel _model = new PersonalDataModel();
        public PersonalDataModel Model
        {
            get { return _model; }
        }

        public event EventHandler UserDataLoadComplete;




        public void GetUserData()
        {
            SendRequest("UA");
        }

        public void SaveName(string name)
        {
            SendRequest("UB" + name);
        }

        public void SavePhone(string phone)
        {
            SendRequest("UC"+phone);
        }

        public void SaveCost(int cost)
        {
            SendRequest("UD" + cost.ToString());
        }

        public void GetMastersList()
        {
            SendRequest("UE");
        }

        public void AddMaster(string name, string phone)
        {
            MasterDataModel model = new MasterDataModel
            {
                Name = name,
                Phone = phone
            };
            SendRequest("UF" + JsonConvert.SerializeObject(model));
        }

        public void UpdateMaster(string name, string phone, int id)
        {
            MasterDataModel model = new MasterDataModel
            {
                Name = name,
                Phone = phone,
                ID = id
            };
            SendRequest("UG" + JsonConvert.SerializeObject(model));
        }

        public void GetCurrentMaster()
        {
            SendRequest("UH");
        }

        public void SetCurrentMaster(int id)
        {
            SendRequest("UI" + id.ToString());
        }

        public void GetBalance()
        {
            SendRequest("UJ");
        }

        

        private void SendRequest(string req)
        {
            try
            {
                TCPConnection.Instance.SendData(req);
            }
            catch { }
        }



        #region TCP

        public void Split(char fx, string row)
        {
            switch (fx)
            {
                case 'a':
                case 'A':
                    ParseUserData(row);
                    break;
                case 'e':
                case 'E':
                    ParseMasters(row);
                    break;
                case 'f':
                case 'F':
                    GetMastersList();
                    break;
                case 'g':
                case 'G':
                    GetMastersList();
                    break;
                case 'h':
                case 'H':
                    ConvertCurrentMaster(row);
                    break;
            }
        }

        private void ParseUserData(string row)
        {
            try
            {
                ServiceInfoDataModel model = JsonConvert.DeserializeObject<ServiceInfoDataModel>(row);
                if (model != null)
                {
                    if (Application.Current != null)
                        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                CompanyName = model.Name;
                                if (UserDataLoadComplete != null)
                                    UserDataLoadComplete(this, new EventArgs());
                            }));
                }
            }
            catch { }
        }

        private void ParseMasters(string row)
        {
            try
            {
                List<MasterDataModel> temp = JsonConvert.DeserializeObject<List<MasterDataModel>>(row);
                if (Application.Current != null)
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            PersonalStorage.Instance.FillMasters(temp);
                        }));
            }
            catch { }
        }

        private void ConvertCurrentMaster(string row)
        {
            int id = 0;
            Int32.TryParse(row, out id);
            if (Application.Current != null)
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        PersonalStorage.Instance.FillCurrentMaster(id);
                    }));
        }

        #endregion
    }
}
