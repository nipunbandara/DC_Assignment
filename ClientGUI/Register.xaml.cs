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
using System.Windows.Shapes;

namespace ClientGUI
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        AuthInterface foob;
        public Register()
        {
            InitializeComponent();
            ChannelFactory<AuthInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();
            //Set the URL and create the connection!
            string URL = "net.tcp://localhost:8100/AuthenticationService";
            foobFactory = new ChannelFactory<AuthInterface>(tcp, URL);
            foob = foobFactory.CreateChannel();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string un = UNameBox.Text;
            string pwd = PasswordBox.Text;
            string res = foob.Register(un, pwd);
            MessageBox.Show(res);
            if(res == "successfully registered")
            {
                MainWindow mw = new MainWindow();
                mw.Show();
                this.Hide();
            }

        }

        private void LoginNowButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw  = new MainWindow();
            mw.Show();
            this.Hide();
        }
    }
}
