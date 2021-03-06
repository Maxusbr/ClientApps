﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCDev.Client.Cars.Engine.DisplayModels;

namespace DTCDev.Client.Cars.Engine.AppLogic
{
    public class CarSelector
    {
        private static DISP_Car _selectedCar;
        private static DISP_Car _detailsCar;

        public static DISP_Car SelectedCar
        {
            get
            {
                return _selectedCar;
            }
            set
            {
                if(_selectedCar == value) return;
                _selectedCar = value;
                if (OnCarChanged != null)
                    OnCarChanged(_selectedCar);
            }
        }

        public static DISP_Car ViewCarDetailsCar
        {
            get { return _detailsCar; }
            set
            {
                _detailsCar = value;
                if (ViewCarDetails != null)
                    ViewCarDetails(_detailsCar);
            }
        }

        public delegate void OnCarChangedHandler(DISP_Car car);
        public delegate void OnCarMoreSelectedHandler(IEnumerable<string> cars);

        public static event OnCarChangedHandler OnCarChanged;
        public static event OnCarChangedHandler ViewCarDetails;
        public static event OnCarMoreSelectedHandler OnCarSelected;
        public static event EventHandler ClearSelection;

        public static void CreateCarSelected(IEnumerable<string> cars)
        {
            if (OnCarSelected != null) OnCarSelected(cars);
        }

        public static void OnClearSelection()
        {
            if (ClearSelection != null) ClearSelection(null, EventArgs.Empty);
        }
    }
}
