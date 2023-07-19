namespace OnlineDoctorSystem.Data.Models
{
    using System;

    using OnlineDoctorSystem.Data.Common.Models;

    public class Prescription : BaseDeletableModel<Guid>
    {
        public Prescription()
        {
            this.Id = Guid.NewGuid();
        }

        public Guid DoctorId { get; set; }

        public virtual Doctor Doctor { get; set; }

        public Guid PatientId { get; set; }

        public virtual Patient Patient { get; set; }

        public virtual ICollection<Medicament> Medicaments { get; set; }
    }
}