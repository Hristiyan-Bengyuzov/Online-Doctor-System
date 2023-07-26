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
        public async Task<IActionResult> All([FromQuery] AllDoctorsQueryModel queryModel)
        {
            AllDoctorsFilteredAndPagedServiceModel serviceModel = await this.doctorsService.AllAsync(queryModel);

            queryModel.Doctors = serviceModel.Doctors;
            queryModel.TotalDoctors = serviceModel.TotalDoctorsCount;
            queryModel.Specialties = await this.specialtiesService.AllSpecialtyNamesAsync();
            queryModel.Towns = await this.townsService.AllTownNamesAsync();

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


        public IActionResult Index()
        {
            return View();
        }
    }
}
