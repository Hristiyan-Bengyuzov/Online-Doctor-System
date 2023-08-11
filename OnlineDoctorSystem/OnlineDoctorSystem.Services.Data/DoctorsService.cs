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

		public async Task AddDoctorToDbAsync(string userId, Doctor doctor)
		{
			doctor.DoctorUserId = userId;
			await this.context.Doctors.AddAsync(doctor);
			await this.context.SaveChangesAsync();
		}

		public AllDoctorsFilteredAndPagedServiceModel All(AllDoctorsQueryModel queryModel)
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

			if (queryModel.Latitude != 0)
			{
				UpdateDoctorDistance(queryModel.Latitude, queryModel.Longitude);
				doctorsQuery = doctorsQuery.OrderByDescending(d => d.Distance);
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
				doctor.Distance = CalculateHaversineDistance(latitude, longitude, doctor.Latitude, doctor.Longitude);
			}

			this.context.SaveChanges();
		}


		public async Task<Doctor> GetDoctorByIdAsync(string id) => await this.context.Doctors.FirstAsync(d => d.Id.ToString() == id);

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
			};
		}

		public async Task<string> GetDoctorNameByIdAsync(string id)
		{
			var doctor = await this.GetDoctorByIdAsync(id);
			return doctor.Name;
		}

		private double CalculateHaversineDistance(double startLatitude, double startLongitude, double endLatitude, double endLongitude)
		{
			double EARTH_RADIUS_KM = 6371.0;

			double deltaLatitude = (endLatitude - startLatitude) * Math.PI / 180.0;
			double deltaLongitude = (endLongitude - startLongitude) * Math.PI / 180.0;

			double havHalfDeltaLat = Math.Sin(deltaLatitude / 2.0) * Math.Sin(deltaLatitude / 2.0);
			double havHalfDeltaLon = Math.Sin(deltaLongitude / 2.0) * Math.Sin(deltaLongitude / 2.0);
			double cosStartLat = Math.Cos(startLatitude * Math.PI / 180.0);
			double cosEndLat = Math.Cos(endLatitude * Math.PI / 180.0);

			double a = havHalfDeltaLat + cosStartLat * cosEndLat * havHalfDeltaLon;
			double centralAngle = 2.0 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1.0 - a));

			double distance = EARTH_RADIUS_KM * centralAngle;

			return distance;
		}
	}
}
