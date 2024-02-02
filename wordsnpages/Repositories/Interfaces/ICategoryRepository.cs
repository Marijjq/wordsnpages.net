using wordsnpages.Models;

namespace wordsnpages.Repositories.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        void Update(Category obj);
    }
}