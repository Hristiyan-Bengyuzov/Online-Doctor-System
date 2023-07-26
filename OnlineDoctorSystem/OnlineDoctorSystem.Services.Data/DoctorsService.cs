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
        private readonly ITownsService townsService;
        private readonly ISpecialtiesService specialtiesService;

        public DoctorsService(OnlineDoctorDbContext context, ITownsService townsService, ISpecialtiesService specialtiesService)
        {
            this.context = context;
            this.townsService = townsService;
            this.specialtiesService = specialtiesService;
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

        public async Task<Doctor> GetDoctorByIdAsync(string id) => await this.context.Doctors.FirstAsync(d => d.DoctorUserId == id);

        public async Task<DoctorDetailsViewModel> GetDoctorDetailsAsync(string id)
        {
            var doctor = await this.context.Doctors.FirstAsync(d => d.Id == Guid.Parse(id));

            return new DoctorDetailsViewModel()
            {
                Id = id,
                Name = doctor.Name,
                Specialty = await this.specialtiesService.GetSpecialtyNameByIdAsync(doctor.SpecialtyId),
                Town = await this.townsService.GetTownNameByIdAsync(doctor.TownId),
                ImageUrl = doctor.ImageUrl!,
                SmallInfo = doctor.SmallInfo,
                Education = doctor.Education,
                Qualifications = doctor.Qualifications,
                Biography = doctor.Biography,
            };
        }

        public async Task<string> GetDoctorNameByIdAsync(string id)
        {
            var doctor = await this.context.Doctors.FirstAsync(d => d.Id == Guid.Parse(id));
            return doctor.Name;
        }
    }
}
