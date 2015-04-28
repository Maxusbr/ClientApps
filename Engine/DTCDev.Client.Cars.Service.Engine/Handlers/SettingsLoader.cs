using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using DTCDev.Models.CarBase.CarList;
using DTCDev.Client.Cars.Service.Engine.Storage;

namespace DTCDev.Client.Cars.Service.Engine.Handlers
{
    class SettingsLoader
    {
        private static SettingsLoader _instance;
        public static SettingsLoader Instance
        {
            get { return _instance ?? (_instance = new SettingsLoader()); }
        }

        public SettingsLoader()
        {
            _instance = this;
        }

        public void Save()
        {
            try
            {
                var md = CheckDirs() + "\\Cars.m2bs";
                if (File.Exists(md) == true)
                    File.Delete(md);

                var xml = new XmlSerializer(typeof(List<CarListBaseDataModel>));
                using (TextWriter tw = new StreamWriter(md))
                    xml.Serialize(tw, CarStorage.Instance.Cars);
            }
            catch { }

        }

        public string CheckDirs()
        {
            var md = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\M2B\\ServiceViewer";
            if (Directory.Exists(md) == false)
                Directory.CreateDirectory(md);
            return md;
        }
    }
}
