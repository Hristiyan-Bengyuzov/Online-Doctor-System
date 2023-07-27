using Microsoft.AspNetCore.Mvc;
using OnlineDoctorSystem.Services.Data.Interfaces;
using OnlineDoctorSystem.Web.ViewModels.Statistics;

namespace OnlineDoctorSystem.Web.Controllers
{
    public class StatisticsController : Controller
    {
        private IStatisticsService statisticsService;

        public StatisticsController(IStatisticsService statisticsService)
        {
            this.statisticsService = statisticsService;
        }

        public async Task<IActionResult> Index()
        {
            var model = new StatisticsViewModel
            {
                DoctorsCount = await this.statisticsService.GetDoctorsCountAsync(),
                Towns = await this.statisticsService.GetTownStatisticsAsync(),
                PatientsCount = await this.statisticsService.GetPatientsCountAsync(),
                ConsultationsCount = await this.statisticsService.GetActiveConsultationsCountAsync(),
                ReviewsCount = await this.statisticsService.GetReviewsCountAsync(),
                SpecialtiesCount = await this.statisticsService.GetSpecialtiesCountAsync()
            };

            return this.View(model);
        }
    }
}
