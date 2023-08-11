namespace OnlineDoctorSystem.Web.ViewModels.Consultations
{
    public class ConsultationViewModel
    {
        public Guid Id { get; set; }

        public string DoctorName { get; set; }

        public string PatientName { get; set; }

        public string PatientId { get; set; }

        public string DoctorId { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public bool? IsConfirmed { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; }

        public int EventId { get; set; }
    }
}
