using DTCDev.Client.Cars.Service.Engine.Handlers;
using DTCDev.Client.ViewModel;
using DTCDev.Models.User;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace DTCDev.Client.Cars.Service.Engine.Controls.ViewModels.Clients
{
    public class SlideClientsListViewModel : ViewModelBase
    {
        public SlideClientsListViewModel()
        {
            ClientsHandler.Instance.UsersUpdated += Instance_UsersUpdated;
            ClientsHandler.Instance.GetClients();
        }

        void Instance_UsersUpdated(object sender, EventArgs e)
        {
            Clients.Clear();
            foreach (var item in ClientsHandler.Instance.Users)
            {
                Clients.Add(item);
            }
        }





        private ObservableCollection<UserLightModel> _clients = new ObservableCollection<UserLightModel>();
        public ObservableCollection<UserLightModel> Clients
        {
            get { return _clients; }
        }


    }
}
