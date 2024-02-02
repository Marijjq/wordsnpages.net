using System.Linq.Expressions;
using wordsnpages.Models;
using wordsnpages.Repositories.Interfaces;

namespace wordsnpages.Repositories.Implementations
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db= db;
        }

        public void Update(Product obj)
        {
            _db.Products.Update(obj);
        }

    }
}