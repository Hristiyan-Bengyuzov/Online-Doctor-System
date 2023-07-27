using OnlineDoctorSystem.Web.ViewModels.Towns;

namespace OnlineDoctorSystem.Web.ViewModels.Statistics
{
    public class StatisticsViewModel
    {
        public int DoctorsCount { get; set; }

        public int PatientsCount { get; set; }

        public int ConsultationsCount { get; set; }

        public int SpecialtiesCount { get; set; }

        public int ReviewsCount { get; set; }

        public IEnumerable<TownStatisticsViewModel> Towns { get; set; }
    }
}
