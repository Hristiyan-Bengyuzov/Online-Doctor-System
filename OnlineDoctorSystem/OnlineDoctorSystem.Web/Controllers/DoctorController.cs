using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineDoctorSystem.Services.Data.Interfaces;
using OnlineDoctorSystem.Services.Data.Models.Doctors;
using OnlineDoctorSystem.Web.ViewModels.Doctors;

namespace OnlineDoctorSystem.Web.Controllers
{
    public class DoctorController : Controller
    {
        private readonly ISpecialtiesService specialtiesService;
        private readonly ITownsService townsService;
        private readonly IDoctorsService doctorsService;

        public DoctorController(ISpecialtiesService specialtiesService, ITownsService townsService, IDoctorsService doctorsService)
        {
            this.specialtiesService = specialtiesService;
            this.townsService = townsService;
            this.doctorsService = doctorsService;
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

        public IActionResult Index()
        {
            return View();
        }
    }
}
