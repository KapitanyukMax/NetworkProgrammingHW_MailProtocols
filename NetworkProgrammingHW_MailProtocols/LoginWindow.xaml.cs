﻿using System.Windows;
using System.Windows.Controls;

namespace NetworkProgrammingHW_MailProtocols
{
    public partial class LoginWindow : Window
    {
        public LoginViewModel LoginViewModel = new LoginViewModel();

        public LoginWindow()
        {
            InitializeComponent();

            DataContext = LoginViewModel;
            LoginViewModel.RequestClose += (s, e) =>
                Application.Current.Dispatcher.Invoke(Close);
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            LoginViewModel.Password = (sender as PasswordBox)?.Password ?? string.Empty;
        }
    }
}