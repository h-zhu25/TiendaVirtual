using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using TiendaVirtual.Models;

namespace TiendaVirtual
{
    public class IdentitySeeder
    {
        public static void SeedRolesAndAdmin()
        {
            using (var context = new TiendaContext())
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

               
                if (!roleManager.RoleExists("Admin"))
                {
                    roleManager.Create(new IdentityRole("Admin"));
                }

               
                if (!roleManager.RoleExists("User"))
                {
                    roleManager.Create(new IdentityRole("User"));
                }

                
                string adminEmail = "admin@tienda.com";
                string adminPassword = "Admin123!";
                var adminUser = userManager.FindByEmail(adminEmail);

                if (adminUser == null)
                {
                    adminUser = new ApplicationUser
                    {
                        UserName = adminEmail,
                        Email = adminEmail
                    };

                    var result = userManager.Create(adminUser, adminPassword);
                    if (result.Succeeded)
                    {
                        userManager.AddToRole(adminUser.Id, "Admin");
                    }
                }
            }
        }
    }
}
