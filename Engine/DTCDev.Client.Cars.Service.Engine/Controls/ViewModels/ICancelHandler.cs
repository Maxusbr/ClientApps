using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Client.Cars.Service.Engine.Controls.ViewModels
{
    interface ICancelHandler
    {
        event EventHandler CancelHandler;
    }
}
