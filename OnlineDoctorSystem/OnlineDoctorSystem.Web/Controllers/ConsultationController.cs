using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineDoctorSystem.Common;
using OnlineDoctorSystem.Services.Data.Interfaces;
using OnlineDoctorSystem.Web.Infrastructure.Extensions;
using OnlineDoctorSystem.Web.ViewModels.Consultations;
using System.Data;
using System.Security.Claims;

namespace OnlineDoctorSystem.Web.Controllers
{
    public class ConsultationController : Controller
    {
        private readonly IDoctorsService doctorsService;
        private readonly IPatientsService patientsService;
        private readonly IConsultationsService consultationsService;

        public ConsultationController(IDoctorsService doctorsService, IPatientsService patientsService, IConsultationsService consultationsService)
        {
            this.doctorsService = doctorsService;
            this.patientsService = patientsService;
            this.consultationsService = consultationsService;
        }

        [HttpGet]
        public async Task<IActionResult> Add(string id)
        {
            var doctorName = await this.doctorsService.GetDoctorNameByIdAsync(id);

            var viewModel = new AddConsultationFormModel()
            {
                DoctorId = id,
                DoctorName = doctorName,
                PatientId = User.GetId()!
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

        public IActionResult Index()
        {
            return View();
        }
    }
}
