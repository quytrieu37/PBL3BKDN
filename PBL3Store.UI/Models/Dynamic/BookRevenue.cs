using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PBL3Store.UI.Models.Dynamic
{
    public class BookRevenue
    {
        public int BookId { get; set; }
        public string BookName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}