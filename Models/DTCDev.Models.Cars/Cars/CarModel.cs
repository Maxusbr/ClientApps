using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.Car.Cars
{
    public class CarModel
    {
        public int id { get; set; }

        public int idCompany { get; set; }

        public int ProduceMonth { get; set; }

        public int ProduceYear { get; set; }

        public int BuyMonth { get; set; }

        public int BuyYear { get; set; }

        public int CurrentDistance { get; set; }

        public string Number { get; set; }

        public int Distance { get; set; }

        public CarExemplarModel Exemplar { get; set; }

        private string id_Device = "";

        public string ID_Device
        {
            get { return id_Device; }
            set
            {
                id_Device = value;
            }
        }

        public int id_StatDriver { get; set; }

        public string DriverName { get; set; }

        public string Link { get; set; }

        public string VIN { get; set; }

        public int DayMove { get; set; }

        public int FuelPosition { get; set; }

        public int FuelStartValue { get; set; }

        public decimal FuelStepPerLiter { get; set; }
    }
}
