using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PBL3Store.UI.Models
{
    public class AdminEditBookModel
    {
        [Display(Name ="Mã sách")]
        public int? BookId { get; set; }
        [Display(Name = "Tên sách")]
        [Required(ErrorMessage = "Vui lòng nhập tên sách")]
        public string BookName { get; set; }
        [Display(Name = "Đơn giá")]
        [Required(ErrorMessage = "Vui lòng nhập đơn giá")]
        [Range(0.1, 10000000, ErrorMessage = "Đơn giá trong khoảng từ 0.1 đến 10000000")]
        public decimal Price { get; set; }
        [Display(Name = "Số lượng")]
        [Required(ErrorMessage = "Vui lòng nhập số lượng")]
        [Range(1, 10000, ErrorMessage = "Số lượng từ 1 đến 10000")]
        public int Quantity { get; set; }
        [Display(Name = "Hình ảnh sách")]
        public string Avatar { get; set; }
        [Display(Name = "Doanh mục")]
        [Required(ErrorMessage = "Vui lòng chọn doanh mục")]
        public int CategoryId { get; set; }
        [Display(Name = "Đăng bán công khai")]
        [Required(ErrorMessage = "Chọn trạng thái")]
        public bool State { get; set; }
        public string Author { get; set; }
        [Display(Name = "Mô tả")]
        [AllowHtml]
        public string Description { get; set; }
    }
}