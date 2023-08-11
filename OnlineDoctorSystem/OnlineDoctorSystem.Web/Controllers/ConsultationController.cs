using Microsoft.AspNetCore.Mvc;
using OnlineDoctorSystem.Services.Data.Interfaces;
using OnlineDoctorSystem.Web.Infrastructure.Extensions;
using OnlineDoctorSystem.Web.ViewModels.Consultations;

namespace OnlineDoctorSystem.Web.Controllers
{
	public class ConsultationController : Controller
	{
		private readonly IDoctorsService doctorsService;
		private readonly IPatientsService patientsService;
		private readonly IConsultationsService consultationsService;
		private readonly IEventsService eventsService;

		public ConsultationController(IDoctorsService doctorsService, IPatientsService patientsService, IConsultationsService consultationsService, IEventsService eventsService)
		{
			this.doctorsService = doctorsService;
			this.patientsService = patientsService;
			this.consultationsService = consultationsService;
			this.eventsService = eventsService;
		}

		[HttpGet]
		public async Task<IActionResult> Add(string id)
		{
			var doctorName = await this.doctorsService.GetDoctorNameByIdAsync(id);

			var viewModel = new AddConsultationFormModel()
			{
				DoctorId = id,
				DoctorName = doctorName,
				PatientId = this.patientsService.GetPatientByUserIdAsync(User.GetId()!).Result.Id.ToString()
			};

			return this.View(viewModel);
		}


		[HttpPost]
		public async Task<IActionResult> Add(AddConsultationFormModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			model.DoctorName = await this.doctorsService.GetDoctorNameByIdAsync(model.DoctorId);

			if (await this.consultationsService.AddAsync(model))
			{
				return this.RedirectToAction(nameof(this.SuccessfullyBooked), model);
			}

			return this.View("InvalidTime");
		}

		public IActionResult SuccessfullyBooked(AddConsultationFormModel model)
		{
			var viewModel = new SuccessfullyBookedViewModel() { Date = model.Date, DoctorName = model.DoctorName! };
			return this.View(viewModel);
		}

		public async Task<IActionResult> Approve(string consultationId)
		{
			await this.consultationsService.Approve(consultationId);

			return this.RedirectToAction("GetUnconfirmedConsultations", "Doctor");
		}

		public async Task<IActionResult> Decline(string consultationId)
		{
			await this.consultationsService.Decline(consultationId);

			return this.RedirectToAction("GetUnconfirmedConsultations", "Doctor");
		}

		public async Task<IActionResult> GetUsersConsultations()
		{
			string userId = User.GetId()!;
			IEnumerable<ConsultationViewModel> model;

			if (User.IsDoctor())
			{
				var doctor = await this.doctorsService.GetDoctorByUserIdAsync(userId);
				model = await this.consultationsService.GetDoctorsConsultationsAsync(doctor.Id.ToString());
			}
			else
			{
				var patient = await this.patientsService.GetPatientByUserIdAsync(userId);
				model = await this.consultationsService.GetPatientsConsultationsAsync(patient.Id.ToString());
			}

			return this.View(model);
		}

		public async Task<IActionResult> RemoveConsultation(int eventId)
		{
			await this.eventsService.DeleteEventByIdAsync(eventId);

			return this.RedirectToAction(nameof(this.GetUsersConsultations));
		}

		public IActionResult UserCalendar()
		{
			return this.View();
		}

		public IActionResult Index()
		{
			return View();
		}
	}
}
