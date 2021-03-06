﻿using System;
using System.Windows;
using System.Windows.Controls;
using DTCDev.Client.Cars.Service.Engine.Handlers;

namespace DTCDev.Client.Cars.Service.SideMenu
{
	/// <summary>
	/// Логика взаимодействия для SM_Feedback.xaml
	/// </summary>
	public partial class SM_Feedback : UserControl
	{
		public SM_Feedback()
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
            tbMessage.Text = string.Empty;
            cbTheme.SelectedIndex = -1;
        }

	    private void Send()
	    {
	        CommunicationsHandler.Instance.SendMessage(cbTheme.SelectedIndex, tbMessage.Text);
	    }

        private void cbTheme_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateEnableButton();
        }

        private void tbMessage_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateEnableButton();
        }

	    private void UpdateEnableButton()
	    {
	        btnSend.IsEnabled = !string.IsNullOrEmpty(tbMessage.Text) 
                //&& tbMessage.Text.Length > 10 
                && cbTheme.SelectedIndex != -1;
	    }
	}
}