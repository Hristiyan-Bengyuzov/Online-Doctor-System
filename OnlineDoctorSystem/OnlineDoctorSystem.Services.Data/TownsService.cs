using Microsoft.EntityFrameworkCore;
using OnlineDoctorSystem.Data;
using OnlineDoctorSystem.Services.Data.Interfaces;
using OnlineDoctorSystem.Web.ViewModels.Towns;

namespace OnlineDoctorSystem.Services.Data
{
    public class TownsService : ITownsService
    {
        private readonly OnlineDoctorDbContext context;

        public TownsService(OnlineDoctorDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<string>> AllTownNamesAsync() => await this.context.Towns.Select(t => t.Name).ToListAsync();

        public async Task<string> GetTownNameByIdAsync(int id)
        {
            var town = await this.context.Towns.FindAsync(id);
            return town!.Name;
        }

        public async Task<IEnumerable<TownIndexViewModel>> GetAllTowns()
        {
            var towns = await this.context.Towns
               .Select(t => new TownIndexViewModel
               {
                   Id = t.Id,
                   Name = t.Name,
               }).ToListAsync();

            return towns;
        }
    }
}