using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBL3Store.Domain.Cart
{
    public class Cart
    {
        public List<CartLine> lines { get; private set; }
        public Cart()
        {
            lines = new List<CartLine>();
        }
        
        public void Add(Book book, int quantity)
        {
            CartLine line = lines.FirstOrDefault(x => x.Book.BookId == book.BookId);
            if(line==null)
            {
                line = new CartLine()
                {
                    Book = book,
                    Quantity = quantity
                };
                lines.Add(line);
            }
            else
            {
                line.Quantity += quantity;
            }
            
        }
        public void Update(Book book, int quantity)
        {
            CartLine line = lines.FirstOrDefault(x => x.Book.BookId == book.BookId);
            if(line!=null)
            {
                line.Quantity = quantity;
            }    
        }
        public void Remove(Book book)
        {
            CartLine line = lines.FirstOrDefault(x => x.Book.BookId == book.BookId);
            if(line!= null)
            {
                lines.Remove(line);
            }    
        }
        public decimal Caculator()
        {
            return lines.Sum(x => x.Book.Price * x.Quantity);
        }
        public void Clear()
        {
            lines.Clear();
        }
    }
}
