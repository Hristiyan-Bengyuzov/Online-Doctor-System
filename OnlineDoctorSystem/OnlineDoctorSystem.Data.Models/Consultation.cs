namespace OnlineDoctorSystem.Data.Models
{
    using System;

    using OnlineDoctorSystem.Data.Common.Models;

    public class Consultation : BaseDeletableModel<Guid>
    {
        public Consultation()
        {
            this.Id = Guid.NewGuid();
        }

        public DateTime Date { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public Guid PatientId { get; set; }

        public Patient Patient { get; set; }

        public Guid DoctorId { get; set; }

        public Doctor Doctor { get; set; }

        public bool? IsConfirmed { get; set; }

        public bool IsActive { get; set; }

        public string Description { get; set; }

        public virtual CalendarEvent CalendarEvent { get; set; }
    }
}