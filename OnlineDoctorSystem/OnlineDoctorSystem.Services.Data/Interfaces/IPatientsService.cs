using OnlineDoctorSystem.Data.Models;

namespace OnlineDoctorSystem.Services.Data.Interfaces
{
    public interface IPatientsService
    {
        Task<Patient> GetPatientByIdAsync(string id);
    }
}
