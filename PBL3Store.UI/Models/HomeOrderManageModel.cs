using PBL3Store.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PBL3Store.UI.Models
{
    public class HomeOrderManageModel
    {
        public List<Order> Orders { get; set; }
        public User customer { get; set; }
    }
}