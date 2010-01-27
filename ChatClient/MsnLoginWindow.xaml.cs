﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Threading;

using MSNPSharp;
using MSNPSharp.Core;
using System.Windows.Threading;

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for MsnLoginWindow.xaml
    /// </summary>
    public partial class MsnLoginWindow : Window
    {
        private Messenger m_messenger;
        public MsnLoginWindow(Messenger messenger)
        {
            InitializeComponent();

            m_messenger = messenger;

            m_messenger.Nameserver.SignedIn += new EventHandler<EventArgs>(Nameserver_SignedIn);
            m_messenger.Nameserver.AuthenticationError += new EventHandler<ExceptionEventArgs>(Nameserver_AuthenticationError);
        }

        void Nameserver_AuthenticationError(object sender, ExceptionEventArgs e)
        {
            MessageBox.Show("Authentication failed, check your emailaddress or password.", e.Exception.InnerException.Message);
            btnLogin.Content = "Login";
            btnLogin.IsEnabled = true;
        }

        void Nameserver_SignedIn(object sender, EventArgs e)
        {
            if (!this.Dispatcher.CheckAccess()) {
                this.Dispatcher.Invoke(DispatcherPriority.Normal, new EventHandler<EventArgs>(Nameserver_SignedIn), sender, e);
                return;
            }

            m_messenger.Owner.Status = PresenceStatus.Online;
            this.Close();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (tbEmailaddress.Text.Trim() == "" || tbPassword.Password.Trim() == "")
            {
                MessageBox.Show("Please fill in both your emailaddress and password.");
                return;
            }

            m_messenger.Credentials = new Credentials(tbEmailaddress.Text, tbPassword.Password, MsnProtocol.MSNP18);

            btnLogin.Content = "Connecting...";
            btnLogin.IsEnabled = false;
            m_messenger.Connect();
        }
    }
}
