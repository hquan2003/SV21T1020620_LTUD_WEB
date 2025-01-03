using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV21T1020620.DomainModels
{
    public class Cart
    {
        public int CartID { get; set; }
        public int Sum { get; set; }
        public int CustomerID { get; set; }
    }
}
