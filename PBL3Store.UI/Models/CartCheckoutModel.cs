using PBL3Store.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PBL3Store.UI.Models
{
    public class CartCheckoutModel
    {
        [Required(ErrorMessage ="Vui lòng chọn phương thức thanh toán")]
        public int paymentId { get; set; }
        [StringLength(1000, ErrorMessage ="Too Long")]
        public string Note { get; set; }
    }
}