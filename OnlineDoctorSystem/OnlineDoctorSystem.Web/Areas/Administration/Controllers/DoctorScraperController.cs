using Microsoft.AspNetCore.Mvc;
using OnlineDoctorSystem.Services.Scraping;
using OnlineDoctorSystem.Web.ViewModels.Doctors;

namespace OnlineDoctorSystem.Web.Areas.Administration.Controllers
{
	public class DoctorScraperController : AdministrationController
	{
		private readonly IDoctorScraperService doctorScraper;

		public DoctorScraperController(IDoctorScraperService doctorScraper)
		{
			this.doctorScraper = doctorScraper;
		}

		[HttpGet]
		public IActionResult GatherDoctors()
		{
			return this.View();
		}

		[HttpPost]
		public async Task<IActionResult> GatherDoctors(DoctorsScraperFormModel model)
		{
			var addedDoctors = await this.doctorScraper.Import(model.DoctorsCount, model.TownId);
			return this.RedirectToAction(nameof(Added), new { addedDoctors = addedDoctors });
		}

		public IActionResult Added(int addedDoctors)
		{
			this.ViewData["Count"] = addedDoctors;
			return this.View();
		}
	}
}
