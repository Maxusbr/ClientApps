using DTCDev.Client.Cars.Service.Engine.Handlers;
using DTCDev.Client.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Client.Cars.Service.Engine.Controls.ViewModels.Settings
{
    public class ControlPersonalViewModel : ViewModelBase
    {
        public ControlPersonalViewModel()
        {
            PersonalHandler.Instance.PersonsDataLoadComplete += Instance_PersonsDataLoadComplete;
            PersonalHandler.Instance.GetPersons();
        }

        void Instance_PersonsDataLoadComplete(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }


    }
}
