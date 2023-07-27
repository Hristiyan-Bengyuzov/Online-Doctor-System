using Microsoft.EntityFrameworkCore;
using OnlineDoctorSystem.Data;
using OnlineDoctorSystem.Services.Data.Interfaces;
using OnlineDoctorSystem.Web.ViewModels.Towns;

namespace OnlineDoctorSystem.Services.Data
{
    public class StatisticsService : IStatisticsService
    {
        private readonly OnlineDoctorDbContext context;

        public StatisticsService(OnlineDoctorDbContext context)
        {
            this.context = context;
        }

        public async Task<int> GetActiveConsultationsCountAsync() => await this.context.Consultations.CountAsync(c => c.IsActive);

        public async Task<int> GetDoctorsCountAsync() => await this.context.Doctors.CountAsync();

        public async Task<int> GetSpecialtiesCountAsync() => await this.context.Specialties.CountAsync();

        public async Task<int> GetPatientsCountAsync() => await this.context.Patients.CountAsync();

        public async Task<int> GetReviewsCountAsync() => await this.context.Reviews.CountAsync();

        public async Task<IEnumerable<TownStatisticsViewModel>> GetTownStatisticsAsync()
        {
            var statistics = await this.context.Towns
                 .Select(t => new TownStatisticsViewModel
                 {
                     TownName = t.Name,
                     DoctorsCount = context.Doctors.Count(d => d.TownId == t.Id)
                 })
                 .OrderByDescending(t => t.DoctorsCount)
                 .ThenBy(t => t.TownName)
                 .ToListAsync();

            return statistics;
        }
    }
}
