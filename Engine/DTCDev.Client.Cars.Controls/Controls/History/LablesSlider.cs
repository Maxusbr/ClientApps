using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DTCDev.Client.Cars.Controls.Controls.History
{
    public class LablesSlider : Slider
    {
        protected override void OnRender(DrawingContext dc)
        {
            var size = new Size(ActualWidth-8, ActualHeight);
            var tickCount = (int)((Maximum - Minimum) / TickFrequency) + 1;

            var tickFrequencySize = Math.Round(size.Width * TickFrequency / (Maximum - Minimum), 2);
            for (var i = 0; i < tickCount; i++)
            {
                var text = string.Format("{0}x", (i + 1 )* 5);

                var formattedText = new FormattedText(text, CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight,
                    new Typeface("Verdana"), 8, Brushes.Black);
                dc.DrawText(formattedText, new Point(tickFrequencySize * i, 30));

            }
        }
    }
}
