using Microsoft.EntityFrameworkCore;
using OnlineDoctorSystem.Data;
using OnlineDoctorSystem.Web.ViewModels.Specialties;

namespace OnlineDoctorSystem.Services.Data.Interfaces
{
    public class SpecialtiesService : ISpecialtiesService
    {
        private readonly OnlineDoctorDbContext context;

        public SpecialtiesService(OnlineDoctorDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<string>> AllSpecialtyNamesAsync() => await this.context.Specialties.Select(s => s.Name).ToListAsync();

        public async Task<string> GetSpecialtyNameByIdAsync(int id)
        {
            var specialty = await this.context.Specialties.FindAsync(id);
            return specialty!.Name;
        }

        public async Task<IEnumerable<SpecialtyIndexViewModel>> GetAllSpecialties()
        {
            var specialties = await this.context.Specialties
                .Select(s => new SpecialtyIndexViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                })
                .ToListAsync();

            return specialties;
        }
    }
}