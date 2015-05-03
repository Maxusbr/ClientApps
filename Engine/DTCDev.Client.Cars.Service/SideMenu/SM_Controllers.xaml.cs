using System;
using System.Windows;
using System.Windows.Controls;
using DTCDev.Client.Cars.Service.Engine.Handlers;

namespace DTCDev.Client.Cars.Service.SideMenu
{
	/// <summary>
	/// Логика взаимодействия для SM_Feedback.xaml
	/// </summary>
	public partial class SM_Controllers : UserControl
	{
        public SM_Controllers()
		{
			this.InitializeComponent();
		}

        public event EventHandler CloseClick;

        private void headerLine_CloseClick(object sender, EventArgs e)
        {
            if (CloseClick != null)
                CloseClick(this, new EventArgs());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Send();
            tbCount.Text = tbMessage.Text = string.Empty;
        }

	    private void Send()
	    {
	        var cnt = 0;
            int.TryParse(tbCount.Text, out cnt);
            if(cnt == 0) return;
            CommunicationsHandler.Instance.SendMessage(cnt, tbMessage.Text);
	    }

	    private void UpdateEnableButton()
        {
            var cnt = 0;
            int.TryParse(tbCount.Text, out cnt);
            btnSend.IsEnabled = cnt > 0;
	    }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateEnableButton();
        }
	}
}