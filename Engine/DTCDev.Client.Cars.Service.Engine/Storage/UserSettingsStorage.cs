using DTCDev.Client.Cars.Service.Engine.Handlers;
using DTCDev.Models.User;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DTCDev.Client.Cars.Service.Engine.Storage
{
    public class UserSettingsStorage
    {
        private static UserSettingsStorage _instance;

        public static UserSettingsStorage Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new UserSettingsStorage();
                return _instance;
            }
        }

        public UserSettingsStorage()
        {
            _instance = this;
        }

        private UserSettingsModel _settings = new UserSettingsModel();
        public UserSettingsModel UserSettings
        {
            get { return _settings; }
            set
            {
                _settings = value;
            }
        }

        private bool _firstLoad = true;
        public bool FirstLoad
        {
            get { return _firstLoad; }
            set { _firstLoad = value; }
        }



        public void Init()
        {
            PersonalHandler.Instance.UserDataLoadComplete += Instance_UserDataLoadComplete;
        }

        void Instance_UserDataLoadComplete(object sender, EventArgs e)
        {
            UserSettings.PersonModel = PersonalHandler.Instance.Model;
        }


        public void Save()
        {
            try
            {
                string data = JsonConvert.SerializeObject(UserSettings);
                string docPath = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string way = docPath + "\\M2B\\ServiceViewer\\";
                if (Directory.Exists(way))
                {
                    string file = way + "user.m2bs";
                    if (File.Exists(file))
                    {
                        File.Delete(file);
                    }

                    StreamWriter sw = File.CreateText(file);
                    sw.WriteLine(data);
                    sw.Close();
                }
            }
            catch { }
        }

        public void Load()
        {
            try
            {
                string docPath = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string way = docPath + "\\M2B\\ServiceViewer\\";
                if (Directory.Exists(way))
                {
                    string file = way + "user.m2bs";
                    if (File.Exists(file))
                    {
                        StreamReader sr = File.OpenText(file);
                        string data = sr.ReadToEnd();
                        UserSettingsModel temp = JsonConvert.DeserializeObject<UserSettingsModel>(data);
                        if (temp != null)
                            UserSettings = temp;
                    }
                }
                CarStorage.Instance.LastCarNumbers = UserSettings.LastCarNumbers;
            }
            catch { }
        }

        public class UserSettingsModel
        {
            public UserSettingsModel()
            {
                LastCarNumbers = new List<string>();
            }

            public bool OpenedLastCars { get; set; }

            public bool OpenedHelp { get; set; }

            public List<string> LastCarNumbers { get; set; }

            PersonalDataModel _person = new PersonalDataModel();
            public PersonalDataModel PersonModel
            {
                get { return _person; }
                set { _person = value; }
            }
        }
    }
}
