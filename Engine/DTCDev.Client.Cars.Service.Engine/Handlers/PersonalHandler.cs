using DTCDev.Client.Cars.Service.Engine.Network;
using DTCDev.Client.Cars.Service.Engine.Storage;
using DTCDev.Models;
using DTCDev.Models.CarBase.CarStatData;
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

        private ServiceInfoDataModel _model = new ServiceInfoDataModel();
        public ServiceInfoDataModel Model
        {
            get { return _model; }
        }

        public event EventHandler SelectedDepRereshed;


        private ServiceInfoDataModel.DepModel _selectedDep;
        public ServiceInfoDataModel.DepModel SelectedDep
        {
            get { return _selectedDep; }
            set
            {
                _selectedDep = value;
                if (SelectedDepRereshed != null)
                    SelectedDepRereshed(this, new EventArgs());
                GetPersons();
            }
        }

        public event EventHandler UserDataLoadComplete;
        public event EventHandler EmployeeDataLoadComplete;


        public delegate void UserAddCompleteHandler(EmployeeModel model);
        public event UserAddCompleteHandler UserAddComplete;

        public delegate void EmployeeRolesLoadedHandler(List<DicDataModel> data);
        public event EmployeeRolesLoadedHandler EmployeeRolesLoaded;

        protected virtual void OnUserAddComplete(EmployeeModel model)
        {
            if (UserAddComplete != null) UserAddComplete(model);
        }


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

        /// <summary>
        /// Запрос на редактирование/добавление департамента
        /// </summary>
        /// <param name="model"></param>
        public void EditDepartment(ServiceInfoDataModel.DepModel model)
        {
            SendRequest("UK" + JsonConvert.SerializeObject(model));
        }

        /// <summary>
        /// Запрос на редактирование/добавление департамента
        /// </summary>
        /// <param name="model"></param>
        public void EditPost(ServiceInfoDataModel.PostSettings model)
        {
            SendRequest("UL" + JsonConvert.SerializeObject(model));
        }

        /// <summary>
        /// Запрос списка работников для департамента
        /// </summary>
        public void GetPersons()
        {
            if (_selectedDep == null)
                return;
            else
            {
                SendRequest("UM" + _selectedDep.id.ToString());
            }
        }

        /// <summary>
        /// Запрос на редактирование/добавление работника
        /// </summary>
        /// <param name="model"></param>
        internal void EditUser(EmployeeModel model)
        {
            if (SelectedDep == null)
                return;
            model.DepartmentsIDs.Add(SelectedDep.id);
            SendRequest("UO" + JsonConvert.SerializeObject(model));
        }

        /// <summary>
        /// Получить список должностей
        /// </summary>
        internal void GetRoles()
        {
            SendRequest("UN");
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
                case 'k':
                case 'K':
                    //after add|edit department refresh settings model
                    GetUserData();
                    break;
                case 'm':
                case 'M':
                    ParseEmployers(row);
                    break;
                case 'n':
                case 'N':
                    ParseRoles(row);
                    break;
                case 'o':
                case 'O':
                    GetPersons();
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
                                _model = model;
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

        /// <summary>
        /// parse employers list data. Call EmployeeDataLoadComplete after complete
        /// </summary>
        /// <param name="row"></param>
        private void ParseEmployers(string row)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<List<EmployeeModel>>(row);
                if (EmployeeDataLoadComplete != null && Application.Current != null)
                    Application.Current.Dispatcher.BeginInvoke(new Action(() => EmployeeDataLoadComplete(data, new EventArgs())));
            }
            catch { }
        }

        private void ParseRoles(string row)
        {
            try
            {
                var roles = JsonConvert.DeserializeObject<List<DicDataModel>>(row);
                if (Application.Current != null && EmployeeRolesLoaded!=null)
                    Application.Current.Dispatcher.BeginInvoke(new Action(() => EmployeeRolesLoaded(roles)));
            }
            catch
            {

            }

        }

        #endregion
    }
}
