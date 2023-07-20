using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using OnlineDoctorSystem.Common;
using OnlineDoctorSystem.Data.Models;

namespace OnlineDoctorSystem.Data.Seeders
{
    public class AdminSeeder : ISeeder
    {
        public async Task SeedAsync(OnlineDoctorDbContext context, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            await SeedAdminAsync(userManager, "Admin", "admin@admin.com");
        }

        private async Task SeedAdminAsync(UserManager<ApplicationUser> userManager, string username, string email)
        {
            var user = new ApplicationUser()
            {
                UserName = username,
                Email = email,
                EmailConfirmed = true,
            };

            var result = await userManager.CreateAsync(user, "Admin123");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, GlobalConstants.AdminRole);
            }
        }
    }
}