using Microsoft.EntityFrameworkCore;
using OnlineDoctorSystem.Data;
using OnlineDoctorSystem.Data.Models;
using OnlineDoctorSystem.Services.Data.Interfaces;

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

        public async Task<IEnumerable<Town>> GetAllTowns() => await this.context.Towns.ToListAsync();
    }
}