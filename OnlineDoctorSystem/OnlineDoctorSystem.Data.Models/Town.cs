namespace OnlineDoctorSystem.Data.Models
{
    using System.Collections.Generic;

    using OnlineDoctorSystem.Data.Common.Models;

    public class Town : BaseDeletableModel<int>
    {
        public string Name { get; set; } = null!;

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public virtual ICollection<Doctor> Doctors { get; set; } = new HashSet<Doctor>();
    }
}