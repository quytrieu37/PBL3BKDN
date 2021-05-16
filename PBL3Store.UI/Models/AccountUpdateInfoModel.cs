using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PBL3Store.UI.Models
{
    public class AccountUpdateInfoModel
    {
        [Required(ErrorMessage ="Please enter the PhoneNumber")]
        [StringLength(15, ErrorMessage ="Number of characters between 9 and 15", MinimumLength =9)]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage ="Please enter your Address")]
        [StringLength(500, ErrorMessage ="Number of characters between 10 and 500", MinimumLength =10)]
        public string Address { get; set; }
    }
}