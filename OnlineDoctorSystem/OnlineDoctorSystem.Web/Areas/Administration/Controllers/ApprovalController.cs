using Microsoft.AspNetCore.Mvc;
using OnlineDoctorSystem.Services.Data.Interfaces;

namespace OnlineDoctorSystem.Web.Areas.Administration.Controllers
{
	public class ApprovalController : AdministrationController
	{
		private readonly IDoctorsService doctorsService;

		public ApprovalController(IDoctorsService doctorsService)
		{
			this.doctorsService = doctorsService;
		}

		public async Task<IActionResult> ApproveDoctor(string doctorId)
		{
			await this.doctorsService.ApproveDoctorAsync(doctorId);
			return this.RedirectToAction(nameof(this.GetUnconfirmedDoctors));
		}

		public async Task<IActionResult> DeclineDoctor(string doctorId)
		{
			await this.doctorsService.DeclineDoctorAsync(doctorId);
			return this.RedirectToAction(nameof(this.GetUnconfirmedDoctors));
		}

		public async Task<IActionResult> GetUnconfirmedDoctors()
		{
			var model = await this.doctorsService.GetUnconfirmedDoctorsAsync();
			return this.View(model);
		}
	}
}
