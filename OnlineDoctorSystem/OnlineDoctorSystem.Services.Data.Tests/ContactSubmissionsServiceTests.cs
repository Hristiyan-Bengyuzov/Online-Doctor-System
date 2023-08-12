using Microsoft.EntityFrameworkCore;
using OnlineDoctorSystem.Data;
using OnlineDoctorSystem.Services.Data;
using OnlineDoctorSystem.Services.Data.Interfaces;
using OnlineDoctorSystem.Web.ViewModels.Contacts;

namespace OnlineDoctorSystem.Tests.Services.Data
{
	[TestFixture]
	public class ContactSubmissionsServiceTests
	{
		private OnlineDoctorDbContext context;
		private IContactSubmissionsService contactSubmissionsService;

		[SetUp]
		public void Setup()
		{
			var options = new DbContextOptionsBuilder<OnlineDoctorDbContext>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
				.Options;

			context = new OnlineDoctorDbContext(options);
			contactSubmissionsService = new ContactSubmissionsService(context);
		}

		[TearDown]
		public void TearDown()
		{
			context.Dispose();
		}

		[Test]
		public async Task AddAsync_ShouldAddContactSubmission()
		{
			var model = new AddContactSubmissionFormModel
			{
				Name = "Ivan",
				Email = "ivan@abv.bg",
				Title = "Test Title",
				Content = "Test Content"
			};

			await contactSubmissionsService.AddAsync(model);

			var addedSubmission = context.ContactSubmissions.FirstOrDefault();
			Assert.IsNotNull(addedSubmission);
			Assert.AreEqual("Ivan", addedSubmission.Name);
			Assert.AreEqual("ivan@abv.bg", addedSubmission.Email);
			Assert.AreEqual("Test Title", addedSubmission.Title);
			Assert.AreEqual("Test Content", addedSubmission.Content);
		}

		[Test]
		public async Task GetContactSubmissionsAsync_ShouldReturnContactSubmissions()
		{
			var model = new AddContactSubmissionFormModel
			{
				Name = "Ivan",
				Email = "ivan@abv.bg",
				Title = "Test Title",
				Content = "Test Content"
			};

			await contactSubmissionsService.AddAsync(model);

			var submissions = await this.contactSubmissionsService.GetContactSubmissionsAsync();
			Assert.IsNotNull(submissions);
			Assert.AreEqual(1, submissions.Count());
		}
	}
}