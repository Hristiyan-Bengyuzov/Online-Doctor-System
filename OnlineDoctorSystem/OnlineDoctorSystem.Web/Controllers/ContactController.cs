using Microsoft.AspNetCore.Mvc;
using OnlineDoctorSystem.Services.Data.Interfaces;
using OnlineDoctorSystem.Web.Infrastructure.Extensions;
using OnlineDoctorSystem.Web.ViewModels.Consultations;
using OnlineDoctorSystem.Web.ViewModels.Contacts;

namespace OnlineDoctorSystem.Web.Controllers
{
	public class ContactController : Controller
	{
		private readonly IContactSubmissionsService contactSubmissionsService;
		private readonly IDoctorsService doctorsService;
		private readonly IPatientsService patientsService;

		public ContactController(IContactSubmissionsService contactSubmissionsService, IDoctorsService doctorsService, IPatientsService patientsService)
		{
			this.contactSubmissionsService = contactSubmissionsService;
			this.doctorsService = doctorsService;
			this.patientsService = patientsService;
		}

		[HttpGet]
		public async Task<IActionResult> Add()
		{
			string userId = User.GetId()!;
			AddContactSubmissionFormModel model;

			if (User.IsDoctor())
			{
				var doctor = await this.doctorsService.GetDoctorByUserIdAsync(userId);
				model = new AddContactSubmissionFormModel { Name = doctor.Name, Email = doctor.User.Email };
			}
			else
			{
				var patient = await this.patientsService.GetPatientByUserIdAsync(userId);
				model = new AddContactSubmissionFormModel { Name = patient.Name, Email = patient.User.Email };
			}

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Add(AddContactSubmissionFormModel model)
		{
			if (!ModelState.IsValid)
			{
				return this.View(model);
			}

			await this.contactSubmissionsService.AddAsync(model);
			return RedirectToAction(nameof(ThankYou));
		}

		public IActionResult ThankYou()
		{
			return this.View();
		}

		public IActionResult Index()
		{
			return View();
		}
	}
}
