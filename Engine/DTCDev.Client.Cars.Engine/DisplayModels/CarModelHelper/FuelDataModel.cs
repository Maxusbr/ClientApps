using DTCDev.Client.ViewModel;
using DTCDev.Models.CarsSending.Car;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Client.Cars.Engine.DisplayModels
{
    public class FuelDataModel : ViewModelBase
    {
        #region FuelData

        public void CalculateFuelData(SCarData Data)
        {
            if (_fuelDataPosition < 0)
                return;
            if (Data.Sensors.Count() - 1 < _fuelDataPosition)
                return;
            if (_stepPerLiter < 0.0001m)
                return;
            int vol = Data.Sensors[_fuelDataPosition];
            vol = vol - _startValue;
            decimal fuel = ((decimal)vol) / _stepPerLiter;
            FuelLevelValue = (int)fuel;
            FuelLevel = Data != null ? Data.FuelLevel.ToString("D") : "";
        }

        private string _fuelLevel = "";

        /// <summary>
        /// уровень топлива в автомобиле
        /// </summary>
        public string FuelLevel
        {
            get { return _fuelLevel; }
            set
            {
                _fuelLevel = value;
                this.OnPropertyChanged("FuelLevel");
            }
        }

        private int _fuelLevelValue = 0;
        public int FuelLevelValue
        {
            get { return _fuelLevelValue; }
            set
            {
                _fuelLevelValue = value;
                this.OnPropertyChanged("FuelLevelValue");
            }
        }

        private int _fuelLevelPercents = 0;
        public int FuelLevelPercents
        {
            get { return _fuelLevelPercents; }
            set
            {
                _fuelLevelPercents = value;
                this.OnPropertyChanged("FuelLevelPercents");
            }
        }

        private int _fuelDataPosition = -1;
        public int FuelDataPosition
        {
            get { return _fuelDataPosition; }
            set
            {
                _fuelDataPosition = value;
                this.OnPropertyChanged("FuelDataPosition");
            }
        }

        private int _startValue = 0;
        public int StartFuelValue
        {
            get { return _startValue; }
            set
            {
                _startValue = value;
                this.OnPropertyChanged("StartFuelValue");
            }
        }

        private decimal _stepPerLiter = 0;
        public decimal StepPerLiter
        {
            get { return _stepPerLiter; }
            set
            {
                _stepPerLiter = value;
                this.OnPropertyChanged("StepPerLiter");
            }
        }

        #endregion
    }
}
