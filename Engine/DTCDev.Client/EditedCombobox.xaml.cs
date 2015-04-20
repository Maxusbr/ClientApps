using DTCDev.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DTCDev.Client
{
    /// <summary>
    /// Interaction logic for EditedCombobox.xaml
    /// </summary>
    public partial class EditedCombobox : UserControl
    {
        public EditedCombobox()
        {
            Items = new ObservableCollection<DicDataModel>();
            InitializeComponent();
            Items.CollectionChanged += Items_CollectionChanged;
        }

        void Items_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            FillCombobox();
        }


        private static readonly DependencyProperty ItemsProperty = DependencyProperty.Register("Items",
             typeof(ObservableCollection<DicDataModel>),
             typeof(EditedCombobox),
             new FrameworkPropertyMetadata(new ObservableCollection<DicDataModel>(), null));

        public ObservableCollection<DicDataModel> Items
        {
            get { return (ObservableCollection<DicDataModel>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }





        public string DefaultText
        {
            get { return txtDefault.Text; }
            set { txtDefault.Text = value; }
        }




        private static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem",
            typeof(DicDataModel),
             typeof(EditedCombobox),
             new PropertyMetadata(new DicDataModel(), OnSelectedItemPropertyChanged));

        private static void OnSelectedItemPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            EditedCombobox control = (EditedCombobox)sender;
            DicDataModel model = (DicDataModel)e.NewValue;
            control.ChangeSelectedItem(model);
        }

        private DicDataModel SelectedItem
        {
            get { return (DicDataModel)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        //private static readonly DependencyProperty TypedTextProperty = DependencyProperty.Register("TypedText",
          //  typeof(string),








        private void FillCombobox()
        {
            cbxData.Items.Clear();
            cbxData.SelectedIndex = -1;
            txtDefault.Visibility = Visibility.Visible;
            if (Items.Count() < 1)
            {
                cbxData.Visibility = Visibility.Collapsed;
            }
            else
            {
                cbxData.Visibility = Visibility.Visible;
                foreach (var item in Items)
                {
                    cbxData.Items.Add(item.Data);
                }
            }
        }

        private void cbxData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbxData.SelectedIndex < 0)
            {
                txtDefault.Visibility = Visibility.Visible;
                txtData.Text = "";
            }
            else
            {
                txtDefault.Visibility = Visibility.Collapsed;
                txtData.Text = cbxData.SelectedItem.ToString();
                if (cbxData.SelectedIndex < Items.Count())
                {
                    SelectedItem = Items[cbxData.SelectedIndex];
                }
            }
        }

        private void ChangeSelectedItem(DicDataModel model)
        {
            txtData.Visibility = Visibility.Visible;
            txtDefault.Visibility = Visibility.Collapsed;
            txtData.Text = model.Data;
        }

        private void txtData_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtData.Text.Length > 0)
            {
                txtDefault.Visibility = Visibility.Collapsed;

            }
            else
            {
                txtDefault.Visibility = Visibility.Visible;
            }
        }
    }
}
