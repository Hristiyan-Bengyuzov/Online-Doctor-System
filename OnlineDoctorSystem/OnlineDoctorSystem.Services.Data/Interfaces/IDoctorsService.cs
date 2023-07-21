using OnlineDoctorSystem.Services.Data.Models.Doctors;
using OnlineDoctorSystem.Web.ViewModels.Doctors;

namespace OnlineDoctorSystem.Services.Data.Interfaces
{
    public interface IDoctorsService
    {
        Task<AllDoctorsFilteredAndPagedServiceModel> AllAsync(AllDoctorsQueryModel queryModel);
    }
}
