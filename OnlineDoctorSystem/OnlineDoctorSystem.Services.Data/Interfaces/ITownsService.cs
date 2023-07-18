using OnlineDoctorSystem.Data.Models;

namespace OnlineDoctorSystem.Services.Data.Interfaces
{
    public interface ITownsService
    {
        Task<IEnumerable<Town>> GetAllTowns();
    }
}
