﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV21T1020620.DomainModels
{
    public class Cartdetail
    {
        public int CartDetailID { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int CartID { get; set; }
        public int ProductID { get; set; }
    }
}
