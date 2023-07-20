using Microsoft.AspNetCore.Identity;
using OnlineDoctorSystem.Common;
using OnlineDoctorSystem.Data.Models.Enums;
using OnlineDoctorSystem.Data.Models;
using Microsoft.Extensions.DependencyInjection;

namespace OnlineDoctorSystem.Data.Seeders
{
    public class DoctorSeeder : ISeeder
    {
        public async Task SeedAsync(OnlineDoctorDbContext context, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            await SeedDoctorAsync(userManager, "Харалампи Славков", "doctor@doctor.com", context);
        }

        private static async Task SeedDoctorAsync(UserManager<ApplicationUser> userManager, string username, string email, OnlineDoctorDbContext context)
        {
            var user = new ApplicationUser()
            {
                UserName = username,
                Email = email,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, "Doctor123");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, GlobalConstants.DoctorRole);

                var doctor = new Doctor
                {
                    Name = user.UserName,
                    SpecialtyId = 15,
                    TownId = 1,
                    Phone = "+359 89 554 32 51",
                    BirthDate = new DateTime(1980, 10, 15),
                    UserId = Guid.Parse(user.Id),
                    Gender = Gender.Male,
                    YearsOfPractice = 12,
                    SmallInfo = "Опитен кардиолог",
                    Education = "Софийски университет",
                    Qualifications = "Доста квалифициран",
                    Biography = "Аз съм Харалампи Славков и съм висококвалифициран кардиолог с над десетилетие опит в диагностицирането и лечението на различни сърдечни заболявания.",
                };

                await context.Doctors.AddAsync(doctor);
                await context.SaveChangesAsync();
            }
        }
    }
}