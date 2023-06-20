using MailKit.Net.Smtp;
using PropertyChanged;
using System.Windows.Input;

namespace NetworkProgrammingHW_MailProtocols
{
    [AddINotifyPropertyChangedInterface]
    public class MainViewModel
    {
        public SmtpClient? SmtpClient { get; set; }

        public string LoginStatus { get; set; } = string.Empty;

        public bool IsLoggedIn { get; set; } = false;

        private RelayCommand? loginCommand;

        public ICommand LoginCommand => loginCommand ??
            (loginCommand = new RelayCommand(o => Login(), o => !IsLoggedIn));

        public void Login()
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.ShowDialog();

            if (loginWindow.DataContext is LoginViewModel loginViewModel && loginViewModel.SmtpClient.IsAuthenticated)
            {
                SmtpClient = loginViewModel.SmtpClient;
                LoginStatus = $"You are logged in as {loginViewModel.Address}";
                IsLoggedIn = true;
            }
            else
            {
                SmtpClient = null;
                LoginStatus = string.Empty;
                IsLoggedIn = true;
            }
        }
    }
}