using System.Windows;
using System.Windows.Controls;

namespace DTCDev.Client.Controls.Map
{
    /// <summary>
    /// Displays a pushpin at a geographic location provided by the MapPanel.Location attached property.
    /// </summary>
    public class Pushpin : ContentControl
    {
        static Pushpin()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(Pushpin), new FrameworkPropertyMetadata(typeof(Pushpin)));
        }
    }
}
