using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCDev.Models.CarsSending.Car;

namespace DTCDev.Client.Cars.Controls.ViewModels.History
{
    public class LoadedHistoryRows
    {
        public DateTime Date { get; set; }

        public string StringDate { get { return Date.ToString("dd.MM.yyyy"); } }

        private double _mileage;

        public double Mileage
        {
            get { return Math.Round(_mileage, 2); }
            set
            {
                _mileage = value;
            }
        }

        public int MiddleSpeed { get; set; }

        public List<CarStateModel> Data { get; set; }

        public string Start { get; set; }

        public string Stop { get; set; }
    }
}
