using Authenticator_RemotingServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServicePublishingConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            AuthInterface foob;
            ChannelFactory<AuthInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();
            //Set the URL and create the connection!
            string URL = "net.tcp://localhost:8100/AuthenticationService";
           
            foobFactory = new ChannelFactory<AuthInterface>(tcp, URL);
            foob = foobFactory.CreateChannel();
           

            Console.WriteLine("Register - R, Login - L, Exit - E");

            string action = Console.ReadLine();

            int tok = 0;

            while (action != "E")
            {
                if (action == "R")
                {
                    Console.WriteLine(foob.Register("test", "psw123"));
                }
                if(action == "L")
                {
                    tok = foob.Login("test", "psw123");
                    Console.WriteLine(tok);
                }
                if(action == "V")
                {
                    Console.WriteLine(foob.validate(tok));
                }
                Console.WriteLine("Register - R, Login - L, Exit - E");

                action = Console.ReadLine();
            }


        }
    }
}
