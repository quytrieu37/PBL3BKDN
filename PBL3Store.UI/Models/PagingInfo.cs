using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PBL3Store.UI.Models
{
    public class PagingInfo
    {
        public int TotalItem { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPage
        {
            get => (int)Math.Ceiling(Convert.ToDecimal(TotalItem) / PageSize);
        }
    }
}