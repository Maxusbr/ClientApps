using DTCDev.Client.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Client.Cars.Engine.DisplayModels
{
    public class ZoneModel : ViewModelBase
    {

        private int _zoneId = -1;
        /// <summary>
        /// принадлежность автомобиля к зоне
        /// </summary>
        public int ZoneId
        {
            get { return _zoneId; }
            set
            {
                if (_zoneId != value)
                {
                    _zoneId = value;
                    OnPropertyChanged("ZoneId");
                }
            }
        }

        private bool inzone = true;
        /// <summary>
        /// Находится ли автомобиль в зоне
        /// </summary>
        public bool InZone
        {
            get { return inzone; }
            set
            {
                inzone = value;
                OnPropertyChanged("InZone");
            }
        }
    }
}
