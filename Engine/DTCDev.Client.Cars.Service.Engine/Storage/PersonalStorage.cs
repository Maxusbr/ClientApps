using DTCDev.Client.Cars.Service.Engine.Handlers;
using DTCDev.Models.CarsSending.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Client.Cars.Service.Engine.Storage
{
    public class PersonalStorage
    {
        private static PersonalStorage _instance;

        public static PersonalStorage Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new PersonalStorage();
                return _instance;
            }
        }

        public PersonalStorage()
        {
            _instance = this;
        }

        private List<MasterDataModel> _masters = new List<MasterDataModel>();
        public List<MasterDataModel> Masters
        {
            get { return _masters; }
        }

        private int _currentMasterID = -1;

        private int _currentBalance = 0;
        public int CurrentBalance
        {
            get { return _currentBalance; }
        }

        private int _currentSMS = 0;
        public int CurrentSMS
        {
            get { return _currentSMS; }
        }

        private MasterDataModel _currentMaster;
        public MasterDataModel CurrentMaster
        {
            get { return _currentMaster; }
        }




        public event EventHandler LoadMastersComplete;
        public event EventHandler LoadCurrentMasterComplete;
        public event EventHandler CurrentBalanceLoaded;



        public void FillMasters(List<MasterDataModel> data)
        {
            _masters = data;
            if (LoadMastersComplete != null)
                LoadMastersComplete(this, new EventArgs());
            PersonalHandler.Instance.GetCurrentMaster();
        }

        public void FillCurrentMaster(int id)
        {
            _currentMasterID = id;
            _currentMaster = Masters.Where(p => p.ID == id).FirstOrDefault();
            if (LoadCurrentMasterComplete != null)
                LoadCurrentMasterComplete(this, new EventArgs());
        }



        public void LoadMasters()
        {
            PersonalHandler.Instance.GetMastersList();
        }

        public bool AddMaster(string name, string phone)
        {
            if (Masters.Where(p => p.Name == name && p.Phone == phone).FirstOrDefault() == null)
            {
                PersonalHandler.Instance.AddMaster(name, phone);
                return true;
            }
            else
                return false;
        }

        public void UpdateMaster(string name, string phone, int ID)
        {
            PersonalHandler.Instance.UpdateMaster(name, phone, ID);
        }

        public void SetCurrentMaster(MasterDataModel model)
        {
            _currentMaster = model;
            _currentMasterID = model.ID;
            PersonalHandler.Instance.SetCurrentMaster(model.ID);
        }

        public void GetCurrentBalance()
        {
        }


    }
}
