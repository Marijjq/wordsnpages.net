using System.Linq.Expressions;
using wordsnpages.Models;
using wordsnpages.Repositories.Interfaces;

namespace wordsnpages.Repositories.Implementations
{
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository 
    {
        private ApplicationDbContext _db;
        public ShoppingCartRepository(ApplicationDbContext db) :base (db)
        {
            _db= db;
        }

        public void Update(ShoppingCart obj)
        {
            _db.ShoppingCarts.Update(obj);
        }
    }
}