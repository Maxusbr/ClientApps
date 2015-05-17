using DTCDev.Client.Cars.Service.Engine.Handlers;
using DTCDev.Client.Cars.Service.Engine.Storage;
using DTCDev.Client.ViewModel;
using DTCDev.Models.CarsSending.Service;
using DTCDev.Models.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace DTCDev.Client.Cars.Service.Engine.Controls.ViewModels.Settings
{
    public class SettingsPersonalViewModel : ViewModelBase
    {
        public SettingsPersonalViewModel()
        {
            PersonalHandler.Instance.UserDataLoadComplete += Instance_UserDataLoadComplete;
            PersonalHandler.Instance.GetUserData();
            if (PersonalStorage.Instance.Masters.Count() < 1)
                PersonalStorage.Instance.LoadMasters();
        }

        void Instance_UserDataLoadComplete(object sender, EventArgs e)
        {
            CompanyName = CompanyEditedName = PersonalHandler.Instance.CompanyName;
            Departments.Clear();
            foreach (var item in PersonalHandler.Instance.Model.Departments)
            {
                Departments.Add(item);
            }
        }


        private string _companyName;
        public string CompanyName
        {
            get { return _companyName; }
            set
            {
                _companyName = value;
                this.OnPropertyChanged("CompanyName");
            }
        }

        private string _companyEditedName;
        public string CompanyEditedName
        {
            get { return _companyEditedName; }
            set
            {
                _companyEditedName = value;
                this.OnPropertyChanged("CompanyEditedName");
            }
        }

        

        private Visibility _visEditName = Visibility.Collapsed;
        public Visibility VisEditName
        {
            get { return _visEditName; }
            set
            {
                _visEditName = value;
                this.OnPropertyChanged("VisEditName");
            }
        }

        private Visibility _visEditPhone = Visibility.Collapsed;
        public Visibility VisEditPhone
        {
            get { return _visEditPhone; }
            set
            {
                _visEditPhone = value;
                this.OnPropertyChanged("VisEditPhone");
            }
        }

        private ObservableCollection<ServiceInfoDataModel.DepModel> _departments = new ObservableCollection<ServiceInfoDataModel.DepModel>();

        public ObservableCollection<ServiceInfoDataModel.DepModel> Departments
        {
            get { return _departments; }
        }

        








        private RelayCommand _editNameCommand;
        public RelayCommand EditNameCommand
        {
            get { return _editNameCommand ?? (_editNameCommand = new RelayCommand(EditName)); }
        }

        private void EditName(object sender)
        {
            VisEditName = Visibility.Visible;
        }
    }
}
