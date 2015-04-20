using System;
using System.Collections.Generic;
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

namespace DTCDev.Client.Menu
{
    /// <summary>
    /// Interaction logic for AEDControl.xaml
    /// </summary>
    public partial class AEDControl : UserControl
    {
        public AEDControl()
        {
            InitializeComponent();
        }

        private static DependencyProperty AddCommandProperty = DependencyProperty.Register(
            "AddCommand", typeof(ICommand), typeof(AEDControl),
            new PropertyMetadata(null));

        public ICommand AddCommand
        {
            get { return (ICommand)GetValue(AddCommandProperty); }
            set { SetValue(AddCommandProperty, value); }
        }

        private static DependencyProperty ChangeCommandProperty = DependencyProperty.Register(
            "ChangeCommand", typeof(ICommand), typeof(AEDControl),
            new PropertyMetadata(null));

        public ICommand ChangeCommand
        {
            get { return (ICommand)GetValue(ChangeCommandProperty); }
            set { SetValue(ChangeCommandProperty, value); }
        }


        private static DependencyProperty DeleteCommandProperty = DependencyProperty.Register(
            "DeleteCommand", typeof(ICommand), typeof(AEDControl),
            new PropertyMetadata(null));

        public ICommand DeleteCommand
        {
            get { return (ICommand)GetValue(DeleteCommandProperty); }
            set { SetValue(DeleteCommandProperty, value); }
        }

        public event EventHandler AddClick;

        private void hlpnkAdd_Click(object sender, RoutedEventArgs e)
        {
            if (AddClick != null)
                AddClick(this, new EventArgs());
        }

        private bool _editEnable = true;
        public bool EditEnable
        {
            get { return _editEnable; }
            set
            {
                _editEnable = value;
                if (value)
                    grdEdit.Visibility = Visibility.Visible;
                else
                    grdEdit.Visibility = Visibility.Collapsed;
            }
        }

        private bool _deleteEnable = true;
        public bool DeleteEnable
        {
            get { return _deleteEnable; }
            set
            {
                _deleteEnable = value;
                if (value)
                    grdDelete.Visibility = Visibility.Visible;
                else
                    grdDelete.Visibility = Visibility.Collapsed;
            }
        }
    }
}
