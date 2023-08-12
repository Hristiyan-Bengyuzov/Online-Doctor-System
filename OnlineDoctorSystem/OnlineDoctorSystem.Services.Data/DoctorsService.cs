using Microsoft.EntityFrameworkCore;
using OnlineDoctorSystem.Data;
using OnlineDoctorSystem.Data.Models;
using OnlineDoctorSystem.Services.Data.Interfaces;
using OnlineDoctorSystem.Services.Data.Models.Doctors;
using OnlineDoctorSystem.Web.Infrastructure.Utilities;
using OnlineDoctorSystem.Web.ViewModels.Doctors;
using OnlineDoctorSystem.Web.ViewModels.Reviews;

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

		public async Task AddDoctorToDbAsync(string userId, Doctor doctor)
		{
			doctor.DoctorUserId = userId;
			await this.context.Doctors.AddAsync(doctor);
			await this.context.SaveChangesAsync();
		}

		public AllDoctorsFilteredAndPagedServiceModel All(AllDoctorsQueryModel queryModel)
		{
			IQueryable<Doctor> doctorsQuery = this.context.Doctors.Where(d => d.IsConfirmed == true).AsQueryable();

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

			if (queryModel.Latitude != 0)
			{
				UpdateDoctorDistance(queryModel.Latitude, queryModel.Longitude);
				doctorsQuery = doctorsQuery.OrderBy(d => d.Distance);
			}

			IEnumerable<AllDoctorsViewModel> allDoctors = doctorsQuery
				.Skip((queryModel.CurrentPage - 1) * queryModel.DoctorsPerPage)
				.Take(queryModel.DoctorsPerPage)
				.Select(d => new AllDoctorsViewModel
				{
					Id = d.Id.ToString(),
					Name = d.Name,
					ImageUrl = d.ImageUrl,
					Specialty = d.Specialty.Name,
					Town = d.Town.Name,
				}).ToList();

			int totalDoctors = doctorsQuery.Count();

			return new AllDoctorsFilteredAndPagedServiceModel
			{
				TotalDoctorsCount = totalDoctors,
				Doctors = allDoctors,
			};
		}

		public void UpdateDoctorDistance(double latitude, double longitude)
		{
			foreach (var doctor in this.context.Doctors.ToList())
			{
				doctor.Distance = Haversine.CalculateHaversineDistance(latitude, longitude, doctor.Latitude, doctor.Longitude);
			}

			this.context.SaveChanges();
		}

		public async Task<Doctor> GetDoctorByIdAsync(string id) => await this.context.Doctors.Include(d => d.Reviews).FirstAsync(d => d.Id.ToString() == id);

		public async Task<Doctor> GetDoctorByUserIdAsync(string id) => await this.context.Doctors.Include(d => d.User).FirstAsync(d => d.DoctorUserId == id);

		public async Task<DoctorDetailsViewModel> GetDoctorDetailsAsync(string id)
		{
			var doctor = await this.GetDoctorByIdAsync(id);

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
				Reviews = doctor.Reviews,
				IsFromThirdParty = doctor.IsFromThirdParty,
				LinkFromThirdParty = doctor.LinkFromThirdParty,
			};
		}

		public async Task<string> GetDoctorNameByIdAsync(string id)
		{
			var doctor = await this.GetDoctorByIdAsync(id);
			return doctor.Name;
		}

		public async Task<IEnumerable<ReviewViewModel>> GetDoctorReviewsAsync(string id)
		{
			var doctor = await this.GetDoctorByIdAsync(id);

			return doctor.Reviews.Select(r => new ReviewViewModel
			{
				Rating = r.Rating,
				Text = r.Text,
			});
		}

		public async Task<IEnumerable<Doctor>> GetUnconfirmedDoctorsAsync()
		{
			var unconfirmedDoctors = await this.context.Doctors
				.Where(d => d.IsConfirmed == null)
				.Include(d => d.Specialty)
				.Include(d => d.Town)
				.ToListAsync();

			return unconfirmedDoctors;
		}


		public async Task ApproveDoctorAsync(string id)
		{
			var doctor = await this.GetDoctorByIdAsync(id);

			doctor.IsConfirmed = true;
			await this.context.SaveChangesAsync();
		}

		public async Task DeclineDoctorAsync(string id)
		{
			var doctor = await this.GetDoctorByIdAsync(id);

			doctor.IsConfirmed = false;
			await this.context.SaveChangesAsync();
		}
	}
}
