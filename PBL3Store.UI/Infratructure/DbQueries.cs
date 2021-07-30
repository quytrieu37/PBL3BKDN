using Dapper;
using PBL3Store.Domain;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace PBL3Store.UI.Infratructure
{
    public class DbQueries : IDbQueries
    {
        private string _connectionString;
        public DbQueries()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["dapperCnn"].ToString();
        }
        public List<Book> GetAllBook()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "select * from Books order by Books.BookId desc ";
                IEnumerable<Book> result = connection.Query<Book>(query);
                return result.ToList();
            }
        }

        public List<Book> GetAllBookDisplay(int page = 1, int pageSize = 12, int categoriId = -1)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("select * From Books as b where b.State = 'true' and( b.CategoryId = @categoriId OR @categoriId = -1) ");
                sb.Append("order by b.BookId DESC ");
                sb.Append("OFFSET (@page-1)*@pageSize ROWS ");
                sb.Append("FETCH FIRST @pageSize ROWS ONLY ");
                string query = sb.ToString();
                IEnumerable<Book> result = connection.Query<Book>(query, new { categoriId = categoriId, page = page, pageSize = pageSize });
                return result.ToList();
            }
        }

        public Book GetBookDetail(int bookId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("select * from Books where Books.BookId = @bookId ");
                //sb.Append("where Books.BookId = @bookId ");
                
                string query = sb.ToString();
                IEnumerable<Book> result = connection.Query<Book>(query, new { bookId = bookId });
                return result.FirstOrDefault();
            }
        }

        public List<Order> GetOrderBaseMileStones(DateTime? start = null, DateTime? end = null)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                StringBuilder sb = new StringBuilder();
                if (start == null)
                {
                    start = new DateTime(1754, 1, 1);
                    end = new DateTime(1754, 1, 1);
                }
                TimeSpan tSpan = new TimeSpan(1, 0, 0, 0);
                end += tSpan;
                string st = start.Value.ToString("yyyy-MM-dd");
                string e = end.Value.ToString("yyyy-MM-dd");

                sb.Append("select * from Orders as o where ");
                sb.Append(" @st1 = '1754-1-1' ");
                sb.Append("or ( o.CreateDate <= @e ");
                sb.Append("and o.CreateDate >= @st ) ");
                string query = sb.ToString();
                IEnumerable<Order> orders = connection.Query<Order>(query, new { st1 = st, st = st, e = e });
                return orders.ToList();
            }
        }

        public List<OrderDetail> GetViewOrder(int orderId)
        {
            using(SqlConnection connection = new SqlConnection(_connectionString))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Select * From OrderDetails AS od Where od.OrderId = @orderId ");
                string query = sb.ToString();
                IEnumerable<OrderDetail> result = connection.Query<OrderDetail>(query, new { orderId = orderId });
                return result.ToList();
            }
        }
    }
}