using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace DTCDev.Client.Cars.Service.Engine.Network
{
    public class UpdateDriver
    {
        private static UpdateDriver _instance;

        public static UpdateDriver Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new UpdateDriver();
                return _instance;
            }
        }

        public UpdateDriver()
        {
            _instance = this;
        }

        public void Start()
        {
            Thread tr = new Thread(ThreadAsker);
            tr.Start();
        }

        private int _updateTicker = 0;
        private int _loginTimer = 2;
        private int _carsTimer = 0;

        private void ThreadAsker()
        {
            
        }
    }
}