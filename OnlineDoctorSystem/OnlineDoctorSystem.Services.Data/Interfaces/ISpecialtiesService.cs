using OnlineDoctorSystem.Data.Models;
using OnlineDoctorSystem.Web.ViewModels.Specialties;

namespace OnlineDoctorSystem.Services.Data.Interfaces
{
    public interface ISpecialtiesService
    {
        Task<IEnumerable<SpecialtyIndexViewModel>> GetAllSpecialties();

        Task<IEnumerable<string>> AllSpecialtyNamesAsync();

        Task<string> GetSpecialtyNameByIdAsync(int id);
    }
}
