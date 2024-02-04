using wordsnpages.Models;

namespace wordsnpages.Repositories.Interfaces
{
    public interface IApplicationUserRepository : IRepository<ApplicationUser>
    {
        public void Update(ApplicationUser applicationUser);

    }
}
