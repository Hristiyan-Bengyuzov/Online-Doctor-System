using OnlineDoctorSystem.Web.ViewModels.Consultations;

namespace OnlineDoctorSystem.Services.Data.Interfaces
{
    public interface IConsultationsService
    {
        Task<bool> AddAsync(AddConsultationFormModel model);
    }
}
