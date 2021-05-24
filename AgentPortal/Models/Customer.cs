using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgentPortal.Models
{
    public class Customer
    {
        public string CustCode { get; set;  }
        public string CustName { get; set; }
        public string City { get; set; }
        public string WorkingArea { get; set; }
        public string Country { get; set; }
        public int Grade { get; set; }
        public double Money { get; set; }
        public string AgentCode { get; set; }
    }
}
