using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPayYouPrint.Models
{
    public class Printer
    {
        public int id { get; set; }
        public int owners_id { get; set; }
        public string name { get; set; }
        public int shipping_cost { get; set; }
    }
}
