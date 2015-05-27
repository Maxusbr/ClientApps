using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace KOT.Common.Controls
{
    public partial class CircularProgressBar : UserControl
    {
        /// <summary>
        /// class constructor
        /// </summary>
        public CircularProgressBar() {
            InitializeComponent();
            Value = 0;
        }

        /// <summary>
        /// Set the text color of the value in the center
        /// </summary>
        public Brush FontBrush {
            set {
                lblValue.Foreground = value;
            }
            get { return lblValue.Foreground; }
        }

        /// <summary>
        /// Set the color for the total pie
        /// </summary>
        public Brush BgBrush {
            set {
                bgCircle.Fill = value;
            }
            get { return bgCircle.Fill; }
        }

        /// <summary>
        /// Set the color for the hole
        /// </summary>
        public Brush HoleBrush {
            set {
                hole.Fill = value;
            }
            get { return hole.Fill; }
        }

        /// <summary>
        /// Set the color for the fill for the sector
        /// </summary>
        public Brush FgBrush {
            set {
                path.Fill = value;
                path.Stroke = value;
            }
            get { return path.Fill; }
        }

        /// <summary>
        /// The value for this pie
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(CircularProgressBar), null);
        public double Value {
            get {
                return (double)GetValue(ValueProperty);
            }

            set {
                value = Math.Truncate(value * 100) / 100;
                if (value > 1) value = 1; else if (value < 0) value = 0;                
                SetValue(ValueProperty,value);
                drawSector(value);
                lblValue.Text = "" + (value * 100) + "%";
            }
        }

        /// <summary>
        /// this method draws the sector based on the value
        /// </summary>
        /// <param name="value"></param>
        private void drawSector(double value){
            path.SetValue(Path.DataProperty, null);
            PathGeometry pg = new PathGeometry();
            PathFigure fig = new PathFigure();
            if (value == 0) return;

            double height = this.ActualHeight;              
            double width = this.ActualWidth;
            double radius = height / 2;
            double theta = (360 * value) - 90; 
            double xC = radius;
            double yC = radius;

            if (value == 1) theta += 1;

            double finalPointX = xC + (radius * Math.Cos(theta * 0.0174));      
            double finalPointY = yC + (radius * Math.Sin(theta * 0.0174));     

            fig.StartPoint = new Point(radius, radius);                                         
            LineSegment firstLine = new LineSegment();                                          
            firstLine.Point = new Point(radius, 0);
            fig.Segments.Add(firstLine);                                                        

            if (value > 0.25) {                                                                 
                ArcSegment firstQuart = new ArcSegment();                                       
                firstQuart.Point = new Point(width, radius);                                    
                firstQuart.SweepDirection = SweepDirection.Clockwise;                           
                firstQuart.Size = new Size(radius, radius);                                     
                fig.Segments.Add(firstQuart);                                                   
            }

            if (value > 0.5) {                                                                  
                ArcSegment secondQuart = new ArcSegment();                                      
                secondQuart.Point = new Point(radius, height);                                  
                secondQuart.SweepDirection = SweepDirection.Clockwise;                          
                secondQuart.Size = new Size(radius, radius);                                    
                fig.Segments.Add(secondQuart);                                                  
            }

            if (value > 0.75) {                                                                 
                ArcSegment thirdQuart = new ArcSegment();                                       
                thirdQuart.Point = new Point(0, radius);                                        
                thirdQuart.SweepDirection = SweepDirection.Clockwise;
                thirdQuart.Size = new Size(radius, radius);
                fig.Segments.Add(thirdQuart);                                                   
            }

            ArcSegment finalQuart = new ArcSegment();                                           
            finalQuart.Point = new Point(finalPointX, finalPointY);                             
            finalQuart.SweepDirection = SweepDirection.Clockwise;                               
            finalQuart.Size = new Size(radius, radius);
            fig.Segments.Add(finalQuart);                                                       

            LineSegment lastLine = new LineSegment();
            lastLine.Point = new Point(radius, radius);
            fig.Segments.Add(lastLine);
            pg.Figures.Add(fig);
            path.SetValue(Path.DataProperty, pg);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //Value = 0;
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Height = Width;
            Value = Value;
        }
    }
}
