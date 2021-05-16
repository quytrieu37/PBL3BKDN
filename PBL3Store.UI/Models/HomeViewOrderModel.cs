using PBL3Store.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PBL3Store.UI.Models
{
    public class HomeViewOrderModel
    {
        public List<OrderDetail> orderDetails { get; set; }
    }
}