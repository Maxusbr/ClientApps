using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Шаблон элемента пользовательского элемента управления задокументирован по адресу http://go.microsoft.com/fwlink/?LinkId=234236

namespace KOT.Common.Controls
{
    public sealed partial class ServiceElement : RadioButton
    {
        public ServiceElement()
        {
            this.InitializeComponent();
            Location = new Geopoint(new BasicGeoposition { Altitude = 0, Latitude = 0, Longitude = 0 });
        }

        public ServiceElement(string style, Geopoint point)
        {
            this.InitializeComponent();
            Location = point ?? new Geopoint(new BasicGeoposition { Altitude = 0, Latitude = 0, Longitude = 0 });
            if (string.IsNullOrEmpty(style)) return;
            Style = Application.Current.Resources[style] as Style;
        }

        public Geopoint Location { get; set; }
    }
}
