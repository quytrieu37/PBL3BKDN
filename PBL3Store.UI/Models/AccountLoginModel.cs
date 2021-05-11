using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PBL3Store.UI.Models
{
    public class AccountLoginModel
    {
        [Display(Name = "Tên đăng nhập")]
        [Required(ErrorMessage = "nhập tên đăng nhập")]
        public string UserName { get; set; }
        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "chưa nhập mật khẩu")]
        public string Password { get; set; }
    }
}