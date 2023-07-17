namespace OnlineDoctorSystem.Data.Seeders
{
    public class OnlineDoctorDbContextSeeder : ISeeder
    {
        public async Task SeedAsync(OnlineDoctorDbContext context, IServiceProvider serviceProvider)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            var seeders = new List<ISeeder>
            {
                new TownsSeeder(),
                new SpecialtiesSeeder(),
            };

            foreach (var seeder in seeders)
            {
                await seeder.SeedAsync(context, serviceProvider);
                await context.SaveChangesAsync();
            }
        }
    }
}
