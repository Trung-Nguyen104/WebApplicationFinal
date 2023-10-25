using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Orders
    {
        public int orderId { get; set; }
        public DateTime? orderDate { get; set; }
        public decimal? total { get; set; }
    }
}