using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using DTCDev.Client.Cars.Engine.Handlers.Cars;

namespace DTCDev.Client.Cars.Engine.Handlers
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
            Thread.Sleep(10000);
            while(true)
            {
                if (TCPConnection.Instance.IsLogined==true)
                {
                    if (Cars.CarsHandler.Instance.Cars.Count() < 1)
                    {
                        if (_carsTimer <= 0)
                        {
                            Cars.CarsHandler.Instance.Update();
                            _carsTimer = 10;
                        }
                        else
                            _carsTimer--;
                    }
                }
                Thread.Sleep(1000);
            }
        }
    }
}