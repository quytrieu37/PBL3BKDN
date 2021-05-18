using PBL3Store.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PBL3Store.UI.Models
{
    public class AdminUserDetailModel
    {
        public User user { get; set; }
        public List<Order> Orders { get; set; }
    }
}