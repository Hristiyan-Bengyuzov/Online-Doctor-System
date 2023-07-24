using OnlineDoctorSystem.Data.Models;
using OnlineDoctorSystem.Services.Data.Models.Doctors;
using OnlineDoctorSystem.Web.ViewModels.Doctors;

namespace OnlineDoctorSystem.Services.Data.Interfaces
{
    public interface IDoctorsService
    {
        Task<AllDoctorsFilteredAndPagedServiceModel> AllAsync(AllDoctorsQueryModel queryModel);
        Task<DoctorDetailsViewModel> GetDoctorDetailsAsync(string id);
        Task<string> GetDoctorNameByIdAsync(string id);
        Task<Doctor> GetDoctorByIdAsync(string id);
    }
}
