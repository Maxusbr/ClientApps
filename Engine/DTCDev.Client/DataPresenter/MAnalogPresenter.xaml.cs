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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DTCDev.Client.DataPresenter
{
    /// <summary>
    /// Interaction logic for MAnalogPresenter.xaml
    /// </summary>
    public partial class MAnalogPresenter : UserControl
    {
        public MAnalogPresenter()
        {
            InitializeComponent();
        }

        private int _start = 0;
        private int _stop = 180;

        public int Start
        {
            get { return _start; }
            set
            {
                _start = value;
                UpdateParams();
            }
        }

        public int Stop
        {
            get { return _stop; }
            set
            {
                _stop = value;
                UpdateParams();
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //Storyboard sb = this.FindResource("stbRange") as Storyboard;
            //sb.Begin();
        }

        private void UpdateParams()
        {
            int total = _stop - _start;
            int step = total / 7;
            txt1.Text = _start.ToString();
            txt2.Text = (_start + step).ToString();
            txt3.Text = (_start + step * 2).ToString();
            txt4.Text = (_start + step * 3).ToString();
            txt5.Text = (_start + step * 4).ToString();
            txt6.Text = (_start + step * 5).ToString();
            txt7.Text = (_start + step * 6).ToString();
        }
    }
}
