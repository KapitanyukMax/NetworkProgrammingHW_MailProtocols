using System.Windows;

namespace NetworkProgrammingHW_MailProtocols
{
    public partial class MainWindow : Window
    {
        public MainViewModel MainViewModel = new MainViewModel();

        public MainWindow()
        {
            InitializeComponent();

            DataContext = MainViewModel;
        }
    }
}