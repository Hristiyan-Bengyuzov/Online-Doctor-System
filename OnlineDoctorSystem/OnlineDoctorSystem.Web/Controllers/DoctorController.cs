using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineDoctorSystem.Services.Data.Interfaces;
using OnlineDoctorSystem.Services.Data.Models.Doctors;
using OnlineDoctorSystem.Web.Infrastructure.Extensions;
using OnlineDoctorSystem.Web.ViewModels.Doctors;

namespace OnlineDoctorSystem.Web.Controllers
{
    public class DoctorController : Controller
    {
        private readonly ISpecialtiesService specialtiesService;
        private readonly ITownsService townsService;
        private readonly IDoctorsService doctorsService;
        private readonly IConsultationsService consultationsService;

        public DoctorController(ISpecialtiesService specialtiesService, ITownsService townsService, IDoctorsService doctorsService, IConsultationsService consultationsService)
        {
            this.specialtiesService = specialtiesService;
            this.townsService = townsService;
            this.doctorsService = doctorsService;
            this.consultationsService = consultationsService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult All([FromQuery] AllDoctorsQueryModel queryModel)
        {
            AllDoctorsFilteredAndPagedServiceModel serviceModel = this.doctorsService.All(queryModel);

            queryModel.Doctors = serviceModel.Doctors;
            queryModel.TotalDoctors = serviceModel.TotalDoctorsCount;
            queryModel.Specialties = this.specialtiesService.AllSpecialtyNamesAsync().Result;
            queryModel.Towns = this.townsService.AllTownNamesAsync().Result;

            return this.View(queryModel);
        }

        public async Task<IActionResult> Details(string id)
        {
            var model = await this.doctorsService.GetDoctorDetailsAsync(id);

            return this.View(model);
        }

        public async Task<IActionResult> GetUnconfirmedConsultations()
        {
            var consultations = await this.consultationsService.GetUnconfirmedConsultations(User.GetId()!);
            return this.View(consultations);
        }

        public async Task<IActionResult> GetReviews(string id)
        {
            var model = await this.doctorsService.GetDoctorReviewsAsync(id);

            return this.View(model);
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
