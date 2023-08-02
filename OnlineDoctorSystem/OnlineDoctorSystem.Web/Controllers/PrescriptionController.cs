using Microsoft.AspNetCore.Mvc;
using OnlineDoctorSystem.Services.Data.Interfaces;
using OnlineDoctorSystem.Web.Infrastructure.Extensions;
using OnlineDoctorSystem.Web.ViewModels.Prescriptions;

namespace OnlineDoctorSystem.Web.Controllers
{
	public class PrescriptionController : Controller
	{
		private readonly IDoctorsService doctorsService;
		private readonly IPrescriptionsService prescriptionsService;

		public PrescriptionController(IDoctorsService doctorsService, IPrescriptionsService prescriptionsService)
		{
			this.doctorsService = doctorsService;
			this.prescriptionsService = prescriptionsService;
		}

		public async Task<IActionResult> Add(string patientId)
		{
			var model = new AddPrescriptionFormModel()
			{
				PatientId = patientId,
				Doctor = await this.doctorsService.GetDoctorByIdAsync(User.GetId()!)
			};
			return this.View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Add(AddPrescriptionFormModel model)
		{
			if (!ModelState.IsValid)
			{
				return this.View();
			}
		
			await this.prescriptionsService.AddPrescriptionAsync(model);
			return this.RedirectToAction("SuccessfullyBooked", "Consultation");
		}



		public IActionResult Index()
		{
			return View();
		}
	}
}
