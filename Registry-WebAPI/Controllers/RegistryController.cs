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
        [Route("AllServices/{token}")]
        [HttpGet]
        public IHttpActionResult AllServices(int token)
        {
            if (foob.validate(token) == "validated")
            {
                string path = @"C:\authentication\registries.json";
                StreamReader file = new StreamReader(path);
                var jsonIn = file.ReadToEnd();
                List<Registry> regs = JsonConvert.DeserializeObject<List<Registry>>(jsonIn);
                file.Close();
                return Ok(regs);
            }
            else
            {
                var result = new { Status = "Denied", Reason = "Authentication Error" };
                return Ok(result);
            }
            
        }


        [Route("search/{token}/{name}")]
        [HttpGet]
        public IHttpActionResult Search(int token, string name)
        {
            if (foob.validate(token) == "validated")
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
            else
            {
                var result = new { Status = "Denied", Reason = "Authentication Error" };
                return Ok(result);
            }
        }

        // POST: api/Registry
        [Route("{token}")]
        [HttpPost]
        public IHttpActionResult Post(int token, [FromBody] Registry data)
        {

            if (foob.validate(token) == "validated")
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
                            return Ok("Service is already available in the list");
                        }

                    }

                    regs.Add(data);

                    string jsonOut = JsonConvert.SerializeObject(regs, Formatting.Indented);

                    File.WriteAllText(path, jsonOut.ToString());

                    return Ok("success");
                }
            }
            else
            {
                var result = new { Status = "Denied", Reason = "Authentication Error" };
                return Ok(result);
            }
        }

        /* // DELETE: api/Registry/5
         public void Delete(int id)
         {
         }*/

        [Route("Unpublish/{token}/{apiendpoint}")]
        [HttpDelete]
        public IHttpActionResult Unpublish(int token, String apiendpoint)
        {

            if (foob.validate(token) == "validated")
            {
                string path = @"C:\authentication\registries.json";
                StreamReader file = new StreamReader(path);
                string jsonIn = file.ReadToEnd();
                file.Close();
                int count = 0;
                List<Registry> regs = JsonConvert.DeserializeObject<List<Registry>>(jsonIn);
                List<Registry> srchedReg = new List<Registry>();

                foreach (Registry reg in regs)
                {
                    if (!(reg.APIendpoint.ToLower()).Contains(apiendpoint.ToLower()))
                    {
                        srchedReg.Add(reg);
                    }
                    else
                    {
                        count++;
                    }
                }
                string jsonOut = JsonConvert.SerializeObject(srchedReg, Formatting.Indented);

                File.WriteAllText(path, jsonOut.ToString());
                if(count == 0)
                {
                    return Ok("Service is not Available in List");
                }
                return Ok("success");

            }
            else
            {
                var result = new { Status = "Denied", Reason = "Authentication Error" };
                return Ok(result);
            }
}

    }
}
