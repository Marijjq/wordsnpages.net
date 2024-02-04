using System.Linq.Expressions;
using wordsnpages.Models;
using wordsnpages.Repositories.Interfaces;

namespace wordsnpages.Repositories.Implementations
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository 
    {
        private ApplicationDbContext _db;
        public ApplicationUserRepository(ApplicationDbContext db) :base (db)
        {
            _db= db;
        }

        public void Update (ApplicationUser applicationUser)
        {
            _db.ApplicationUsers.Update(applicationUser);
        }

    }
}
