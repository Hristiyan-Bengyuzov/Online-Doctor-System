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
				Name = "Dr. John Doe",
				Biography = "Experienced doctor",
				Education = "Medical School",
				Phone = "123-456-7890",
				Qualifications = "MD",
				SmallInfo = "Specializes in General Medicine",
			};

			var patient = new Patient
			{
				Name = "Patient John Doe",
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
	}
}