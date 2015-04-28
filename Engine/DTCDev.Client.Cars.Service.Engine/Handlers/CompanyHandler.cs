using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCDev.Client.Cars.Service.Engine.Network;
using DTCDev.Models.CarBase.CarList;
using Newtonsoft.Json;

namespace DTCDev.Client.Cars.Service.Engine.Handlers
{
    public class CompanyHandler
    {
        private static CompanyHandler _instance;
        public static CompanyHandler Instance
        {
            get { return _instance ?? (_instance = new CompanyHandler()); }
        }

        public CompanyHandler()
        {
            _instance = this;
        }

        public event EventHandler LoadComplete, LoadError;
        //TODO заменить на модель данных компании
        private void OnLoadComplete(string company)
        {
            if (LoadComplete != null) LoadComplete(company, EventArgs.Empty);
        }

        protected void OnLoadError()
        {
            if (LoadError != null) LoadError(this, EventArgs.Empty);
        }

        public void GetCompany(string id)
        {
            try
            {
                TCPConnection.Instance.SendData("UA" + id);
            }
            catch { }
        }

        public void Split(char fx, string row)
        {
            if (fx != 'a' && fx != 'A') return;
            //TODO разобрать данные по компании, заменить на модель данных компании
            var temp = JsonConvert.DeserializeObject<string>(row);
            if (temp != null)
                OnLoadComplete(temp);
            else
                OnLoadError();
        }
    }
}
