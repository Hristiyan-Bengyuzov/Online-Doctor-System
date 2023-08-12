using Microsoft.AspNetCore.Mvc;
using OnlineDoctorSystem.Services.Data.Interfaces;

namespace OnlineDoctorSystem.Web.Areas.Administration.Controllers
{
	public class ContactsController : AdministrationController
	{
		private readonly IContactSubmissionsService submissionsService;

		public ContactsController(IContactSubmissionsService submissionsService)
		{
			this.submissionsService = submissionsService;
		}

		public async Task<IActionResult> GetSubmissions()
		{
			var model = await submissionsService.GetContactSubmissionsAsync();
			return View(model);
		}
	}
}
