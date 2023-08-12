using Microsoft.EntityFrameworkCore;
using OnlineDoctorSystem.Data;
using OnlineDoctorSystem.Data.Models;
using OnlineDoctorSystem.Services.Data;
using OnlineDoctorSystem.Services.Data.Interfaces;

namespace OnlineDoctorSystem.Tests.Services.Data
{
	[TestFixture]
	public class PatientsServiceTests
	{
		private OnlineDoctorDbContext context;
		private IPatientsService patientsService;

		[SetUp]
		public void Setup()
		{
			var options = new DbContextOptionsBuilder<OnlineDoctorDbContext>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
				.Options;

			context = new OnlineDoctorDbContext(options);

			var user1 = new ApplicationUser { UserName = Guid.NewGuid().ToString() };
			var user2 = new ApplicationUser { UserName = Guid.NewGuid().ToString() };
			var user3 = new ApplicationUser { UserName = Guid.NewGuid().ToString() };


			var patients = new List<Patient>
			{
				new Patient { PatientUserId = user1.Id, Name = "John", Phone = "123456789", User = user1 },
				new Patient { PatientUserId = user2.Id, Name = "Mike", Phone = "987654321", User = user2 },
				new Patient { PatientUserId = user3.Id, Name = "Sean", Phone = "444444444", User = user3 }
			};

			context.Patients.AddRange(patients);
			context.SaveChanges();

			patientsService = new PatientsService(context);
		}

		[TearDown]
		public void TearDown()
		{
			context.Dispose();
		}

		[Test]
		public async Task GetPatientByIdAsync_ValidId_ReturnsPatient()
		{
			var targetPatient = context.Patients.First();

			var result = await patientsService.GetPatientByIdAsync(targetPatient.Id.ToString());

			Assert.NotNull(result);
			Assert.AreEqual(targetPatient.Id, result.Id);
		}

		[Test]
		public async Task GetPatientByUserIdAsync_ValidUserId_ReturnsPatient()
		{
			var targetPatient = context.Patients.First();

			var result = await patientsService.GetPatientByUserIdAsync(targetPatient.PatientUserId);

			Assert.NotNull(result);
			Assert.AreEqual(targetPatient.Id, result.Id);
		}

		[Test]
		public async Task AddPatientToDbAsync_ValidData_PatientAddedToDatabase()
		{
			var newPatient = new Patient
			{
				PatientUserId = "newUser",
				Name = "Ivan",
				Phone = "0895556517"
			};

			await patientsService.AddPatientToDbAsync(newPatient.PatientUserId, newPatient);

			var result = context.Patients.Find(newPatient.Id);
			Assert.NotNull(result);
			Assert.AreEqual(newPatient.PatientUserId, result.PatientUserId);
		}
	}
}