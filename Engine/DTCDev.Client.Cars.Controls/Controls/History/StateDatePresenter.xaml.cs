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
using DTCDev.Models.CarsSending.Car;

namespace DTCDev.Client.Cars.Controls.Controls.History
{
    /// <summary>
    /// Interaction logic for StateDatePresenter.xaml
    /// </summary>
    public partial class StateDatePresenter : UserControl
    {
        public StateDatePresenter()
        {
            InitializeComponent();
        }

        private CarStateModel _displayState;

        private static DependencyProperty DisplayStateProperty = DependencyProperty.Register(
            "DisplayState", typeof(CarStateModel),
            typeof(StateDatePresenter),
            new PropertyMetadata(new CarStateModel(), OnStateRefreshed));

        public CarStateModel DisplayState
        {
            get { return (CarStateModel)GetValue(DisplayStateProperty); }
            set { SetValue(DisplayStateProperty, value); }
        }

        private static void OnStateRefreshed(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            StateDatePresenter cve = sender as StateDatePresenter;
            cve._displayState = (CarStateModel)e.NewValue;
            cve.DisplayParams(cve._displayState);
        }

        private void DisplayParams(CarStateModel model)
        {
            if (model == null)
                return;
            DateTime dt = new DateTime(model.yy, model.Mnth, model.dd, model.hh, model.mm, model.ss);
            txtDate.Text = dt.ToString("dd.MM.yy hh:mm:ss");
        }
    }
}
