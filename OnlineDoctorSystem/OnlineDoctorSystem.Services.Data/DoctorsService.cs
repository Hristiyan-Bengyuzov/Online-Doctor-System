using Microsoft.EntityFrameworkCore;
using OnlineDoctorSystem.Data;
using OnlineDoctorSystem.Data.Models;
using OnlineDoctorSystem.Services.Data.Interfaces;
using OnlineDoctorSystem.Services.Data.Models.Doctors;
using OnlineDoctorSystem.Web.ViewModels.Doctors;

namespace OnlineDoctorSystem.Services.Data
{
    public class DoctorsService : IDoctorsService
    {
        private readonly OnlineDoctorDbContext context;

        public DoctorsService(OnlineDoctorDbContext context)
        {
            this.context = context;
        }

        public async Task<AllDoctorsFilteredAndPagedServiceModel> AllAsync(AllDoctorsQueryModel queryModel)
        {
            IQueryable<Doctor> doctorsQuery = this.context.Doctors.AsQueryable();

            if (!string.IsNullOrWhiteSpace(queryModel.Specialty))
            {
                doctorsQuery = doctorsQuery.Where(d => d.Specialty.Name == queryModel.Specialty);
            }

            if (!string.IsNullOrWhiteSpace(queryModel.Town))
            {
                doctorsQuery = doctorsQuery.Where(d => d.Town.Name == queryModel.Town);
            }

            if (!string.IsNullOrEmpty(queryModel.Name))
            {
                string wildCard = $"%{queryModel.Name.ToLower()}%";
                doctorsQuery = doctorsQuery.Where(h => EF.Functions.Like(h.Name, wildCard));
            }

            IEnumerable<AllDoctorsViewModel> allDoctors = await doctorsQuery
                .Skip((queryModel.CurrentPage - 1) * queryModel.DoctorsPerPage)
                .Take(queryModel.DoctorsPerPage)
                .Select(d => new AllDoctorsViewModel
                {
                    Id = d.Id.ToString(),
                    Name = d.Name,
                    ImageUrl = d.ImageUrl,
                    Specialty = d.Specialty.Name,
                    Town = d.Town.Name,
                }).ToListAsync();

            int totalDoctors = doctorsQuery.Count();

            return new AllDoctorsFilteredAndPagedServiceModel
            {
                TotalDoctorsCount = totalDoctors,
                Doctors = allDoctors,
            };
        }
    }
}
