using OnlineDoctorSystem.Web.ViewModels.Towns;

namespace OnlineDoctorSystem.Services.Data.Interfaces
{
    public interface IStatisticsService
    {
        Task<int> GetDoctorsCountAsync();
        Task<int> GetPatientsCountAsync();
        Task<int> GetActiveConsultationsCountAsync();
        Task<int> GetReviewsCountAsync();
        Task<int> GetSpecialtiesCountAsync();
        Task<IEnumerable<TownStatisticsViewModel>> GetTownStatisticsAsync();
    }
}
