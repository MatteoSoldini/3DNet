using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPayYouPrint.Models
{
    public class PrinterMaterial
    {
        public int id { get; set; }
        public int printer_id { get; set; }
        public string name { get; set; }
        public int high_quality_volume_cost { get; set; }
        public int low_quality_volume_cost { get; set; }
    }
}
