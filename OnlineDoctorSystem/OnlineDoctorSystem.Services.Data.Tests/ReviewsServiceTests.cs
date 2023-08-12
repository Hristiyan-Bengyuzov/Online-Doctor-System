using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Moq;
using OnlineDoctorSystem.Data;
using OnlineDoctorSystem.Data.Models;
using OnlineDoctorSystem.Services.Data.Interfaces;
using OnlineDoctorSystem.Web.ViewModels.Reviews;

namespace OnlineDoctorSystem.Services.Data.Tests
{
	[TestFixture]
	public class ReviewsServiceTests
	{
		[Test]
		public async Task AddReviewAsync_ShouldAddReviewToDoctorAndContext()
		{
			var options = new DbContextOptionsBuilder<OnlineDoctorDbContext>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
				.Options;

			using (var context = new OnlineDoctorDbContext(options))
			{
				var doctorsServiceMock = new Mock<IDoctorsService>();

				var doctor = new Doctor();
				doctorsServiceMock.Setup(s => s.GetDoctorByIdAsync(It.IsAny<string>())).ReturnsAsync(doctor);

				var reviewsService = new ReviewsService(context, doctorsServiceMock.Object);
				var model = new AddReviewFormModel
				{
					DoctorId = Guid.NewGuid().ToString(),
					Rating = 5,
					ReviewText = "Great doctor!"
				};

				await reviewsService.AddReviewAsync(model);

				Assert.AreEqual(1, doctor.Reviews.Count);
				Assert.AreEqual(1, context.Reviews.Count());
			}
		}
	}
}