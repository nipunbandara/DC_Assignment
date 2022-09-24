using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Registry_WebAPI.Models
{
    public class Registry
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string APIendpoint { get; set; }
        public int numberOfOperands { get; set; }
        public string operandType { get; set; }
    }
}