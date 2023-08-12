using Ganss.Xss;
using OnlineDoctorSystem.Data;
using OnlineDoctorSystem.Data.Models;
using OnlineDoctorSystem.Services.Data.Interfaces;
using OnlineDoctorSystem.Web.ViewModels.Reviews;

namespace OnlineDoctorSystem.Services.Data
{
	public class ReviewsService : IReviewsService
	{
		private readonly OnlineDoctorDbContext context;
		private IDoctorsService doctorsService;

		public ReviewsService(OnlineDoctorDbContext context, IDoctorsService doctorsService)
		{
			this.context = context;
			this.doctorsService = doctorsService;
		}

		public async Task AddReviewAsync(AddReviewFormModel model)
		{
			var htmlSanitizer = new HtmlSanitizer();

			var doctor = await this.doctorsService.GetDoctorByIdAsync(model.DoctorId);

			var review = new Review
			{
				Rating = model.Rating,
				Text = htmlSanitizer.Sanitize(model.ReviewText)
			};

			doctor.Reviews.Add(review);
			await this.context.Reviews.AddAsync(review);
			await this.context.SaveChangesAsync();
		}
	}
}
