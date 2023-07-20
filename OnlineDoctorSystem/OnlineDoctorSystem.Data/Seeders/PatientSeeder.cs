using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using OnlineDoctorSystem.Common;
using OnlineDoctorSystem.Data.Models.Enums;
using OnlineDoctorSystem.Data.Models;

namespace OnlineDoctorSystem.Data.Seeders
{
    public class PatientSeeder : ISeeder
    {
        public async Task SeedAsync(OnlineDoctorDbContext context, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            await SeedPatientAsync(userManager, "Emil Sandev", "patient@patient.com", context);
        }

        private async Task SeedPatientAsync(UserManager<ApplicationUser> userManager, string username, string email, OnlineDoctorDbContext context)
        {
            var user = new ApplicationUser()
            {
                UserName = username,
                Email = email,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, "Patient123");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, GlobalConstants.PatientRole);

                var patient = new Patient
                {
                    Name = user.UserName,
                    Phone = "+359 89 152 82 95",
                    BirthDate = new DateTime(2005, 7, 12),
                    TownId = 1,
                    UserId = Guid.Parse(user.Id),
                    Gender = Gender.Female,
                };

                await context.Patients.AddAsync(patient);
                await context.SaveChangesAsync();
            }
        }
    }
}
