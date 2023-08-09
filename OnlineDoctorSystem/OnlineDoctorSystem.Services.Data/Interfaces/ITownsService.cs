using OnlineDoctorSystem.Data.Models;
using OnlineDoctorSystem.Web.ViewModels.Towns;

namespace OnlineDoctorSystem.Services.Data.Interfaces
{
    public interface ITownsService
    {
        Task<IEnumerable<TownIndexViewModel>> GetAllTowns();

        Task<IEnumerable<string>> AllTownNamesAsync();

        Task<string> GetTownNameByIdAsync(int id);
    }
}
