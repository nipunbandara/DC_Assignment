using Authenticator_RemotingServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.Web.Http;

namespace ServiceProvider_WebAPI.Controllers
{
    [RoutePrefix("api/service")]
    public class ServiceController : ApiController
    {
        AuthInterface foob;

        public ServiceController()
        {
            ChannelFactory<AuthInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();
            //Set the URL and create the connection!
            string URL = "net.tcp://localhost:8100/AuthenticationService";
            foobFactory = new ChannelFactory<AuthInterface>(tcp, URL);
            foob = foobFactory.CreateChannel();

        }

        

        [Route("add/{token}/{firstNumber}/{secondNumber}")]
        [Route("add")]
        [HttpGet]
        public IHttpActionResult Add(int token, int firstNumber, int secondNumber)
        {
            if (foob.validate(token) == "validated")
            {
                return Ok(firstNumber + secondNumber);
            }
            else
            {
                var result = new { Status = "Denied", Reason = "Authentication Error" };
                return Ok(result);
            }
        }

        [Route("add/{token}/{firstNumber}/{secondNumber}/{thirdNumber}")]
        [Route("add")]
        [HttpGet]
        public IHttpActionResult Add(int token, int firstNumber, int secondNumber, int thirdNumber)
        {
            if (foob.validate(token) == "validated")
            {
                return Ok(firstNumber + secondNumber + thirdNumber);
            }
            else
            {
                var result = new { Status = "Denied", Reason = "Authentication Error" };
                return Ok(result);
            }
        }

        [Route("mul/{token}/{firstNumber}/{secondNumber}")]
        [Route("mul")]
        [HttpGet]
        public IHttpActionResult Mul(int token, int firstNumber, int secondNumber)
        {
            if(foob.validate(token) == "validated")
            {
                return Ok(firstNumber * secondNumber);
            }
            else
            {
                var result = new { Status = "Denied", Reason = "Authentication Error" };
                return Ok(result);
            }
        }


        [Route("mul/{token}/{firstNumber}/{secondNumber}/{thirdNumber}")]
        [Route("mul")]
        [HttpGet]
        public IHttpActionResult Mul(int token, int firstNumber, int secondNumber, int thirdNumber)
        {
            if (foob.validate(token) == "validated")
            {
                return Ok(firstNumber * secondNumber * thirdNumber);
            }
            else
            {
                var result = new { Status = "Denied", Reason = "Authentication Error" };
                return Ok(result);
            }
        }

    }
}
