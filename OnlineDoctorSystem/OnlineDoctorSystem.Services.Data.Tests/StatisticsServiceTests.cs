using Microsoft.EntityFrameworkCore;
using OnlineDoctorSystem.Data;
using OnlineDoctorSystem.Data.Models;
using OnlineDoctorSystem.Services.Data;

namespace OnlineDoctorSystem.Tests.Services.Data
{
	[TestFixture]
	public class StatisticsServiceTests
	{
		private OnlineDoctorDbContext context;
		private StatisticsService statisticsService;

		[SetUp]
		public void Setup()
		{
			var options = new DbContextOptionsBuilder<OnlineDoctorDbContext>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
				.Options;

			this.context = new OnlineDoctorDbContext(options);


			var towns = new List<Town>
			{
				new Town { Id = 1, Name = "Town1" },
				new Town { Id = 2, Name = "Town2" },
				new Town { Id = 3, Name = "Town3" }
			};
			this.context.Towns.AddRange(towns);

			var doctors = new List<Doctor>
			{
				 new Doctor {  Name = "Doctor1", TownId = 1, Biography = "Biography1", Education = "Education1", Phone = "123456789", Qualifications = "Qualifications1", SmallInfo = "SmallInfo1" },
				 new Doctor {  Name = "Doctor2", TownId = 2, Biography = "Biography2", Education = "Education2", Phone = "987654321", Qualifications = "Qualifications2", SmallInfo = "SmallInfo2" },
				 new Doctor {  Name = "Doctor3", TownId = 2, Biography = "Biography3", Education = "Education3", Phone = "555555555", Qualifications = "Qualifications3", SmallInfo = "SmallInfo3" },
				 new Doctor {  Name = "Doctor4", TownId = 3, Biography = "Biography4", Education = "Education4", Phone = "444444444", Qualifications = "Qualifications4", SmallInfo = "SmallInfo4" }
			};
			this.context.Doctors.AddRange(doctors);

			var consultations = new List<Consultation>
			{
				new Consultation { IsActive = true,  Description = "Description1"},
				new Consultation { IsActive = false, Description = "Description2"},
				new Consultation { IsActive = true , Description = "Description3"}
			};
			this.context.Consultations.AddRange(consultations);

			var specialties = new List<Specialty>
			{
				new Specialty { Id = 1, Name = "Specialty1" },
				new Specialty { Id = 2, Name = "Specialty2" }
			};
			this.context.Specialties.AddRange(specialties);

			var patients = new List<Patient>
			{
				new Patient { Name = "Patient1",  PatientUserId = Guid.NewGuid().ToString(), Phone = "123456789"},
				new Patient { Name = "Patient2" , PatientUserId = Guid.NewGuid().ToString(), Phone = "444444444"}
			};
			this.context.Patients.AddRange(patients);

			var reviews = new List<Review>
			{
				new Review { Rating = 5, Text = "Amazing" },
				new Review { Rating = 4, Text = "Good" },
				new Review { Rating = 3, Text = "Meh" }
			};
			this.context.Reviews.AddRange(reviews);

			this.context.SaveChanges();

			this.statisticsService = new StatisticsService(this.context);
		}

		[TearDown]
		public void TearDown()
		{
			this.context.Dispose();
		}

		[Test]
		public async Task GetActiveConsultationsCountAsync_ShouldReturnCorrectCount()
		{
			var count = await this.statisticsService.GetActiveConsultationsCountAsync();

			Assert.AreEqual(2, count);
		}

		[Test]
		public async Task GetDoctorsCountAsync_ShouldReturnCorrectCount()
		{
			var count = await this.statisticsService.GetDoctorsCountAsync();

			Assert.AreEqual(4, count);
		}

		[Test]
		public async Task GetSpecialtiesCountAsync_ShouldReturnCorrectCount()
		{
			var count = await this.statisticsService.GetSpecialtiesCountAsync();

			Assert.AreEqual(2, count);
		}

		[Test]
		public async Task GetPatientsCountAsync_ShouldReturnCorrectCount()
		{
			var count = await this.statisticsService.GetPatientsCountAsync();

			Assert.AreEqual(2, count);
		}

		[Test]
		public async Task GetReviewsCountAsync_ShouldReturnCorrectCount()
		{
			var count = await this.statisticsService.GetReviewsCountAsync();

			Assert.AreEqual(3, count);
		}

		[Test]
		public async Task GetTownStatisticsAsync_ShouldReturnCorrectStatistics()
		{
			var statistics = await this.statisticsService.GetTownStatisticsAsync();

			Assert.AreEqual(3, statistics.Count());

			var firstTown = statistics.First();
			Assert.AreEqual("Town2", firstTown.TownName);
			Assert.AreEqual(2, firstTown.DoctorsCount);

			var lastTown = statistics.Last();
			Assert.AreEqual("Town3", lastTown.TownName);
			Assert.AreEqual(1, lastTown.DoctorsCount);
		}
	}
}