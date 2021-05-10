using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PBL3Store.UI.Models
{
    public class AddToCartModel
    {
        [Required]
        [Range(1, int.MaxValue)]    
        public int BookId { get; set; }
        [Required]
        [Range(1, 100, ErrorMessage = "Vui lòng chọn số lượng trong khoảng từ 1-100")]
        public int quantity { get; set; }
    }
}