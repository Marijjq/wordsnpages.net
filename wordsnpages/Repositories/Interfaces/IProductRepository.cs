using wordsnpages.Models;

namespace wordsnpages.Repositories.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        void Update(Product obj);
    }
}