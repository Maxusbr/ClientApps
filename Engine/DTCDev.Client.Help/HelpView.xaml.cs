using DTCDev.Client.Help.DataModels;
using DTCDev.Client.Help.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DTCDev.Client.Help
{
    /// <summary>
    /// Interaction logic for HelpView.xaml
    /// </summary>
    public partial class HelpView : UserControl
    {
        MenuWorker worker = new MenuWorker();

        public HelpView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Update();
        }

        public void Update()
        {
            worker.LoadMenu();
            displayedModel = null;
            DisplayRoot();
            _history.Clear();
        }

        HelpMenuModel displayedModel;

        List<HelpMenuModel> _history = new List<HelpMenuModel>();

        private void DisplayRoot()
        {
            stkHelpList.Children.Clear();
            if (displayedModel == null)
            {
                foreach (var item in worker.DOC)
                {
                    TextBlock txt = new TextBlock();
                    txt.Foreground = new SolidColorBrush(Colors.Blue);
                    txt.Cursor = Cursors.Hand;
                    txt.Text = item.Header;
                    txt.Tag = item;
                    txt.MouseLeftButtonUp += txt_MouseLeftButtonUp;
                    stkHelpList.Children.Add(txt);
                    txt.Margin = new Thickness(5);
                }
            }
            else
            {
                foreach (var item in displayedModel.Children)
                {
                    TextBlock txt = new TextBlock();
                    txt.Foreground = new SolidColorBrush(Colors.Blue);
                    txt.Cursor = Cursors.Hand;
                    txt.Text = item.Header;
                    txt.Tag = item;
                    txt.MouseLeftButtonUp += txt_MouseLeftButtonUp;
                    stkHelpList.Children.Add(txt);
                    txt.Margin = new Thickness(5);
                }
            }
            TryLoadContent();
        }

        void txt_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            HelpMenuModel model = (HelpMenuModel)((TextBlock)sender).Tag;
            if (model != null)
                _history.Add(model);
            if (model == null)
                _history.Clear();
            displayedModel = model;
            DisplayRoot();
        }

        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_history.Count() > 0)
            {
                _history.RemoveAt(_history.Count() - 1);
                if (_history.Count() > 0)
                    displayedModel = _history[_history.Count() - 1];
                else
                    displayedModel = null;
            }
            DisplayRoot();
        }



        private void TryLoadContent()
        {
            stkContent.Children.Clear();
            if (displayedModel == null)
                return;
            else
            {
                FileDataModel fdm = new ContentWorker().LoadContent(displayedModel.File);
                foreach (var item in fdm.Elements)
                {
                    if (item.Text != null && item.Text.Length > 0)
                    {
                        TextBlock txt = new TextBlock();
                        txt.Text = item.Text;
                        txt.TextWrapping = TextWrapping.Wrap;
                        txt.Margin = new Thickness(2,10,2,2);
                        stkContent.Children.Add(txt);
                    }
                    if (item.MenuRow != null && item.MenuRow.Length > 0)
                    {
                        TextBlock txt = new TextBlock();
                        txt.Text = item.MenuRow;
                        txt.TextWrapping = TextWrapping.Wrap;
                        txt.Margin = new Thickness(2,5,2,5);
                        txt.FontWeight = FontWeights.Bold;
                        stkContent.Children.Add(txt);
                    }
                    if (item.ImageURL != null && item.ImageURL.Length > 0)
                    {
                        Image img = new Image();
                        img.Source = new ContentWorker().GetImage(item.ImageURL);
                        img.Margin = new Thickness(5);
                        stkContent.Children.Add(img);
                        img.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                        img.Stretch = Stretch.None;
                    }
                }
            }
        }
    }
}
