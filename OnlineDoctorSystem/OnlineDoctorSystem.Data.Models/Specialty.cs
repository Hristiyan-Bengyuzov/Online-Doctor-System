namespace OnlineDoctorSystem.Data.Models
{
    using OnlineDoctorSystem.Data.Common.Models;

    public class Specialty : BaseDeletableModel<int>
    {
        public string Name { get; set; }

        public ICollection<Doctor> Doctors { get; set; } = new HashSet<Doctor>();
    }
}