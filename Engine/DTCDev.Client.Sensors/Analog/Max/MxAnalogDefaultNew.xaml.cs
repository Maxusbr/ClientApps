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
using DTCDev.Client.Sensors.Analog.Max.Help;
using DTCDev.Models.DeviceSender.DISP;

namespace DTCDev.Client.Sensors.Analog.Max
{
    /// <summary>
    /// Interaction logic for MxAnalogDefaultNew.xaml
    /// </summary>
    public partial class MxAnalogDefaultNew : UserControl
    {
        public MxAnalogDefaultNew()
        {
            InitializeComponent();
        }

        public MxAnalogDefaultNew(DevicePresenter.Sensor pres)
        {
            InitializeComponent();
            AnalogViewModel m = new AnalogViewModel(pres);
            m.StartAnimate += m_StartAnimate;
            this.DataContext = m;
        }

        void m_StartAnimate(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
                {
                    AnalogViewModel avm = sender as AnalogViewModel;
                    //Storyboard sb = this.FindResource("stbMove") as Storyboard;
                    //sb.Begin();
                    DoubleAnimation oLabelAngleAnimation = new DoubleAnimation();

                    oLabelAngleAnimation.From = avm.LastAngle;
                    oLabelAngleAnimation.To = avm.NewAngle;
                    oLabelAngleAnimation.Duration = new Duration(new TimeSpan(0, 0, 0, 0, 500));

                    RotateTransform rt = new RotateTransform();
                    grid.RenderTransform = rt;
                    rt.BeginAnimation(RotateTransform.AngleProperty, oLabelAngleAnimation);
                }));
        }

        private void StartStoryborard()
        {

        }
    }
}
