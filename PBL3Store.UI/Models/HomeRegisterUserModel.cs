using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PBL3Store.UI.Models
{
    public class HomeRegisterUserModel
    {
        [Required(ErrorMessage ="Vui lòng không xóa mã user")]
        public int UserId { get; set; }
        [Display(Name ="Tên đầy đủ")]
        [Required(ErrorMessage ="Vui lòng điền tên")]
        [StringLength(100, ErrorMessage ="Từ 1 đến 100 kí tự", MinimumLength =1)]
        public string ShipperName { get; set; }
        [Display(Name = "Quê quán")]
        [Required(ErrorMessage = "Vui lòng điền quê quán")]
        [StringLength(500, ErrorMessage = "Từ 10 đến 500 kí tự", MinimumLength = 10)]
        public string HomeTown { get; set; }
        [Display(Name = "Địa chỉ thường trú")]
        [Required(ErrorMessage = "Vui lòng điền địa chỉ thường trú")]
        [StringLength(500, ErrorMessage = "Từ 10 đến 500 kí tự", MinimumLength = 10)]
        public string Address { get; set; }
        [Display(Name = "Số điện thoại")]
        [Required(ErrorMessage = "Vui lòng điền SDT")]
        [StringLength(15, ErrorMessage = "Từ 9 đến 15 kí tự", MinimumLength = 9)]
        public string Phone { get; set; }
        [Display(Name = "Số CMND")]
        [Required(ErrorMessage = "Vui lòng điền số CMND")]
        [StringLength(9, ErrorMessage ="CMND có 9 số", MinimumLength =9)]
        public string CMND { get; set; }
    }
}