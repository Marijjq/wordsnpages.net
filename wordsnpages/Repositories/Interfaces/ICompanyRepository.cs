using wordsnpages.Models;

namespace wordsnpages.Repositories.Interfaces
{
    public interface ICompanyRepository : IRepository<Company>
    {
        void Update(Company companyObj);
    }
}