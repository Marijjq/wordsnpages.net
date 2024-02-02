using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using wordsnpages.Models;
using wordsnpages.Utilities;

namespace wordsnpages.DbInitializer
{
    //seeding data
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _applicationDbContext;

        public DbInitializer(
            UserManager<IdentityUser> userManager, 
            RoleManager<IdentityRole> roleManager, 
            ApplicationDbContext applicationDbContext)
        {
            _userManager=userManager;
            _roleManager=roleManager;
            _applicationDbContext=applicationDbContext;
        }

        public void Initialize()
        {

            //push migrations if they are not applied
            try
            {
                if (_applicationDbContext.Database.GetPendingMigrations().Count()>0)
                {
                    _applicationDbContext.Database.Migrate();
                }
            }
            catch(Exception ex) { }


            //create roles if they are not created
            if (!_roleManager.RoleExistsAsync(SD.Role_Customer).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Company)).GetAwaiter().GetResult();

                //if roles are not created, we will create admin user as well
                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                    Name="Admin",
                    PhoneNumber = "112233445566",
                    StreetAddress="str. 123 Ave",
                    State="State",
                    PostalCode="1111",
                    City="City"
                }, "Admin123*").GetAwaiter().GetResult();

                ApplicationUser user = _applicationDbContext.ApplicationUsers.FirstOrDefault(u => u.Email == "admin@gmail.com");
                _userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();

            }
            return;
        }
    }
}
