using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class OrderDetail
    {
        public int buyId { get; set; }
        public string movieName { get; set; }
        public int? quantity { get; set; }
        public decimal? price { get; set; }
    }
}