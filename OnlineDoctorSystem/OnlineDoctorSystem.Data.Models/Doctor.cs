using OnlineDoctorSystem.Data.Common.Models;
using OnlineDoctorSystem.Data.Models.Enums;

namespace OnlineDoctorSystem.Data.Models
{
	public class Doctor : BaseDeletableModel<Guid>
    {
        public Doctor()
        {
            this.Id = Guid.NewGuid();
            this.Consultations = new HashSet<Consultation>();
            this.Reviews = new HashSet<Review>();
        }

        public string Name { get; set; }

        public int SpecialtyId { get; set; }

        public Specialty Specialty { get; set; }

        public int TownId { get; set; }

        public Town Town { get; set; }

        public string Phone { get; set; }

        public string? ImageUrl { get; set; }

        public DateTime BirthDate { get; set; }

        public string DoctorUserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<Prescription> Prescriptions { get; set; }

        public Gender Gender { get; set; }

        public int YearsOfPractice { get; set; }

        public string SmallInfo { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }

        public string Education { get; set; }

        public string Qualifications { get; set; }

        public string Biography { get; set; }

        public virtual ICollection<Consultation> Consultations { get; set; }
    }
}