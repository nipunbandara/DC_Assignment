using Authenticator_RemotingServer;
using Registry_WebAPI.Models;
using RestSharp;
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
            RestClient client;

            ChannelFactory<AuthInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();
            //Set the URL and create the connection!
            string URL = "net.tcp://localhost:8100/AuthenticationService";
            foobFactory = new ChannelFactory<AuthInterface>(tcp, URL);
            foob = foobFactory.CreateChannel();

            string RURL = "http://localhost:61028/";
            client = new RestClient(RURL);
            //RestRequest request = new RestRequest("api/Registry");

            //RestResponse  = client.Get(request);


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
                if(action == "P")
                {
                    tok = foob.Login("test", "psw123");

                    Registry reg = new Registry();
                    reg.Name = "ADDThreeNumbers";
                    reg.Description = "Adding three Numbers";
                    reg.APIendpoint = "http://localhost:port/ADDThreeNumbers";
                    reg.numberOfOperands = 3;
                    reg.operandType = "integer";

                    RestRequest request = new RestRequest("api/Registry/" + tok.ToString());
                    request.AddJsonBody(reg);
                    RestResponse resp = client.Post(request);
                    
                    Console.WriteLine(resp.Content);
                }

                if(action == "U")
                {
                    tok = foob.Login("test", "psw123");
                    string apiendpoint = "ADDThreeNumbers";

                    RestRequest request = new RestRequest("api/Registry/Unpublish/" + tok.ToString() +"/"+ apiendpoint);
                    RestResponse resp = client.Delete(request);

                    Console.WriteLine(resp.Content);
                }
                Console.WriteLine("Register - R, Login - L, Exit - E");

                action = Console.ReadLine();
            }


        }
    }
}
