using PBL3Store.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PBL3Store.UI.Models
{
    public class HomeListBookModel
    {
        public List<Book> Books { get; set; }
        public PagingInfo pagingInfo { get; set; }
        public int categoryID { get; set; }
    }
}