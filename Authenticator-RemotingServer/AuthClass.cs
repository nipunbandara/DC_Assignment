using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Authenticator_RemotingServer
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    public class AuthClass : AuthInterface
    {
        public String Register(String name, String Password)
        {

            String namePwd = name + " " + Password;
            
            lock (this)
            {
                string path = @"C:\authentication\accounts.txt";
                if (!File.Exists(path))
                {
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        sw.WriteLine(namePwd);
                        //sw.Close();
                    }

                }
                using (StreamReader file = new StreamReader(path))
                {

                    string ln;

                    while ((ln = file.ReadLine()) != null)
                    {
                        string[] split = ln.Split(' ');

                        if (split[0] == name)
                        {
                            return "User name already Exists";
                        }

                    }
                    file.Close();

                }
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(namePwd);

                }

            }
            
            return "successfully registered";
        }

        public int Login(String name, String Password)
        {
            Random random = new Random();
            int token = 0;

            string path = @"C:\authentication\accounts.txt";
            if (File.Exists(path))
            {
                // Read file using StreamReader. Reads file line by line  
                using (StreamReader file = new StreamReader(path))
                {
                    
                    string ln;

                    while ((ln = file.ReadLine()) != null)
                    {
                        string[] split = ln.Split(' ');

                        if ((split[0] == name) & split[1] == Password)
                        {
                            token = random.Next(111111, 999999);
                        }

                    }
                    file.Close();
                    
                }
            }
            if(token != 0) 
            {
                lock (this)
                {
                    string tpath = @"C:\authentication\tokens.txt";
                    if (!File.Exists(tpath))
                    {
                        using (StreamWriter sw = File.CreateText(tpath))
                        {
                            sw.WriteLine(token);
                            //sw.Close();
                        }

                    }
                    using (StreamWriter sw = File.AppendText(tpath))
                    {
                        sw.WriteLine(token);

                    }

                }
            }
            

            return token;
        }

        public String validate(int token)
        {

            string tpath = @"C:\authentication\tokens.txt";
            if (File.Exists(tpath))
            {
                // Read file using StreamReader. Reads file line by line  
                using (StreamReader file = new StreamReader(tpath))
                {

                    string ln;

                    while ((ln = file.ReadLine()) != null)
                    {
                        //string[] split = ln.Split(' ');

                        if (ln == token.ToString())
                        {
                            return "validated";
                        }

                    }
                    file.Close();

                }
            }
            return "not validated";
        }
           
    }
    
}
