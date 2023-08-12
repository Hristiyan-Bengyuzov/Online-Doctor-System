using Microsoft.EntityFrameworkCore;
using OnlineDoctorSystem.Data;
using OnlineDoctorSystem.Data.Models;
using OnlineDoctorSystem.Services.Data.Interfaces;

namespace OnlineDoctorSystem.Services.Data.Tests
{
	[TestFixture]
	public class TownsServiceTests
	{
		private OnlineDoctorDbContext context;
		private ITownsService townsService;

		[SetUp]
		public void Setup()
		{
			var options = new DbContextOptionsBuilder<OnlineDoctorDbContext>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
				.Options;

			context = new OnlineDoctorDbContext(options);

			context.Towns.AddRange
			(
				new Town() { Id = 1, Name = "Town1" },
				new Town() { Id = 2, Name = "Town2" },
				new Town() { Id = 3, Name = "Town3" }
			);

			context.SaveChanges();

			townsService = new TownsService(context);
		}

		[TearDown]
		public void TearDown()
		{
			context.Dispose();
		}

		[Test]
		public async Task AllTownNamesAsync_ShouldReturnAllTownNames()
		{
			var townNames = await townsService.AllTownNamesAsync();

			Assert.AreEqual(3, townNames.Count());
			Assert.IsTrue(townNames.Contains("Town1"));
			Assert.IsTrue(townNames.Contains("Town2"));
			Assert.IsTrue(townNames.Contains("Town3"));
		}

		[Test]
		public async Task GetTownNameByIdAsync_ShouldReturnCorrectName()
		{
			var townName = await townsService.GetTownNameByIdAsync(2);

			Assert.AreEqual("Town2", townName);
		}

		[Test]
		public async Task GetAllTowns_ShouldReturnAllTowns()
		{
			var townViewModels = await townsService.GetAllTowns();

			Assert.AreEqual(3, townViewModels.Count());
			Assert.IsTrue(townViewModels.Any(t => t.Name == "Town1"));
			Assert.IsTrue(townViewModels.Any(t => t.Name == "Town2"));
			Assert.IsTrue(townViewModels.Any(t => t.Name == "Town3"));
		}
	}
}