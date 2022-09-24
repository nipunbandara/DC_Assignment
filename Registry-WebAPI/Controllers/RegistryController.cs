using Authenticator_RemotingServer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Registry_WebAPI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.Web.Http;

namespace Registry_WebAPI.Controllers
{
    [RoutePrefix("api/Registry")]
    public class RegistryController : ApiController
    {
        AuthInterface foob;
        public RegistryController()
        {
            ChannelFactory<AuthInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();
            //Set the URL and create the connection!
            string URL = "net.tcp://localhost:8100/AuthenticationService";
            foobFactory = new ChannelFactory<AuthInterface>(tcp, URL);
            foob = foobFactory.CreateChannel();
        }
        // GET: api/Registry
        public IHttpActionResult Get(int token)
        {
            string path = @"C:\authentication\registries.json";
            StreamReader file = new StreamReader(path);
            var jsonIn = file.ReadToEnd();
            List<Registry> regs = JsonConvert.DeserializeObject<List<Registry>>(jsonIn);
            file.Close();
            return Ok(regs);
        }


        [Route("search/{name}")]
        [HttpGet]
        public IHttpActionResult Search(string name)
        {
            string path = @"C:\authentication\registries.json";
            StreamReader file = new StreamReader(path);
            string jsonIn = file.ReadToEnd();
            file.Close();

            List<Registry> regs = JsonConvert.DeserializeObject<List<Registry>>(jsonIn);
            List<Registry> srchedReg = new List<Registry>();

            foreach (Registry reg in regs)
            {
                if ((reg.Name.ToLower()).Contains(name.ToLower()))
                {
                    srchedReg.Add(reg);

                }
            }

            return Ok(srchedReg);
        }

        // POST: api/Registry
        public string Post([FromBody] Registry data)
        {


            lock (this)
            {
                string path = @"C:\authentication\registries.json";
                StreamReader file = new StreamReader(path);
                string jsonIn = file.ReadToEnd();
                file.Close();

                List<Registry> regs = JsonConvert.DeserializeObject<List<Registry>>(jsonIn);

                foreach (Registry reg in regs)
                {
                    if (reg.Name == data.Name)
                    {
                        return "Service is already available in the list";
                    }

                }

                regs.Add(data);

                string jsonOut = JsonConvert.SerializeObject(regs, Formatting.Indented);

                File.WriteAllText(path, jsonOut.ToString());

                return "success";
            }
        }

        /* // DELETE: api/Registry/5
         public void Delete(int id)
         {
         }*/

        [Route("Unpublish/{apiendpoint}")]
        [HttpDelete]
        public IHttpActionResult Unpublish(String apiendpoint)
        {
            string path = @"C:\authentication\registries.json";
            StreamReader file = new StreamReader(path);
            string jsonIn = file.ReadToEnd();
            file.Close();

            List<Registry> regs = JsonConvert.DeserializeObject<List<Registry>>(jsonIn);
            List<Registry> srchedReg = new List<Registry>();

            foreach (Registry reg in regs)
            {
                if (!(reg.APIendpoint.ToLower()).Contains(apiendpoint.ToLower()))
                {
                    srchedReg.Add(reg);
                }
            }
            string jsonOut = JsonConvert.SerializeObject(srchedReg, Formatting.Indented);

            File.WriteAllText(path, jsonOut.ToString());

            return Ok("success");
        }

    }
}
