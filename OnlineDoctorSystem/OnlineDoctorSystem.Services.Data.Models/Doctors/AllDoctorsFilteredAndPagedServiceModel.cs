using OnlineDoctorSystem.Web.ViewModels.Doctors;

namespace OnlineDoctorSystem.Services.Data.Models.Doctors
{
    public class AllDoctorsFilteredAndPagedServiceModel
    {
        public int TotalDoctorsCount { get; set; }

        public IEnumerable<AllDoctorsViewModel> Doctors { get; set; } = new HashSet<AllDoctorsViewModel>();
    }
}