using Microsoft.AspNetCore.Mvc;
using OnlineDoctorSystem.Services.Data.Interfaces;
using OnlineDoctorSystem.Web.ViewModels.Reviews;

namespace OnlineDoctorSystem.Web.Controllers
{
	public class ReviewController : Controller
	{
		private IDoctorsService doctorsService;
		private IReviewsService reviewsService;

		public ReviewController(IReviewsService reviewsService, IDoctorsService doctorsService)
		{
			this.reviewsService = reviewsService;
			this.doctorsService = doctorsService;
		}

		public IActionResult Add(string doctorId)
		{
			var model = new AddReviewFormModel
			{
				DoctorId = doctorId,
				DoctorName = this.doctorsService.GetDoctorNameByIdAsync(doctorId).Result
			};

			return this.View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Add(AddReviewFormModel model)
		{
			model.DoctorName = await this.doctorsService.GetDoctorNameByIdAsync(model.DoctorId);

			if (!ModelState.IsValid)
			{
				return this.View(model);
			}

			await this.reviewsService.AddReviewAsync(model);
			return this.RedirectToAction("Details", "Doctor", new { id = model.DoctorId });
		}


		public IActionResult Index()
		{
			return View();
		}
	}
}
