using ASPNETCoreWebApi.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETCoreWebApi.Model
{
    public class PreSeeder
    {
        public static async Task Seeder(AppDbContext ctx, RoleManager<IdentityRole> roleManager, UserManager<UserModel> userManager)
        {
            ctx.Database.EnsureCreated();

            if (!roleManager.Roles.Any())
            {
                var listOfRoles = new List<IdentityRole>
                {
                    new IdentityRole("Admin"),
                    new IdentityRole("Customer")
                };

                foreach (var role in listOfRoles)
                {
                    await roleManager.CreateAsync(role);
                }
            }

            if (!userManager.Users.Any())
            {
                var listOfUsers = new List<UserModel>
                {
                    new UserModel{ UserName="louis@mail.com", Email = "louis@mail.com", LastName="Otu", FirstName="Louis", Photo = "~/images/avatar.jpg" },
                    new UserModel{ UserName="jess@mail.com", Email = "jess@mail.com", LastName="Jones", FirstName="Jess", Photo = "~/images/avatar.jpg" }
                };

                int counter = 0;
                foreach (var user in listOfUsers)
                {
                    var result = await userManager.CreateAsync(user, "P@$$word1");

                    if (result.Succeeded)
                    {
                        if (counter == 0)
                        {
                            await userManager.AddToRoleAsync(user, "Admin");
                        }
                        else
                        {
                            await userManager.AddToRoleAsync(user, "Customer");
                        }
                    }
                    counter++;
                }
            }
        }
    }
}

