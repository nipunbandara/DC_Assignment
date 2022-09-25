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

            string un, pwd, endp = "";

            Console.WriteLine("Register - R, Login - L, Publish - P, UnPublish - U, Exit - E");
            Console.Write("Enter Operation ID: ");
            string action = Console.ReadLine();

            int tok = 0;

            while (action != "E")
            {
                if (action == "R")
                {
                    Console.Write("Enter User name : ");
                    un = Console.ReadLine();
                    Console.Write("Enter Password : ");
                    pwd = Console.ReadLine();
                    Console.WriteLine(foob.Register(un, pwd));
                }
                if(action == "L")
                {
                    Console.Write("Enter User name : ");
                    un = Console.ReadLine();
                    Console.Write("Enter Password : ");
                    pwd = Console.ReadLine();
                    tok = foob.Login(un, pwd);
                    if(tok != 0)
                    {
                        Console.WriteLine("Successfully Logged In");
                    }
                    //Console.WriteLine(tok);
                }
                else if(action == "V")
                {
                    Console.WriteLine(foob.validate(tok));
                }
                else if(action == "P")
                {
                    if(tok != 0)
                    {
                        Registry reg = new Registry();

                        Console.Write("Enter Name : ");
                        reg.Name = Console.ReadLine();
                        Console.Write("Enter Description : ");
                        reg.Description = Console.ReadLine();
                        Console.Write("Enter APIendpoint : ");
                        reg.APIendpoint = Console.ReadLine();
                        Console.Write("Enter numberOfOperands : ");
                        reg.numberOfOperands = Int32.Parse(Console.ReadLine());
                        Console.Write("Enter operandType : ");
                        reg.operandType = Console.ReadLine();

                        /* reg.Name = "ADDThreeNumbers";
                         reg.Description = "Adding three Numbers";
                         reg.APIendpoint = "http://localhost:port/ADDThreeNumbers";
                         reg.numberOfOperands = 3;
                         reg.operandType = "integer";*/

                        RestRequest request = new RestRequest("api/Registry/" + tok.ToString());
                        request.AddJsonBody(reg);
                        RestResponse resp = client.Post(request);

                        Console.WriteLine(resp.Content);
                    }
                    else
                    {
                        Console.WriteLine("Please Login!");
                    }


                }

                else if(action == "U")
                {
                    if(tok != 0)
                    {
                        Console.Write("Enter APIendpoint : ");
                        endp = Console.ReadLine();

                        //string apiendpoint = "ADDThreeNumbers";

                        RestRequest request = new RestRequest("api/Registry/Unpublish/" + tok.ToString() + "/" + endp);
                        RestResponse resp = client.Delete(request);

                        Console.WriteLine(resp.Content);
                    }
                    else
                    {
                        Console.WriteLine("Please Login!");
                    }

                }
                
                Console.WriteLine("Register - R, Login - L, Publish - P, UnPublish - U, Exit - E");
                Console.Write("Enter Operation ID: ");
                action = Console.ReadLine();
            }


        }
    }
}
