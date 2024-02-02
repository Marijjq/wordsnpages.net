using System.Linq.Expressions;
using wordsnpages.Models;
using wordsnpages.Repositories.Interfaces;

namespace wordsnpages.Repositories.Implementations
{
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository 
    {
        private ApplicationDbContext _db;
        public OrderDetailRepository(ApplicationDbContext db) :base (db)
        {
            _db= db;
        }

        public void Update(OrderDetail obj)
        {
            _db.OrderDetails.Update(obj);
        }
    }
}
