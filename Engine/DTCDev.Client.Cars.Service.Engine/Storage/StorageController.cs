using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Client.Cars.Service.Engine.Storage
{
    public class StorageController
    {
        public static void Start()
        {
            UserSettingsStorage.Instance.Load();
        }

        public static void Stop()
        {
            UserSettingsStorage.Instance.Save();
        }
    }
}
