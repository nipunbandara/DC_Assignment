using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Authenticator_RemotingServer
{
    [ServiceContract]
    public interface AuthInterface
    {

        [OperationContract]
        String Register(String name, String Password);
        [OperationContract]
        int Login(String name, String Password);
        [OperationContract]
        String validate(int token);
        //[OperationContract]
    }
}