using System.Windows;

namespace NetworkProgrammingHW_MailProtocols
{
    public partial class LoginWindow : Window
    {
        public LoginViewModel LoginViewModel = new LoginViewModel();

        public LoginWindow()
        {
            InitializeComponent();

            DataContext = LoginViewModel;
        }
    }
}