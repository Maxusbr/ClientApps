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

namespace DTCDev.Client.Cars.Service.Controls
{
    /// <summary>
    /// Interaction logic for PrintControl.xaml
    /// </summary>
    public partial class PrintControl : UserControl
    {
        public PrintControl()
        {
            InitializeComponent();
        }

        UserControl _control;

        public void AddPrinted(UserControl control)
        {
            _control = control;
            grdDisplay.Children.Add(control);
        }

        public event EventHandler CloseClick;

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog pd = new PrintDialog();
            if (pd.ShowDialog() == true)
            {
                _control.Width = pd.PrintableAreaWidth - 20;
                pd.PrintDocument(((IDocumentPaginatorSource)flowDocument).DocumentPaginator, "Отчет");
            }
        }
    }
}
