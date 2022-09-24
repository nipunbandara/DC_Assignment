using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Authenticator_RemotingServer
{
    internal class Server
    {
        private static System.Timers.Timer aTimer;

        static void Main(string[] args)
        {


            //This should *definitely* be more descriptive.
            Console.WriteLine("hey so like welcome to my server");
            //This is the actual host service system
            ServiceHost host;
            //This represents a tcp/ip binding in the Windows network stack
            NetTcpBinding tcp = new NetTcpBinding();
            //Bind server to the implementation of DataServer
            host = new ServiceHost(typeof(AuthClass));
            //Present the publicly accessible interface to the client. 0.0.0.0 tells .net to
            //accept on any interface. :8100 means this will use port 8100. DataService is a name for the
            //actual service, this can be any string.

            host.AddServiceEndpoint(typeof(AuthInterface), tcp, "net.tcp://0.0.0.0:8100/AuthenticationService");
            //And open the host for business!
            Console.Write("Enter Minutes for Clean-Up :");
            int time = Int32.Parse(Console.ReadLine());
            SetTimer(time*1000*60);
            host.Open();
            Console.WriteLine("System Online");
            Console.ReadLine();
            //Don't forget to close the host after you're done!
            host.Close();
            aTimer.Stop();
            aTimer.Dispose();
        }

        private static void SetTimer(int time)
        {
            // Create a timer with a two second interval.
            aTimer = new System.Timers.Timer(time);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            string path = @"C:\authentication\tokens.txt";

            File.WriteAllText(path, String.Empty);
            File.Create(path).Close();
        }
    }
}