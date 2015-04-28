using DTCDev.Client.Cars.Service.Engine.Handlers;
using DTCDev.Client.Cars.Service.Engine.Storage;
using DTCDev.Client.ViewModel;
using DTCDev.Models.CarsSending.Service;
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
            PersonalStorage.Instance.LoadMastersComplete += Instance_LoadMastersComplete;
            FillMasters();
            if (PersonalStorage.Instance.Masters.Count() < 1)
                PersonalStorage.Instance.LoadMasters();
        }

        void Instance_LoadMastersComplete(object sender, EventArgs e)
        {
            FillMasters();
        }

        private void FillMasters()
        {
            Masters.Clear();
            PersonalStorage.Instance.Masters.ForEach(x => Masters.Add(x));
        }

        void Instance_UserDataLoadComplete(object sender, EventArgs e)
        {
            Login = PersonalHandler.Instance.Login;
            CompanyName = CompanyEditedName = PersonalHandler.Instance.CompanyName;
            CompnayPhone = CompnayEditedPhone = PersonalHandler.Instance.CompanyPhone;
            Adress = PersonalHandler.Instance.Adress;
        }

        private string _login;
        public string Login
        {
            get { return _login; }
            set
            {
                _login = value;
                this.OnPropertyChanged("Login");
            }
        }

        private string _adress;
        public string Adress
        {
            get { return _adress; }
            set
            {
                _adress = value;
                this.OnPropertyChanged("Adress");
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

        private string _companyPhone;
        public string CompnayPhone
        {
            get { return _companyPhone; }
            set
            {
                _companyPhone = value;
                this.OnPropertyChanged("CompnayPhone");
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

        private string _companyEditedPhone;
        public string CompnayEditedPhone
        {
            get { return _companyEditedPhone; }
            set
            {
                _companyEditedPhone = value;
                this.OnPropertyChanged("CompnayEditedPhone");
            }
        }

        private int _companyCost = UserSettingsStorage.Instance.UserSettings.PersonModel.NHCost;
        public int CompanyCost
        {
            get { return _companyCost; }
            set
            {
                _companyCost = value;
                this.OnPropertyChanged("CompanyCost");
            }
        }

        private int _companyEditedCost = UserSettingsStorage.Instance.UserSettings.PersonModel.NHCost;
        public int CompanyEditedCost
        {
            get { return _companyEditedCost; }
            set
            {
                _companyEditedCost = value;
                this.OnPropertyChanged("CompanyEditedCost");
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

        private Visibility _visEditAdress = Visibility.Collapsed;
        public Visibility VisEditAdress
        {
            get { return _visEditAdress; }
            set
            {
                _visEditAdress = value;
                this.OnPropertyChanged("VisEditAdress");
            }
        }

        private Visibility _visEditCost = Visibility.Collapsed;
        public Visibility VisEditCost
        {
            get { return _visEditCost; }
            set
            {
                _visEditCost = value;
                this.OnPropertyChanged("VisEditCost");
            }
        }

        private readonly ObservableCollection<MasterDataModel> _masters = new ObservableCollection<MasterDataModel>();
        public ObservableCollection<MasterDataModel> Masters { get { return _masters; } }

        private MasterDataModel _selectedMaster;
        public MasterDataModel SelectedMaster
        {
            get { return _selectedMaster; }
            set
            {
                _selectedMaster = value;
                this.OnPropertyChanged("SelectedMaster");
                if (value == null)
                {
                    ButtonName = "Добавить";
                }
                else
                {
                    MasterName = value.Name;
                    MasterPhone = value.Phone;
                    ButtonName = "Изменить";
                }
            }
        }

        private string _masterName = "";
        public string MasterName
        {
            get { return _masterName; }
            set
            {
                _masterName = value;
                this.OnPropertyChanged("MasterName");
            }
        }

        private string _masterPhone = "";
        public string MasterPhone
        {
            get { return _masterPhone; }
            set
            {
                _masterPhone = value;
                this.OnPropertyChanged("MasterPhone");
            }
        }

        private string _buttonName = "Добавить";
        public string ButtonName
        {
            get { return _buttonName; }
            set
            {
                _buttonName = value;
                this.OnPropertyChanged("ButtonName");
            }
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

        private RelayCommand _editPhonesCommand;
        public RelayCommand EditPhonesCommand
        {
            get { return _editPhonesCommand ?? (_editPhonesCommand = new RelayCommand(EditPhones)); }
        }
        private void EditPhones(object sender)
        {
            VisEditPhone = Visibility.Visible;
        }

        private RelayCommand _editCostCommand;
        public RelayCommand EditCostCommand
        {
            get { return _editCostCommand ?? (_editCostCommand = new RelayCommand(EditCost)); }
        }
        private void EditCost(object sender)
        {
            VisEditCost = Visibility.Visible;
        }

        private RelayCommand _saveNameCommand;
        public RelayCommand SaveNameCommand
        {
            get { return _saveNameCommand ?? (_saveNameCommand = new RelayCommand(SaveName)); }
        }
        private void SaveName(object sender)
        {
            if (CompanyEditedName != null && CompanyEditedName.Length > 1)
            {
                PersonalHandler.Instance.SaveName(CompanyEditedName);
                CompanyName = CompanyEditedName;
            }
            VisEditName = Visibility.Collapsed;
        }

        private RelayCommand _savePhonesCommand;
        public RelayCommand SavePhonesCommand
        {
            get { return _savePhonesCommand ?? (_savePhonesCommand = new RelayCommand(SavePhones)); }
        }
        private void SavePhones(object sender)
        {
            if (CompnayEditedPhone != null && CompnayEditedPhone.Length > 1)
            {
                PersonalHandler.Instance.SavePhone(CompnayEditedPhone);
                CompnayPhone = CompnayEditedPhone;
            }
            VisEditPhone = Visibility.Collapsed;
        }


        private RelayCommand _saveCostCommand;
        public RelayCommand SaveCostCommand
        {
            get { return _saveCostCommand ?? (_saveCostCommand = new RelayCommand(SaveCost)); }
        }
        private void SaveCost(object sender)
        {
            if (CompanyEditedCost>0)
            {
                PersonalHandler.Instance.SaveCost(CompanyEditedCost);
                CompanyCost = CompanyEditedCost;
                UserSettingsStorage.Instance.UserSettings.PersonModel.NHCost = CompanyCost;
            }
            VisEditCost = Visibility.Collapsed;
        }


        private RelayCommand _saveMastersChangesCommand;
        public RelayCommand SaveMasterChangesCommand { get { return _saveMastersChangesCommand ?? (_saveMastersChangesCommand = new RelayCommand(SaveMasterChanges)); } }
        private void SaveMasterChanges(object sender)
        {
            if (MasterName.Length > 3 && MasterPhone.Length > 5)
            {
                if (SelectedMaster == null)
                {
                    PersonalStorage.Instance.AddMaster(MasterName, MasterPhone);
                }
                else
                {
                    PersonalStorage.Instance.UpdateMaster(MasterName, MasterPhone, SelectedMaster.ID);
                }
                MasterName = "";
                MasterPhone = "";
                SelectedMaster = null;
            }
        }




    }
}
