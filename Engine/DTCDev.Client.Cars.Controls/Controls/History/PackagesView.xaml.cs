using DTCDev.Client.Cars.Controls.ViewModels.History;
using DTCDev.Client.Cars.Engine.AppLogic;
using DTCDev.Client.Cars.Engine.DisplayModels;
using DTCDev.Models;
using DTCDev.Models.CarsSending.Car;
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

namespace DTCDev.Client.Cars.Controls.Controls.History
{
    /// <summary>
    /// Логика взаимодействия для PackagesView.xaml
    /// </summary>
    public partial class PackagesView : UserControl
    {
        public PackagesView()
        {
            InitializeComponent();
            CarSelector.OnCarChanged += CarSelector_OnCarChanged;
        }

        void CarSelector_OnCarChanged(Engine.DisplayModels.DISP_Car car)
        {
            _car = car;
        }

        private LinesDataModel _data;
        private DISP_Car _car;
        List<CarStateModel> _track;

        private static DependencyProperty DataProperty = DependencyProperty.Register("Data", typeof(LinesDataModel),
            typeof(PackagesView),
            new PropertyMetadata(new LinesDataModel(), DataPropertyChanged));

        public LinesDataModel Data
        {
            get { return (LinesDataModel)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        private static void DataPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            PackagesView control = sender as PackagesView;
            control._data = (LinesDataModel)e.NewValue;
            control.DisplayData();
        }

        private static DependencyProperty TrackProperty = DependencyProperty.Register("Track", typeof(List<CarStateModel>),
            typeof(PackagesView),
            new PropertyMetadata(new List<CarStateModel>(), TrackPropertyChanged));

        public List<CarStateModel> Track
        {
            get { return (List<CarStateModel>)GetValue(TrackProperty); }
            set { SetValue(TrackProperty, value); }
        }

        private static void TrackPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            PackagesView control = sender as PackagesView;
            control._track = (List<CarStateModel>)e.NewValue;
            control.DisplayData();
        }

        private void DisplayData()
        {
            lstData.ItemsSource = null;
            if (_track == null)
                return;
            if (_data == null)
                return;
            if (_car == null)
                return;
            if (_track.Count() < 1)
                return;
            List<PackageViewModel> pvm = new List<PackageViewModel>();
            foreach (var item in _data.Rows)
            {
                CarStateModel csm = new CarStateModel();
                List<CarStateModel> temp = _track.Where(p => p.hh == item.DT.hh).ToList();
                if (temp.Count() == 1)
                    csm = temp[0];
                else if (temp.Count() > 1)
                {
                    temp = _track.Where(p => p.hh == item.DT.hh && p.mm == item.DT.mm).ToList();
                    if (temp.Count() == 1)
                        csm = temp[0];
                    else if (temp.Count() > 1)
                    {
                        temp = _track.Where(p => p.hh == item.DT.hh && p.mm == item.DT.mm && p.ss == item.DT.ss).ToList();
                        if (temp.Count() > 0)
                            csm = temp[0];
                    }
                }

                PackageViewModel pm = new PackageViewModel(csm, item, _car.Device.Sensors);
                pvm.Add(pm);
            }
            lstData.ItemsSource = pvm;
        }
    }
}
