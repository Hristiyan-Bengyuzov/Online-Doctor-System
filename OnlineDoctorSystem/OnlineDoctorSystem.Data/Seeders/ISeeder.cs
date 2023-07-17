namespace OnlineDoctorSystem.Data.Seeders
{
    public interface ISeeder
    {
        Task SeedAsync(OnlineDoctorDbContext context, IServiceProvider serviceProvider);
    }
}