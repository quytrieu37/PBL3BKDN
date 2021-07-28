using PBL3Store.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBL3Store.UI.Infratructure
{
    public interface IDbQueries
    {
        List<Book> GetAllBook();
        List<Book> GetAllBookDisplay(int page = 1, int pageSize = 12, int categoriId = -1);
        /// <summary>
        /// method get all order base 2 time mile stones
        /// </summary>
        /// <param name="start">time start</param>
        /// <param name="end">time end</param>
        /// <returns></returns>
        List<Order> GetOrderBaseMileStones(DateTime? start = null, DateTime? end = null);

        /// <summary>
        /// Lấy chi tiết sách theo ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Book GetBookDetail(int id);

    }
}