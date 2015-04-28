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
using DTCDev.Client.Cars.Service.Engine.Controls.ViewModels.Slides;

namespace DTCDev.Client.Cars.Service.Slides
{
    /// <summary>
    /// Interaction logic for SlideCarsList.xaml
    /// </summary>
    public partial class SlideCarsList : UserControl
    {
        SlideCarsListViewModel _vm = new SlideCarsListViewModel();
        public SlideCarsList()
        {
            InitializeComponent();
            this.DataContext = _vm;
        }

        public SlideCarsListViewModel VM
        {
            get { return _vm; }
        }
    }
}
