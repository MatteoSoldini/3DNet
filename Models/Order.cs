using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPayYouPrint.Models
{
    public class Order
    {
        public int id { get; set; }
        public int buyer_id { get; set; }
        public int material_id { get; set; }
        public bool supplier_confermation { get; set; }
        public bool package_delivered { get; set; }
    }
}
