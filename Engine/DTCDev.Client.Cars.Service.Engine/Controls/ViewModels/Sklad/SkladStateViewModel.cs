using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using DTCDev.Client.Cars.Service.Engine.Handlers;
using DTCDev.Client.ViewModel;

namespace DTCDev.Client.Cars.Service.Engine.Controls.ViewModels.Sklad
{
    public class SkladStateViewModel : ViewModelBase
    {
        private readonly SkladHandler _handler = SkladHandler.Instance;
        private readonly ObservableCollection<ItemSkladViewModel> _skladeState = new ObservableCollection<ItemSkladViewModel>();
        

        public SkladStateViewModel()
        {
            _handler.LoadSkladStateComplete += _handler_LoadSkladStateComplete;
            _handler.LoadDivisionsComplete += _handler_LoadDivisionsComplete;
            _handler.LoadVendorsComplete += _handler_LoadVendorsComplete;
            _handler.UpdateSkladState();
        }

        void _handler_LoadVendorsComplete(object sender, EventArgs e)
        {
            //TODO Добавить список производителей
        }

        void _handler_LoadDivisionsComplete(object sender, EventArgs e)
        {
            //TODO Добавить список подразделений
        }

        void _handler_LoadSkladStateComplete(object sender, EventArgs e)
        {
            SkladeState.Clear();
            foreach (var item in _handler.SkladState)
            {
                var el = item;
                SkladeState.Add(new ItemSkladViewModel(el));
            }
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                SelectedItem = SkladeState[0];
        }
        /// <summary>
        /// список всех товаров
        /// </summary>
        public ObservableCollection<ItemSkladViewModel> SkladeState { get { return _skladeState; } }

        public ItemSkladViewModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if(_selectedItem != null) _selectedItem.PropertyChanged -= SelectedItemOnPropertyChanged;
                _selectedItem = value;
                EnableButtonSave = false;
                if (_selectedItem != null) _selectedItem.PropertyChanged += SelectedItemOnPropertyChanged;
                OnPropertyChanged("SelectedItem");
                OnPropertyChanged("VisableDetail");
            }
        }

        private void SelectedItemOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            EnableButtonSave =SelectedItem != null && !string.IsNullOrEmpty(SelectedItem.Name) && 
                !string.IsNullOrEmpty(SelectedItem.ArtNo);
        }


        public bool EnableButtonSave
        {
            get { return _enableButtonNext; }
            set
            {
                _enableButtonNext = value;
                OnPropertyChanged("EnableButtonSave");
            }
        }

        public bool VisableDetail { get { return SelectedItem != null; } }
        private object _selectedIndexTab = 0;
        public object SelectedIndexTab
        {
            get { return _selectedIndexTab; }
            set
            {
                _selectedIndexTab = value;
                OnPropertyChanged("SelectedIndexTab");
            }
        }

        private RelayCommand _addCommand;
        private ItemSkladViewModel _selectedItem;
        private RelayCommand _saveCommand;
        private bool _enableButtonNext;


        public RelayCommand AddCommand
        {
            get { return _addCommand ?? (_addCommand = new RelayCommand(AddItemTosklad)); }
        }

        private void AddItemTosklad(object obj)
        {
            SelectedIndexTab = 1;
            if (SelectedItem == null) SelectedItem = new ItemSkladViewModel();
        }


        public RelayCommand AddPositionCommand
        {
            get { return _addPositionCommand ?? (_addPositionCommand = new RelayCommand(AddItem)); }
        }

        private void AddItem(object obj)
        {
            SelectedIndexTab = 0;
            SelectedItem = new ItemSkladViewModel{Uses = _handler.Uses[0], Type = _handler.Types[0]};
        }

        /// <summary>
        /// Сохранить изменения позиции в справочнике
        /// </summary>
        public RelayCommand SaveCommand
        {
            get { return _saveCommand ?? (_saveCommand = new RelayCommand(Save)); }
        }

        /// <summary>
        /// Сохранить изменения позиции на складе
        /// </summary>
        public RelayCommand SaveAddPositionCommand
        {
            get { return _saveAddPositionCommand ?? (_saveAddPositionCommand = new RelayCommand(SaveAddPosition)); }
        }

        private void SaveAddPosition(object obj)
        {
            _handler.AddSkladPosition(SelectedItem.GetModel);
        }

        private RelayCommand _deleteCommand;
        private RelayCommand _addPositionCommand;
        private RelayCommand _saveAddPositionCommand;
        private RelayCommand _cancelCommand;

        public RelayCommand DeleteCommand
        {
            get { return _deleteCommand ?? (_deleteCommand = new RelayCommand(Delete)); }
        }

        public RelayCommand CancelCommand
        {
            get { return _cancelCommand ?? (_cancelCommand = new RelayCommand(Cancel)); }
        }

        private void Cancel(object obj)
        {
            _handler.LoadCache();
        }


        private void Delete(object obj)
        {
            
        }


        private void Save(object obj)
        {
            _handler.AddPosition(SelectedItem.Model);
            SelectedItem = null;
        }
    }
}
