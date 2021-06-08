using System;
using System.Collections.Generic;
<<<<<<< HEAD
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
=======
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

>>>>>>> develop
namespace PBL3Store.UI.Models
{
    public class AccountChangePassModel
    {
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [StringLength(200, ErrorMessage = "mật khẩu quá ngắn", MinimumLength = 3)]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Nhập lại mật khẩu")]
<<<<<<< HEAD
        [Compare("NewPassword", ErrorMessage ="Không khớp")]
=======
        [Compare("NewPassword", ErrorMessage = "Không khớp")]
>>>>>>> develop
        public string ConfirmPassword { get; set; }
    }
}