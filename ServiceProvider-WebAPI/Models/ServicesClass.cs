using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceProvider_WebAPI.Models
{
    public class ServicesClass
    {
        public int AddTwoNums (int n1, int n2)
        {
            return n1 + n2;
        }
        public int AddThreeNums(int n1, int n2, int n3)
        {
            return n1 + n2 + n3;
        }
    }
}