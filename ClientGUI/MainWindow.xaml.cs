using Authenticator_RemotingServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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

namespace ClientGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        AuthInterface foob;
        int tok = 0;
        public MainWindow()
        {
            
            InitializeComponent();
            ChannelFactory<AuthInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();
            //Set the URL and create the connection!
            string URL = "net.tcp://localhost:8100/AuthenticationService";
            foobFactory = new ChannelFactory<AuthInterface>(tcp, URL);
            foob = foobFactory.CreateChannel();
            
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string un = UNameBox.Text;
            string pwd = PasswordBox.Text;
            tok = foob.Login(un, pwd);
            if(tok != 0)
            {
                MessageBox.Show("Success fully Logged in");
                AllServices alls = new AllServices(tok);
                this.Hide();
                alls.Show();

            }
            else
            {
                MessageBox.Show("Try Again!");

            }

        }

        private void RegisterNowButton_Click(object sender, RoutedEventArgs e)
        {
            Register register = new Register();
            register.Show();
            this.Hide();
        }
    }
}
