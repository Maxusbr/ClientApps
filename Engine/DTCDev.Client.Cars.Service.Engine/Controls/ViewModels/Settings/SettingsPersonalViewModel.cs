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

        private ServiceInfoDataModel.DepModel _selectedDepartment;
        public ServiceInfoDataModel.DepModel SelectedDepartment
        {
            get { return _selectedDepartment; }
            set
            {
                _selectedDepartment = value;
                this.OnPropertyChanged("SelectedDepartment");
                FillDepartmentData();
                if (value == null)
                    VisEnableEdit = Visibility.Collapsed;
                else
                    VisEnableEdit = Visibility.Visible;
            }
        }



        #region Edit Department

        private Visibility _visAddDep = Visibility.Collapsed;

        public Visibility VisAddDep
        {
            get { return _visAddDep; }
            set
            {
                _visAddDep = value;
                this.OnPropertyChanged("VisAddDep");
            }
        }

        private Visibility _visEnableEdit = Visibility.Collapsed;
        public Visibility VisEnableEdit
        {
            get { return _visEnableEdit; }
            set
            {
                _visEnableEdit = value;
                this.OnPropertyChanged("VisEnableEdit");
            }
        }

        private string _depName = "";
        private string _depPhones = "";
        private string _depAdress = "";
        private int _depNHCost = 0;
        private int _timeFrom = 9;
        private int _timeTo = 19;
        private ObservableCollection<int> _hours = new ObservableCollection<int>
        {
            0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23
        };

        public string DepName
        {
            get { return _depName; }
            set { _depName = value; this.OnPropertyChanged("DepName"); }
        }

        public ObservableCollection<int> Hours { get { return _hours; } }

        public int TimeFrom
        {
            get { return _timeFrom; }
            set { _timeFrom = value; this.OnPropertyChanged("TimeFrom"); }
        }

        public int TimeTo
        {
            get { return _timeTo; }
            set { _timeTo = value; this.OnPropertyChanged("TimeTo"); }
        }

        public string DepPhones
        {
            get { return _depPhones; }
            set
            {
                _depPhones = value;
                this.OnPropertyChanged("DepPhones");
            }
        }

        public string DepAdress
        {
            get { return _depAdress; }
            set
            {
                _depAdress = value;
                this.OnPropertyChanged("DepAdress");
            }
        }

        public int DepNHCost
        {
            get { return _depNHCost; }
            set
            {
                _depNHCost = value;
                this.OnPropertyChanged("DepNHCost");
            }
        }

        private RelayCommand _backCommand;
        public RelayCommand BackCommand { get { return _backCommand ?? (_backCommand = new RelayCommand(GoBack)); } }

        private RelayCommand _saveCommand;
        public RelayCommand SaveCommand { get { return _saveCommand ?? (_saveCommand = new RelayCommand(SaveChanges)); } }

        private RelayCommand _editDepCommand;
        public RelayCommand EditDepCommand { get { return _editDepCommand ?? (_editDepCommand = new RelayCommand(EditDep)); } }

        private RelayCommand _newDepCommand;
        public RelayCommand NewDepCommand { get { return _newDepCommand ?? (_newDepCommand = new RelayCommand(NewDep)); } }

        private void GoBack(object sender)
        {
            ClearDepVols();
            VisAddDep = Visibility.Collapsed;
        }

        private void SaveChanges(object sender)
        {
            if(SelectedDepartment!=null)
            {
                SelectedDepartment.Adress = DepAdress;
                SelectedDepartment.From = TimeFrom;
                SelectedDepartment.Name = DepName;
                SelectedDepartment.NHCost = DepNHCost;
                SelectedDepartment.Phone = DepPhones;
                SelectedDepartment.To = TimeTo;

                PersonalHandler.Instance.EditDepartment(SelectedDepartment);
            }
            else
            {
                ServiceInfoDataModel.DepModel model = new ServiceInfoDataModel.DepModel()
                {
                    Adress = DepAdress,
                    From = TimeFrom,
                    Name = DepName,
                    NHCost = DepNHCost,
                    Phone = DepPhones,
                    To = TimeTo
                };

                PersonalHandler.Instance.EditDepartment(model);
            }

            ClearDepVols();
            VisAddDep = Visibility.Collapsed;
        }

        private void EditDep(object sender)
        {
            VisAddDep = Visibility.Visible;
        }

        private void NewDep(object sender)
        {
            ClearDepVols();
            VisAddDep = Visibility.Visible;
        }

        private void ClearDepVols()
        {
            DepAdress = "";
            DepName = "";
            DepNHCost = 0;
            DepPhones = "";
        }

        private void FillDepartmentData()
        {
            if (SelectedDepartment == null)
                return;
            else
            {
                DepName = SelectedDepartment.Name;
                DepNHCost = SelectedDepartment.NHCost;
                DepPhones = SelectedDepartment.Phone;
                TimeFrom = SelectedDepartment.From;
                TimeTo = SelectedDepartment.To;
                DepAdress = SelectedDepartment.Adress;
            }
        }

        #endregion Edit Department





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
