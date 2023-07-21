using OnlineDoctorSystem.Data.Models;

namespace OnlineDoctorSystem.Services.Data.Interfaces
{
    public interface ISpecialtiesService
    {
        Task<IEnumerable<Specialty>> GetAllSpecialties();

        Task<IEnumerable<string>> AllSpecialtyNamesAsync();
    }
}
