using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace PBL3Store.UI.Models
{
    public class AccountChangePassModel
    {
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [StringLength(200, ErrorMessage = "mật khẩu quá ngắn", MinimumLength = 3)]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Nhập lại mật khẩu")]
        [Compare("NewPassword", ErrorMessage ="Không khớp")]
        public string ConfirmPassword { get; set; }
    }
}