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

namespace DTCDev.Client
{
    /// <summary>
    /// Interaction logic for MultiSlider.xaml
    /// </summary>
    public partial class MultiSlider : UserControl
    {
        public MultiSlider()
        {
            InitializeComponent();
        }

        public event EventHandler ValueChanged;

        protected virtual void OnValueChanged()
        {
            if (ValueChanged != null) ValueChanged(this, EventArgs.Empty);
        }

        Point _lastPos;
        private bool _leftMove = false;
        private bool _rightMove = false;
        private double _left = 0;
        private double _right = 20;
        private double _min = 0;
        private double _max = 100;


        public decimal Min
        {
            get { return (decimal)_min; }
            set
            {
                _min = (double)value;
                SetValue(MinProperty, value);
                UpdatePos();
            }
        }

        public decimal Max
        {
            get { return (decimal)_max; }
            set
            {
                _max = (double)value;
                SetValue(MaxProperty, value);
                UpdatePos();
            }
        }

        public decimal Left
        {
            get { return (decimal)GetValue(LeftProperty); }
            set { SetValue(LeftProperty, value); }
        }

        public decimal Right
        {
            get { return (decimal)GetValue(RightProperty); }
            set { SetValue(RightProperty, value); }
        }

        private static readonly DependencyProperty MinProperty = DependencyProperty.Register("Min",
             typeof(decimal),
              typeof(MultiSlider),
              new PropertyMetadata(0m));

        private static readonly DependencyProperty MaxProperty = DependencyProperty.Register("Max",
             typeof(decimal),
              typeof(MultiSlider),
              new PropertyMetadata(100m));

        private static readonly DependencyProperty LeftProperty = DependencyProperty.Register("Left",
             typeof(decimal),
              typeof(MultiSlider),
              new PropertyMetadata(OnLeftChanged));

        private static readonly DependencyProperty RightProperty = DependencyProperty.Register("Right",
            typeof(decimal),
            typeof(MultiSlider),
            new PropertyMetadata(OnRightChanged));

        private static readonly DependencyProperty FirstDecorationProperty = DependencyProperty.Register("FirstDecoration",
             typeof(Brush),
              typeof(MultiSlider),
              new PropertyMetadata(null));

        private static readonly DependencyProperty SecondDecorationProperty = DependencyProperty.Register("SecondDecoration",
             typeof(Brush),
              typeof(MultiSlider),
              new PropertyMetadata(new SolidColorBrush(Colors.Blue)));

        private static readonly DependencyProperty ThirdDecorationProperty = DependencyProperty.Register("ThirdDecoration",
             typeof(Brush),
              typeof(MultiSlider),
              new PropertyMetadata(null));

        public Brush FirstDecoration
        {
            get { return (Brush)GetValue(FirstDecorationProperty); }
            set { SetValue(FirstDecorationProperty, value); }
        }

        public Brush SecondDecoration
        {
            get { return (Brush)GetValue(SecondDecorationProperty); }
            set { SetValue(SecondDecorationProperty, value); }
        }

        public Brush ThirdDecoration
        {
            get { return (Brush)GetValue(ThirdDecorationProperty); }
            set { SetValue(ThirdDecorationProperty, value); }
        }

        private static void OnLeftChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var control = (MultiSlider)sender;
            var v = (decimal)e.NewValue;
            control._left = (double)v; ;
            control.UpdatePos();
        }

        private static void OnRightChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var control = (MultiSlider)sender;
            var v = (decimal)e.NewValue;
            control._right = (double)v;
            control.UpdatePos();
        }


        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_leftMove && !_rightMove) return;
            var step = (cnvMove.ActualWidth - 10) / Math.Abs(_max - _min);
            if (_leftMove)
            {
                var p = e.GetPosition(cnvMove);
                if (p.X < 5)
                {
                    Canvas.SetLeft(brdrLeft, 0);
                    _left = 0;
                }
                else if (p.X > cnvMove.ActualWidth - 5)
                {
                    Canvas.SetLeft(brdrLeft, cnvMove.ActualWidth - 5);
                    _left = _max;
                }
                else
                {
                    Canvas.SetLeft(brdrLeft, p.X - 5);
                    _left = _min + p.X / step;
                }
                Left = (decimal)Math.Round(_left, 1);
            }
            if (_rightMove)
            {
                var p = e.GetPosition(cnvMove);
                if (p.X < 5)
                {
                    Canvas.SetLeft(brdrRight, 0);
                    _right = 0;
                }
                else if (p.X > cnvMove.ActualWidth - 5)
                {
                    Canvas.SetLeft(brdrRight, cnvMove.ActualWidth - 5);
                    _right = _max;
                }
                else
                {
                    Canvas.SetLeft(brdrRight, p.X - 5);
                    _right = _min + p.X / step;
                }
                Right = (decimal)Math.Round(_right, 1);
            }
            //UpdateDecorator(step);
            //DisplayValues();
        }


        private void brdrLeft_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            cnvMove.CaptureMouse();
            _leftMove = true;
            _lastPos = e.GetPosition(cnvMove);
            brdrLeft.BorderBrush = new SolidColorBrush(Colors.Blue);
        }

        private void brdrRight_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            cnvMove.CaptureMouse();
            _rightMove = true;
            _lastPos = e.GetPosition(cnvMove);
            brdrRight.BorderBrush = new SolidColorBrush(Colors.Blue);
        }

        private void cnvMove_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            cnvMove.ReleaseMouseCapture();
            _leftMove = false;
            _rightMove = false;
            brdrLeft.BorderBrush = new SolidColorBrush(Colors.DarkGray);
            brdrRight.BorderBrush = new SolidColorBrush(Colors.DarkGray);
            OnValueChanged();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UpdatePos();
        }

        public void UpdatePos(decimal left, decimal right, decimal min, decimal max)
        {
            _max = (double)max;
            _min = (double)min;
            _left = (double)left;
            _right = (double)right;
            UpdatePos();
        }

        public void UpdatePos()
        {
            if (cnvMove.ActualWidth < 1)
                return;
            double step = (cnvMove.ActualWidth - 10) / Math.Abs(_max - _min);
            Canvas.SetLeft(brdrLeft, step * Math.Abs(_left - _min));
            Canvas.SetLeft(brdrRight, Math.Abs(_right - _min) * step);

            UpdateDecorator(step);

            for (int i = cnvMove.Children.Count - 1; i > -1; i--)
            {
                if (cnvMove.Children[i].GetType() == typeof(TextBlock))
                    cnvMove.Children.RemoveAt(i);
            }

            double displayStep = Math.Abs(_max - _min) / 10;
            double displayStepRange = (cnvMove.ActualWidth - 25) / 10;
            for (int i = 0; i < 10; i++)
            {
                TextBlock txt = new TextBlock
                {
                    FontSize = 8,
                    Text = Math.Round(_min + i * displayStep, 1).ToString()
                };
                cnvMove.Children.Add(txt);
                Canvas.SetLeft(txt, displayStepRange * i);

            }

            DisplayValues();
        }

        private void UpdateDecorator(double step)
        {
            var multy = cnvMove.ActualWidth / Math.Abs(_max - _min);
            brdrDecoratorLast.Width = Math.Abs(_max - _right) * multy;
            brdrDecoratorFirst.Width = Math.Abs(_left - _min)* multy;
            brdrDecorator.Width = Math.Abs(_right - _left) * multy;
            Canvas.SetLeft(brdrDecorator, multy * Math.Abs(_left - _min));
            Canvas.SetLeft(brdrDecoratorLast, multy * Math.Abs(_right - _min));
        }

        private void DisplayValues()
        {
            double step = (cnvMove.ActualWidth - 10) / Math.Abs(_max - _min);
            for (int i = cnvMove.Children.Count - 1; i > -1; i--)
            {
                if (cnvMove.Children[i].GetType() == typeof(TextBlock))
                {
                    TextBlock t = (TextBlock)cnvMove.Children[i];
                    if (t.Tag == "vol")
                        cnvMove.Children.RemoveAt(i);
                }
            }

            TextBlock txtLeft = new TextBlock
            {
                FontSize = 8,
                Text = Math.Round(_left, 1).ToString(),
                Tag = "vol"
            };
            cnvMove.Children.Add(txtLeft);
            Canvas.SetLeft(txtLeft, step * Math.Abs(_left - _min));
            Canvas.SetTop(txtLeft, 32);



            TextBlock txtRight = new TextBlock
            {
                FontSize = 8,
                Text = Math.Round(_right, 1).ToString(),
                Tag = "vol"
            };
            cnvMove.Children.Add(txtRight);
            Canvas.SetLeft(txtRight, step * Math.Abs(_right - _min));
            Canvas.SetTop(txtRight, 32);
        }

        private void userControl_LayoutUpdated(object sender, EventArgs e)
        {
            
        }
    }
}
