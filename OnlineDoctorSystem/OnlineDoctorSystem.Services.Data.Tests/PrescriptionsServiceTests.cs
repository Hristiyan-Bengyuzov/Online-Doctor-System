using Microsoft.EntityFrameworkCore;
using OnlineDoctorSystem.Data;
using OnlineDoctorSystem.Data.Models;
using OnlineDoctorSystem.Services.Data;
using OnlineDoctorSystem.Services.Data.Interfaces;
using OnlineDoctorSystem.Web.ViewModels.Prescriptions;

namespace OnlineDoctorSystem.Tests.Services
{
	[TestFixture]
	public class PrescriptionsServiceTests
	{
		private OnlineDoctorDbContext context;
		private IPrescriptionsService prescriptionsService;

		[SetUp]
		public void Setup()
		{
			var options = new DbContextOptionsBuilder<OnlineDoctorDbContext>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
				.Options;

			context = new OnlineDoctorDbContext(options);
			prescriptionsService = new PrescriptionsService(context);
		}

		[TearDown]
		public void TearDown()
		{
			context.Dispose();
		}

		[Test]
		public async Task AddPrescriptionAsync_ShouldAddPrescriptionToDatabase()
		{
			var doctor = new Doctor
			{
				Name = "Test Doctor",
				Biography = "Experienced doctor",
				Education = "Medical School",
				Phone = "123-456-7890",
				Qualifications = "MD",
				SmallInfo = "Specializes in General Medicine",
			};

			var patient = new Patient
			{
				Name = "Test Patient",
				PatientUserId = Guid.NewGuid().ToString(),
				Phone = "123456789"
			};

			var model = new AddPrescriptionFormModel
			{
				Doctor = doctor,
				DoctorId = doctor.Id.ToString(),
				Patient = patient,
				PatientId = patient.Id.ToString(),
				MedicamentName = "Medicine",
				Instructions = "Take once daily",
			};

			await prescriptionsService.AddPrescriptionAsync(model);

			var prescriptionsCount = await context.Prescriptions.CountAsync();
			Assert.AreEqual(1, prescriptionsCount);
		}

		[Test]
		public void GetPatientsPrescriptions_ShouldReturnCorrectPrescriptions()
		{
			var doctor = new Doctor
			{
				Name = "Test Doctor",
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

			var user = new ApplicationUser { UserName = Guid.NewGuid().ToString() };


			var patient = new Patient
			{
				Name = "Test Patient",
				PatientUserId = user.Id,
				User = user,
				Phone = "123456789"
			};

			var prescription = new Prescription
			{
				Doctor = doctor,
				DoctorId = doctor.Id,
				Patient = patient,
				PatientId = patient.Id,
				MedicamentName = "Medicine",
				Instructions = "Take once daily",
			};

			context.Doctors.Add(doctor);
			context.Patients.Add(patient);
			context.Prescriptions.Add(prescription);
			context.SaveChanges();

			var result = prescriptionsService.GetPatientsPrescriptions(patient.Id.ToString());
	
			Assert.AreEqual(1, result.Count());
			Assert.IsNotNull(result);
		}
	}
}