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
            await SeedDoctorAsync(userManager, "doctor@doctor.com", context);
        }

        private static async Task SeedDoctorAsync(UserManager<ApplicationUser> userManager, string username, OnlineDoctorDbContext context)
        {
            var user = new ApplicationUser()
            {
                UserName = username,
                Email = username,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, "Doctor123");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, GlobalConstants.DoctorRole);

                var doctor = new Doctor
                {
                    Name = "Харалампи Славков",
                    SpecialtyId = 15,
                    TownId = 1,
                    Phone = "+359 89 554 32 51",
                    BirthDate = new DateTime(1980, 10, 15),
                    DoctorUserId = user.Id,
                    Gender = Gender.Male,
                    YearsOfPractice = 12,
                    ImageUrl = "https://t4.ftcdn.net/jpg/02/60/04/09/360_F_260040900_oO6YW1sHTnKxby4GcjCvtypUCWjnQRg5.jpg",
                    SmallInfo = "Опитен кардиолог",
                    Education = "Софийски университет",
                    Qualifications = "Доста квалифициран",
                    Biography = "Аз съм Харалампи Славков и съм висококвалифициран кардиолог с над десетилетие опит в диагностицирането и лечението на различни сърдечни заболявания.",
                    Latitude = 42,
                    Longitude = 23
                };

                await context.Doctors.AddAsync(doctor);
                await context.SaveChangesAsync();
            }
        }
    }
}