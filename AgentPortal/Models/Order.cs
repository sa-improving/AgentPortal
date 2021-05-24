using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgentPortal.Models
{
    public class Order
    {
        public int OrderNumber { get; set; }
        public double Amount { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustCode { get; set;  }
        public string AgentCode { get; set; }
    }
}
