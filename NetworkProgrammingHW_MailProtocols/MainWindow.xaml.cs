using System.Windows;

namespace NetworkProgrammingHW_MailProtocols
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainViewModel();
        }
    }
}