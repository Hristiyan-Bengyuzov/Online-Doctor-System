namespace OnlineDoctorSystem.Data.Models
{
    using System.Collections.Generic;

    using OnlineDoctorSystem.Data.Common.Models;

    public class Town : BaseDeletableModel<int>
    {
        public string Name { get; set; } = null!;

        public virtual ICollection<Doctor> Doctors { get; set; } = new HashSet<Doctor>();

        public virtual ICollection<Patient> Patients { get; set; } = new HashSet<Patient>();
    }
}