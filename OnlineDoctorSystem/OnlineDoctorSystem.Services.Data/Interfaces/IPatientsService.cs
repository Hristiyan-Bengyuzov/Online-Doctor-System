using OnlineDoctorSystem.Data.Models;

namespace OnlineDoctorSystem.Services.Data.Interfaces
{
    public interface IPatientsService
    {
        Task<Patient> GetPatientByUserIdAsync(string id);
        Task<Patient> GetPatientByIdAsync(string id);
    }
}
