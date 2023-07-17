using OnlineDoctorSystem.Data.Models;

namespace OnlineDoctorSystem.Data.Seeders
{
    public class TownsSeeder : ISeeder
    {
        public async Task SeedAsync(OnlineDoctorDbContext context, IServiceProvider serviceProvider)
        {
            if (context.Towns.Any()) return;

            await context.Towns.AddRangeAsync(new List<Town>()
            {
                new Town { Name = "Благоевград" },
                new Town { Name = "Бургас" },
                new Town { Name = "Варна" },
                new Town { Name = "Велико Търново" },
                new Town { Name = "Враца" },
                new Town { Name = "Габрово" },
                new Town { Name = "Добрич" },
                new Town { Name = "Кърджали" },
                new Town { Name = "Кюстендил" },
                new Town { Name = "Монтана" },
                new Town { Name = "Пазарджик" },
                new Town { Name = "Перник" },
                new Town { Name = "Плевен" },
                new Town { Name = "Пловдив" },
                new Town { Name = "Разград" },
                new Town { Name = "Русе" },
                new Town { Name = "Силистра" },
                new Town { Name = "Сливен" },
                new Town { Name = "Смолян" },
                new Town { Name = "София" },
                new Town { Name = "Стара Загора" },
                new Town { Name = "Търговище" },
                new Town { Name = "Хасково" },
                new Town { Name = "Шумен" },
                new Town { Name = "Ямбол" }
            });
        }
    }
}