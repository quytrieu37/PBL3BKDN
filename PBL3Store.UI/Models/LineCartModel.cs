using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PBL3Store.UI.Models
{
    public class LineCartModel
    {
        public int BookId { get; set; }
        [Range(1,1000, ErrorMessage ="Nhập số lượng")]
        public int Quantity { get; set; }
    }
}