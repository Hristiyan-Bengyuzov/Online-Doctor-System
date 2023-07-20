using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using OnlineDoctorSystem.Common;

namespace OnlineDoctorSystem.Data.Seeders
{
    public class RolesSeeder : ISeeder
    {
        public async Task SeedAsync(OnlineDoctorDbContext context, IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            await SeedRoleAsync(roleManager, GlobalConstants.AdminRole);
            await SeedRoleAsync(roleManager, GlobalConstants.PatientRole);
            await SeedRoleAsync(roleManager, GlobalConstants.DoctorRole);
        }

        private async Task SeedRoleAsync(RoleManager<IdentityRole> roleManager, string roleName)
        {
            var role = await roleManager.FindByNameAsync(roleName);

            if (role == null)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }
}
