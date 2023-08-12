using Microsoft.EntityFrameworkCore;
using OnlineDoctorSystem.Data;
using OnlineDoctorSystem.Data.Models;
using OnlineDoctorSystem.Services.Data.Interfaces;
using OnlineDoctorSystem.Web.ViewModels.Consultations;

namespace OnlineDoctorSystem.Services.Data.Tests
{
	[TestFixture]
	public class ConsultationsServiceTests
	{
		private OnlineDoctorDbContext context;
		private IConsultationsService consultationsService;
		private IDoctorsService doctorsService;
		private IPatientsService patientsService;
		private ITownsService townsService;
		private ISpecialtiesService specialtiesService;

		[SetUp]
		public void Setup()
		{
			var options = new DbContextOptionsBuilder<OnlineDoctorDbContext>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
				.Options;

			context = new OnlineDoctorDbContext(options);

			SeedData();

			townsService = new TownsService(context);
			specialtiesService = new SpecialtiesService(context);
			doctorsService = new DoctorsService(context, townsService, specialtiesService);
			patientsService = new PatientsService(context);
			consultationsService = new ConsultationsService(context, doctorsService, patientsService);
		}

		[TearDown]
		public void TearDown()
		{
			context.Dispose();
		}

		public void SeedData()
		{
			var town = new Town { Name = "Test Town" };
			this.context.Towns.Add(town);

			var specialty = new Specialty { Name = "Test Specialty" };
			this.context.Specialties.Add(specialty);

			var reviews = new List<Review>
			{
				new Review { Rating = 5, Text = "Best Doctor" },
				new Review { Rating = 1, Text = "Worst Doctor" }
			};
			this.context.Reviews.AddRange(reviews);

			var doctor = new Doctor
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
				IsConfirmed = true,
				Latitude = 50,
				Longitude = 30,
				Reviews = reviews,
				DoctorUserId = "doctorId",
			};

			var patient = new Patient
			{
				Id = Guid.Parse("ed5eb77e-5a33-4aa0-a200-25d5ccfa37c2"),
				Name = "Test Patient",
				Phone = "123456789",
				PatientUserId = "patientId"
			};

			var consultation = new Consultation
			{
				Id = Guid.Parse("ed5eb77e-5a33-4aa0-a200-25d5ccfa37c2"),
				DoctorId = doctor.Id,
				PatientId = patient.Id,
				IsActive = true,
				IsConfirmed = true,
				Description = "Boli me glavata",
				Date = new DateTime(2010, 1, 20), // invalid date for update test
				CalendarEvent = new CalendarEvent
				{
					Id = 1,
					Color = "yellow",
					Text = "Boli me glavata"
				}
			};

			doctor.Consultations.Add(consultation);
			patient.Consultations.Add(consultation);

			this.context.Consultations.Add(consultation);
			this.context.Doctors.Add(doctor);
			this.context.Patients.Add(patient);
			this.context.SaveChanges();
		}

		[Test]
		public async Task AddAsync_ValidModel_ReturnsTrue()
		{
			var model = new AddConsultationFormModel
			{
				DoctorId = "ed5eb77e-5a33-4aa0-a200-25d5ccfa37c2",
				PatientId = "ed5eb77e-5a33-4aa0-a200-25d5ccfa37c2",
				Description = "Description",
				DoctorName = "Test Doctor",
				Date = new DateTime(2024, 10, 10),
				StartTime = new TimeSpan(1, 20, 0),
				EndTime = new TimeSpan(1, 30, 0),
			};

			var result = await this.consultationsService.AddAsync(model);

			Assert.IsTrue(result);
		}

		[Test]
		public async Task AddAsync_InvalidModel_ReturnsFalse()
		{
			var model = new AddConsultationFormModel
			{
				DoctorId = "ed5eb77e-5a33-4aa0-a200-25d5ccfa37c2",
				PatientId = "ed5eb77e-5a33-4aa0-a200-25d5ccfa37c2",
				Date = new DateTime(2010, 10, 10),
				StartTime = new TimeSpan(1, 10, 0),
				EndTime = new TimeSpan(2, 50, 0),
			};

			var result = await this.consultationsService.AddAsync(model);

			Assert.IsFalse(result);
		}

		[Test]
		public async Task GetUnconfirmedConsultations_ReturnsUnconfirmedConsultations()
		{
			const string doctorUserId = "doctorId";

			var unconfirmedConsultations = await this.consultationsService.GetUnconfirmedConsultations(doctorUserId);

			Assert.IsNotNull(unconfirmedConsultations);
		}

		[Test]
		public async Task UpdateConsultationsWhenCompleted_PastConsultationsMarkedInactive()
		{
			await this.consultationsService.UpdateConsultationsWhenCompleted();

			var inactiveConsultations = await context.Consultations
				.Where(c => c.IsActive == false)
				.ToListAsync();

			Assert.IsTrue(inactiveConsultations.Any());
		}

		[Test]
		public async Task GetDoctorsConsultationsAsync_ShouldReturnDoctorsConsultations()
		{
			const string doctorId = "ed5eb77e-5a33-4aa0-a200-25d5ccfa37c2";

			var consultations = await this.consultationsService.GetDoctorsConsultationsAsync(doctorId);

			Assert.IsNotNull(consultations);
			Assert.IsTrue(consultations.Any());
		}

		[Test]
		public async Task GetPatientsConsultationsAsync_ShouldReturnPatientsConsultations()
		{
			const string patientId = "ed5eb77e-5a33-4aa0-a200-25d5ccfa37c2";

			var consultations = await this.consultationsService.GetPatientsConsultationsAsync(patientId);

			Assert.IsNotNull(consultations);
			Assert.IsTrue(consultations.Any());
		}

		[Test]
		public async Task GetConsultationByIdAsync_ShouldReturnProperConsultation()
		{
			const string consultationId = "ed5eb77e-5a33-4aa0-a200-25d5ccfa37c2";

			var consultation = await this.consultationsService.GetConsultationByIdAsync(consultationId);

			Assert.IsNotNull(consultation);
		}

		[Test]
		public async Task DeclineConsultation_ShouldDecline()
		{
			const string consultationId = "ed5eb77e-5a33-4aa0-a200-25d5ccfa37c2";

			await this.consultationsService.Decline(consultationId);

			Assert.IsFalse(context.Consultations.First().IsConfirmed);
		}

		[Test]
		public async Task ApproveConsultation_ShouldApprove()
		{
			const string consultationId = "ed5eb77e-5a33-4aa0-a200-25d5ccfa37c2";

			await this.consultationsService.Approve(consultationId);

			Assert.IsTrue(context.Consultations.First().IsConfirmed);
		}
	}
}