using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineDoctorSystem.Common;

namespace OnlineDoctorSystem.Web.Areas.Administration.Controllers
{
	[Authorize(Roles = GlobalConstants.AdminRole)]
	[Area("Administration")]
	public class AdministrationController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
