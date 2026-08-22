using GymSystem.DAL.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Data.DataSeeding
{
    public static class IdentityDataSeeding
    {
        public static async Task SeedIdentityDataAsync(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, ILogger logger, CancellationToken ct = default)
        {
            var hasRoles = roleManager.Roles.Any();
            var hasUsers = userManager.Users.Any();

            if (hasRoles && hasUsers)
            {
                logger.LogInformation("Roles and Users already exist. Skipping seeding.");
                return;
            }

            try
            {

                if (!hasRoles)
                {
                    var roles = new List<IdentityRole>()
                {
                    new IdentityRole("Admin"),
                    new IdentityRole("SuperAdmin")
                };

                    var rolesNames = roles.Select(r => r.Name).ToList();
                    foreach (var role in rolesNames)
                    {
                        if (!await roleManager.RoleExistsAsync(role))
                        {
                            var result = await roleManager.CreateAsync(new IdentityRole(role));
                            if (result.Succeeded)
                            {
                                logger.LogInformation($"Role '{role}' created successfully.");
                            }
                            else
                            {
                                logger.LogError($"Error creating role '{role}': {string.Join(", ", result.Errors.Select(e => e.Description))}");
                            }
                        }
                    }
                }
                if (!hasUsers)
                {
                    var superAdmin = new ApplicationUser()
                    {
                        FirstName = "Muhammed",
                        LastName = "Hamza",
                        Email = "Mu7amza@gmail.com",
                        UserName = "Mu7amza",
                        PhoneNumber = "01234567890",
                    };

                    await userManager.CreateAsync(superAdmin, "P@ssw0rd");
                    await userManager.AddToRoleAsync(superAdmin, "SuperAdmin");
                    logger.LogInformation($"User '{string.Concat(superAdmin.FirstName, "", superAdmin.LastName)}' created successfully.");


                    var admin = new ApplicationUser()
                    {
                        FirstName = "Hajar",
                        LastName = "Hamza",
                        Email = "Hajar@gmail.com",
                        UserName = "Hajar",
                        PhoneNumber = "01234567891"
                    };

                    await userManager.CreateAsync(admin, "P@ssw0rd");
                    logger.LogInformation($"User '{string.Concat(admin.FirstName , "" , admin.LastName)}' created successfully.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"An error occurred while seeding identity data: {ex.Message}");
            }
        }
    }
}
