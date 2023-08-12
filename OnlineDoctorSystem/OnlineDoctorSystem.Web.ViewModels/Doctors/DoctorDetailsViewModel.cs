using OnlineDoctorSystem.Data.Models;

namespace OnlineDoctorSystem.Web.ViewModels.Doctors
{
    public class DoctorDetailsViewModel
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Specialty { get; set; }
        public string Town { get; set; }
        public string ImageUrl { get; set; } = null!;
        public string SmallInfo { get; set; } = null!;
        public string Education { get; set; } = null!;
        public string Qualifications { get; set; } = null!;
        public string Biography { get; set; } = null!;
        public ICollection<Review> Reviews { get; set; } = new HashSet<Review>();
        public bool IsFromThirdParty { get; set; }
        public string? LinkFromThirdParty { get; set; } 
        public double AverageRating() => this.Reviews.Any() ? this.Reviews.Average(r => r.Rating) : 0;
    }
}
