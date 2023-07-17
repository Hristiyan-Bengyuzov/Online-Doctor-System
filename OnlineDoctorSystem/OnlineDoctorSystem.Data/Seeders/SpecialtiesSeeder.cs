using OnlineDoctorSystem.Data.Models;

namespace OnlineDoctorSystem.Data.Seeders
{
    internal class SpecialtiesSeeder : ISeeder
    {
        public async Task SeedAsync(OnlineDoctorDbContext context, IServiceProvider serviceProvider)
        {
            if (context.Specialties.Any()) return;

            await context.Specialties.AddRangeAsync(new List<Specialty>()
            {   
                new Specialty { Name = "Алерголог" },
                new Specialty { Name = "Ангиолог" },
                new Specialty { Name = "Анестезиолог" },
                new Specialty { Name = "Аюрведа" },
                new Specialty { Name = "Боуен терапевт" },
                new Specialty { Name = "Вирусолог" },
                new Specialty { Name = "Гастроентеролог" },
                new Specialty { Name = "Гръден хирург" },
                new Specialty { Name = "Дерматолог" },
                new Specialty { Name = "Диетолог" },
                new Specialty { Name = "Ендокринолог" },
                new Specialty { Name = "Естетичен дерматолог" },
                new Specialty { Name = "Зъболекар (Стоматолог)" },
                new Specialty { Name = "Имунолог" },
                new Specialty { Name = "Кардиолог" },
                new Specialty { Name = "Кардиохирург" },
                new Specialty { Name = "Кинезитерапевт" },
                new Specialty { Name = "Клинична генетика" },
                new Specialty { Name = "Лицево-челюстен хирург" },
                new Specialty { Name = "Логопед" },
                new Specialty { Name = "Мамолог" },
                new Specialty { Name = "Микробиолог" },
                new Specialty { Name = "Невролог" },
                new Specialty { Name = "Неврохирург" },
                new Specialty { Name = "Неонатолог" },
                new Specialty { Name = "Нефролог" },
                new Specialty { Name = "Общопрактикуващ лекар" },
                new Specialty { Name = "Озонотерапевт" },
                new Specialty { Name = "Онколог" },
                new Specialty { Name = "Орален хирург" },
                new Specialty { Name = "Ортодонт" },
                new Specialty { Name = "Ортопед" },
                new Specialty { Name = "Отоневролог" },
                new Specialty { Name = "Офталмолог (Очен лекар)" },
                new Specialty { Name = "Паразитолог" },
                new Specialty { Name = "Педиатър" },
                new Specialty { Name = "Пластичен хирург" },
                new Specialty { Name = "Профилактични прегледи" },
                new Specialty { Name = "Психиатър" },
                new Specialty { Name = "Психолог" },
                new Specialty { Name = "Психотерапевт" },
                new Specialty { Name = "Пулмолог (Белодробни болести)" },
                new Specialty { Name = "Ревматолог" },
                new Specialty { Name = "Репродуктивна медицина" },
                new Specialty { Name = "Рехабилитатор" },
                new Specialty { Name = "Спортна медицина" },
                new Specialty { Name = "Съдов хирург" },
                new Specialty { Name = "Токсиколог" },
                new Specialty { Name = "УНГ" },
                new Specialty { Name = "Уролог" },
                new Specialty { Name = "Физиотерапевт" },
                new Specialty { Name = "Хематолог (Клинична хематология)" },
                new Specialty { Name = "Хематолог (Трансфузионна хематология)" },
                new Specialty { Name = "Хирург" },
                new Specialty { Name = "Хомеопат" },
            });
        }
    }
}
