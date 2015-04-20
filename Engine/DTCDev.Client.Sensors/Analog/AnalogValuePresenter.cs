using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DTCDev.Client.Sensors.Analog
{
    public class AnalogValuePresenter
    {
        private bool isActive = false;

        public bool IsActive
        {
            get { return isActive; }
        }

        private UIElement drivedElement;

        public UIElement DrivedElement
        {
            get
            {
                return drivedElement;
            }
            set
            {
                drivedElement = value;
                isActive = true;
            }
        }

        public void SetValue(decimal MinValue, decimal CurrentValue, decimal MaxValue)
        {
            if (isActive)
            {
                if ((MinValue < CurrentValue) && (CurrentValue < MaxValue))
                    drivedElement.Visibility = Visibility.Collapsed;
                else
                    drivedElement.Visibility = Visibility.Visible;
            }
        }

    }
}
