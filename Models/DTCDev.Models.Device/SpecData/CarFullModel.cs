
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.Device.SpecData
{
    public class CarFullModel
    {
        private SpeedDataModel _speed = new SpeedDataModel();
        
        public SpeedDataModel Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        private AccelerometerData _accelerometer = new AccelerometerData();
        public AccelerometerData Accelerometer
        {
            get { return _accelerometer; }
            set { _accelerometer = value; }
        }

        public string VIN { get; set; }

        private List<DicClass> _obd = new List<DicClass>();

        public List<DicClass> OBD
        {
            get { return _obd; }
            set { _obd = value; }
        }

        private List<DicClass> _errors = new List<DicClass>();

        public List<DicClass> Errors
        {
            get { return _errors; }
            set { _errors = value; }
        }

        public class DicClass
        {
            public string Key { get; set; }

            public string Value { get; set; }
        }
    }
}
