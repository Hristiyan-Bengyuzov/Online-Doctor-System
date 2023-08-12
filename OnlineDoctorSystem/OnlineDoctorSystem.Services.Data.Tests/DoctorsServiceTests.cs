using Microsoft.EntityFrameworkCore;
using OnlineDoctorSystem.Data;
using OnlineDoctorSystem.Data.Models;
using OnlineDoctorSystem.Services.Data;
using OnlineDoctorSystem.Services.Data.Interfaces;
using OnlineDoctorSystem.Web.ViewModels.Doctors;

namespace OnlineDoctorSystem.Tests.Services.Data
{
	[TestFixture]
	public class DoctorsServiceTests
	{
		private OnlineDoctorDbContext context;
		private IDoctorsService doctorsService;

		[SetUp]
		public void Setup()
		{
			var options = new DbContextOptionsBuilder<OnlineDoctorDbContext>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
				.Options;

			this.context = new OnlineDoctorDbContext(options);

			SeedSampleData();

			var townsService = new TownsService(this.context);
			var specialtiesService = new SpecialtiesService(this.context);

			this.doctorsService = new DoctorsService(this.context, townsService, specialtiesService);
		}

		[TearDown]
		public void TearDown()
		{
			this.context.Dispose();
		}

		[Test]
		public async Task Test_AddDoctorToDbAsync()
		{
			const string userId = "testUserId";
			var doctor = new Doctor
			{
				Name = "Test Doctor2",
				Specialty = new Specialty { Name = "Test Specialty2" },
				Town = new Town { Name = "Test Town2" },
				DoctorUserId = userId,
				Biography = "Biography",
				Education = "Education",
				Qualifications = "Qualifications",
				Phone = "123456789",
				SmallInfo = "Smallinfo",
				Latitude = 40,
				Longitude = 23
			};

			await this.doctorsService.AddDoctorToDbAsync(userId, doctor);
			var addedDoctor = this.context.Doctors.FirstOrDefault(d => d.Name == "Test Doctor2");

			Assert.IsNotNull(addedDoctor);
			Assert.AreEqual(userId, addedDoctor.DoctorUserId);
		}

		[Test]
		public void Test_All_WithFilters()
		{
			var queryModel = new AllDoctorsQueryModel
			{
				Specialty = "Test Specialty",
				Town = "Test Town",
				Name = "Test",
			};

			var result = this.doctorsService.All(queryModel);

			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.TotalDoctorsCount);
		}

		[Test]
		public async Task Test_All_SortByProximity()
		{
			var doctor = new Doctor
			{
				Name = "Test Doctor3",
				Specialty = new Specialty { Name = "Test Specialty3" },
				Town = new Town { Name = "Test Town3" },
				Biography = "Biography",
				Education = "Education",
				Qualifications = "Qualifications",
				Phone = "123456789",
				SmallInfo = "Smallinfo",
				IsConfirmed = true,
				Latitude = 50,
				Longitude = 30
			};
			await this.doctorsService.AddDoctorToDbAsync("userId", doctor);

			var queryModel = new AllDoctorsQueryModel
			{
				Latitude = 50,
				Longitude = 30,
			};

			var result = this.doctorsService.All(queryModel);

			Assert.IsNotNull(result);
			Assert.AreEqual(doctor.Name, result.Doctors.First().Name);
		}


		[Test]
		public async Task Test_GetDoctorDetailsAsync()
		{
			const string doctorId = "ed5eb77e-5a33-4aa0-a200-25d5ccfa37c2";

			var viewModel = await this.doctorsService.GetDoctorDetailsAsync(doctorId);

			Assert.IsNotNull(viewModel);
		}

		[Test]
		public async Task Test_GetDoctorReviewsAsync()
		{
			const string doctorId = "ed5eb77e-5a33-4aa0-a200-25d5ccfa37c2";

			var reviews = await this.doctorsService.GetDoctorReviewsAsync(doctorId);

			Assert.IsNotNull(reviews);
			Assert.AreEqual(2, reviews.Count());
		}

		[Test]
		public async Task Test_GetUnconfirmedDoctorsAsync()
		{
			var unconfirmedDoctors = await this.doctorsService.GetUnconfirmedDoctorsAsync();

			Assert.IsNotNull(unconfirmedDoctors);
		}

		[Test]
		public async Task Test_ApproveDoctorAsync_ShouldApproveDoctor()
		{
			const string userId = "testUserId";
			var doctor = new Doctor
			{
				Name = "Test Doctor2",
				Specialty = new Specialty { Name = "Test Specialty2" },
				Town = new Town { Name = "Test Town2" },
				DoctorUserId = userId,
				Biography = "Biography",
				Education = "Education",
				Qualifications = "Qualifications",
				Phone = "123456789",
				SmallInfo = "Smallinfo",
				Latitude = 40,
				Longitude = 23,
				IsConfirmed = null
			};

			await this.doctorsService.AddDoctorToDbAsync(userId, doctor);
			await this.doctorsService.ApproveDoctorAsync(doctor.Id.ToString());

			Assert.AreEqual(true, doctor.IsConfirmed);
		}

		[Test]
		public async Task Test_GetDoctorNameByIdAsync_ShouldReturnCorrectName()
		{
			const string doctorId = "ed5eb77e-5a33-4aa0-a200-25d5ccfa37c2";

			string name = await this.doctorsService.GetDoctorNameByIdAsync(doctorId);

			Assert.AreEqual("Test Doctor", name);
		}

		[Test]
		public async Task Test_UpdateDoctorDistance_ShouldUpdateDistance()
		{
			const string userId = "testUserId";
			var doctor = new Doctor
			{
				Name = "Test Doctor2",
				Specialty = new Specialty { Name = "Test Specialty2" },
				Town = new Town { Name = "Test Town2" },
				DoctorUserId = userId,
				Biography = "Biography",
				Education = "Education",
				Qualifications = "Qualifications",
				Phone = "123456789",
				SmallInfo = "Smallinfo",
				Latitude = 40,
				Longitude = 23,
				IsConfirmed = null
			};
			await this.doctorsService.AddDoctorToDbAsync(userId, doctor);

			this.doctorsService.UpdateDoctorDistance(40, 23);


			Assert.AreEqual(1144.825573423502, this.context.Doctors.First().Distance);
			Assert.AreEqual(0, doctor.Distance);
		}


		[Test]
		public async Task Test_DeclineDoctorAsync_ShouldDeclineDoctor()
		{
			const string userId = "testUserId";
			var doctor = new Doctor
			{
				Name = "Test Doctor2",
				Specialty = new Specialty { Name = "Test Specialty2" },
				Town = new Town { Name = "Test Town2" },
				DoctorUserId = userId,
				Biography = "Biography",
				Education = "Education",
				Qualifications = "Qualifications",
				Phone = "123456789",
				SmallInfo = "Smallinfo",
				Latitude = 40,
				Longitude = 23,
				IsConfirmed = null
			};

			await this.doctorsService.AddDoctorToDbAsync(userId, doctor);
			await this.doctorsService.DeclineDoctorAsync(doctor.Id.ToString());

			Assert.AreEqual(false, doctor.IsConfirmed);
		}


		private void SeedSampleData()
		{
			var specialty = new Specialty { Name = "Test Specialty" };
			var town = new Town { Name = "Test Town" };

			this.context.Specialties.Add(specialty);
			this.context.Towns.Add(town);
			this.context.Doctors.Add(new Doctor
			{
				Id = Guid.Parse("ed5eb77e-5a33-4aa0-a200-25d5ccfa37c2"),
				Name = "Test Doctor",
				Specialty = specialty,
				Town = town,
				Biography = "Biography",
				Education = "Education",
				Qualifications = "Qualifications",
				Phone = "123456789",
				SmallInfo = "Smallinfo",
				Latitude = 30,
				Longitude = 20,
				IsConfirmed = true,
				Reviews = new List<Review>
				{
					new Review { Rating = 5, Text = "Best Doctor"},
					new Review { Rating = 1, Text = "Worst Doctor"}
				}
			});
			this.context.SaveChanges();
		}
	}
}