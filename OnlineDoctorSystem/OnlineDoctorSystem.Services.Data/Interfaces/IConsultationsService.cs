using OnlineDoctorSystem.Data.Models;
using OnlineDoctorSystem.Web.ViewModels.Consultations;

namespace OnlineDoctorSystem.Services.Data.Interfaces
{
    public interface IConsultationsService
    {
        Task<bool> AddAsync(AddConsultationFormModel model);

        Task<Consultation> GetConsultationByIdAsync(string id);

        Task Decline(string id);

        Task Approve(string id);

        Task<IEnumerable<Consultation>> GetUnconfirmedConsultations(string doctorId);

        Task UpdateConsultationsWhenCompleted();

        Task<IEnumerable<ConsultationViewModel>> GetDoctorsConsultationsAsync(string doctorId);

        Task<IEnumerable<ConsultationViewModel>> GetPatientsConsultationsAsync(string doctorId);
    }
}