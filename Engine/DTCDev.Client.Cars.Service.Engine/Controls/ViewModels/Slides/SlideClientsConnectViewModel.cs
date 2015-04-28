using DTCDev.Client.Cars.Service.Engine.Handlers;
using DTCDev.Client.Cars.Service.Engine.Storage;
using DTCDev.Client.ViewModel;
using DTCDev.Models.CarBase.CarStatData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace DTCDev.Client.Cars.Service.Engine.Controls.ViewModels.Slides
{
    public class SlideClientsConnectViewModel : ViewModelBase
    {
        public SlideClientsConnectViewModel()
        {
            MonthesFrom = new ObservableCollection<int>();
            MonthesAfter = new ObservableCollection<int>();
            for (int i = 0; i < 6; i++)
            {
                MonthesFrom.Add(i + 1);
            }
            for (int i = 5; i < 18; i++)
            {
                MonthesAfter.Add(i + 1);
            }
            SpecificationDataStorage.Instance.Update();
            ClientsHandler.Instance.SendingUserCountUpdated += Instance_SendingUserCountUpdated;
        }

        void Instance_SendingUserCountUpdated(object sender, EventArgs e)
        {
            MessagesCount = ClientsHandler.Instance.UserCount;
        }





        public ObservableCollection<int> MonthesFrom { get; set; }

        public ObservableCollection<int> MonthesAfter { get; set; }

        public ObservableCollection<KVPBase> Marks { get { return SpecificationDataStorage.Instance.Marks; } }




        private bool _sendSMS = true;
        public bool SendSMS
        {
            get { return _sendSMS; }
            set
            {
                _sendSMS = value;
                this.OnPropertyChanged("SendSMS");
                if (value == true)
                {
                    SendMail = false;
                    SendProgram = false;
                }
            }
        }

        private bool _sendMail = false;
        public bool SendMail
        {
            get { return _sendMail; }
            set
            {
                _sendMail = value;
                this.OnPropertyChanged("SendMail");
                if (value == true)
                {
                    SendSMS = false;
                    SendProgram = false;
                }
            }
        }

        private bool _sendProgram = false;
        public bool SendProgram
        {
            get { return _sendProgram; }
            set
            {
                _sendProgram = value;
                this.OnPropertyChanged("SendProgram");
                if (value)
                {
                    SendMail = false;
                    SendSMS = false;
                }
            }
        }

        private bool _sendClientsFrom = true;
        public bool SendClientsFrom
        {
            get { return _sendClientsFrom; }
            set
            {
                _sendClientsFrom = value;
                this.OnPropertyChanged("SendClientsFrom");
                if (value)
                {
                    SendClientsAfter = false;
                    SendByMark = false;
                }
            }
        }

        private bool _sendClientsAfter = false;
        public bool SendClientsAfter
        {
            get { return _sendClientsAfter; }
            set
            {
                _sendClientsAfter = value;
                this.OnPropertyChanged("SendClientsAfter");
                if (value)
                {
                    SendClientsFrom = false;
                    SendByMark = false;
                }
            }
        }

        private bool _sendByMark = false;
        public bool SendByMark
        {
            get { return _sendByMark; }
            set
            {
                _sendByMark = value;
                this.OnPropertyChanged("SendByMark");
                if (value)
                {
                    SendClientsAfter = false;
                    SendClientsFrom = false;
                }
            }
        }

        private KVPBase _selectedMark;
        public KVPBase SelectedMark
        {
            get { return _selectedMark; }
            set
            {
                _selectedMark = value;
                this.OnPropertyChanged("SelectedMark");
                if (value != null)
                    StartUpdateClients();
            }
        }

        private int? _selectedSendFrom;
        public int? SelectedSendFrom
        {
            get { return _selectedSendFrom; }
            set
            {
                _selectedSendFrom = value;
                this.OnPropertyChanged("SelectedSendFrom");
                if (value != null)
                    StartUpdateClients();
            }
        }

        private int? _selectedSendAfter;
        public int? SelectedSendAfter
        {
            get { return _selectedSendAfter; }
            set
            {
                _selectedSendAfter = value;
                this.OnPropertyChanged("SelectedSendAfter");
                if (value != null)
                    StartUpdateClients();
            }
        }

        private int _messagesCount = 0;
        public int MessagesCount
        {
            get { return _messagesCount; }
            set
            {
                _messagesCount = value;
                this.OnPropertyChanged("MessagesCount");
            }
        }





        private void StartUpdateClients()
        {
            if (SendClientsFrom)
                ClientsHandler.Instance.GetClientsCount((int)SelectedSendFrom, Models.CarsSending.Sending.SendingClientsEnum.SendModel.From);
            else if (SendClientsAfter)
                ClientsHandler.Instance.GetClientsCount((int)SelectedSendAfter, Models.CarsSending.Sending.SendingClientsEnum.SendModel.After);
            else if (SendByMark)
                ClientsHandler.Instance.GetClientsCount(SelectedMark.id, Models.CarsSending.Sending.SendingClientsEnum.SendModel.ByMark);
        }
    }
}
