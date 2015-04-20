using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using Newtonsoft.Json;

namespace DTCDev.Client.Window
{
    class SettingsLoader
    {
        private static SettingsLoader _instance;
        public static SettingsLoader Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new SettingsLoader();
                return _instance;
            }
        }

        public SettingsLoader()
        {
            _instance = this;
        }

        public void SaveSettings(string path, IEnumerable<SettingsModel> val)
        {
            string res = JsonConvert.SerializeObject(val);
            if (File.Exists(path))
                File.Delete(path);
            using(TextWriter tw = new StreamWriter(path))
                tw.Write(res);
        }

        internal IEnumerable<SettingsModel> GetSettings(string path)
        {
            if (!File.Exists(path)) return new List<SettingsModel>();
            using (TextReader tr = new StreamReader(path))
            {
                string res = tr.ReadToEnd();
                return JsonConvert.DeserializeObject<IEnumerable<SettingsModel>>(res);
            }
        }
    }
}
