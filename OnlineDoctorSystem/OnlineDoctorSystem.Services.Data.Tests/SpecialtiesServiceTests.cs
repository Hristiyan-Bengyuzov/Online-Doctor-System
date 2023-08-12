using Microsoft.EntityFrameworkCore;
using OnlineDoctorSystem.Data.Models;
using OnlineDoctorSystem.Data;
using OnlineDoctorSystem.Services.Data.Interfaces;

namespace OnlineDoctorSystem.Services.Data.Tests
{
	[TestFixture]
	public class SpecialtiesServiceTests
	{
		private OnlineDoctorDbContext context;
		private ISpecialtiesService specialtiesService;

		[SetUp]
		public void Setup()
		{
			var options = new DbContextOptionsBuilder<OnlineDoctorDbContext>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
				.Options;

			context = new OnlineDoctorDbContext(options);

			var specialties = new List<Specialty>
			{
				new Specialty { Id = 1, Name = "Cardiology" },
				new Specialty { Id = 2, Name = "Dermatology" },
            };

			context.Specialties.AddRange(specialties);
			context.SaveChanges();

			specialtiesService = new SpecialtiesService(context);
		}

		[TearDown]
		public void TearDown()
		{
			context.Dispose();
		}

		[Test]
		public async Task AllSpecialtyNamesAsync_ShouldReturnAllSpecialtyNames()
		{
			var specialtyNames = await specialtiesService.AllSpecialtyNamesAsync();

			Assert.AreEqual(2, specialtyNames.Count()); 
			Assert.IsTrue(specialtyNames.Contains("Cardiology"));
			Assert.IsTrue(specialtyNames.Contains("Dermatology"));
		}

		[Test]
		public async Task GetSpecialtyNameByIdAsync_ShouldReturnCorrectName()
		{
			var specialtyName = await specialtiesService.GetSpecialtyNameByIdAsync(1);

			Assert.AreEqual("Cardiology", specialtyName);
		}

		[Test]
		public async Task GetAllSpecialties_ShouldReturnAllSpecialties()
		{
			var specialties = await specialtiesService.GetAllSpecialties();

			Assert.AreEqual(2, specialties.Count());

			var cardiology = specialties.FirstOrDefault(s => s.Name == "Cardiology");
			var dermatology = specialties.FirstOrDefault(s => s.Name == "Dermatology");

			Assert.IsNotNull(cardiology);
			Assert.IsNotNull(dermatology);
		}
	}
}
