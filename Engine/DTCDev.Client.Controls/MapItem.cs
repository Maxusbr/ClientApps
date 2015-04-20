using System.Windows;
using System.Windows.Controls;

namespace DTCDev.Client.Controls.Map
{
    /// <summary>
    /// Container class for an item in a MapItemsControl.
    /// </summary>
    public class MapItem : ListBoxItem
    {
        static MapItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(MapItem), new FrameworkPropertyMetadata(typeof(MapItem)));
        }
    }
}
