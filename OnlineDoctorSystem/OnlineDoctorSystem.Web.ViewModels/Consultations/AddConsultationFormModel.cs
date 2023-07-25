using System.ComponentModel.DataAnnotations;

namespace OnlineDoctorSystem.Web.ViewModels.Consultations
{
    public class AddConsultationFormModel
    {
        public string DoctorId { get; set; }

        public string? DoctorName { get; set; }

        public string PatientId { get; set; }

        [Required(ErrorMessage = "Моля въведете дата на консултацията")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Моля въведете начален час на консултацията")]
        [DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; }

        [Required(ErrorMessage = "Моля въведете краен час")]
        [DataType(DataType.Time)]
        public TimeSpan EndTime { get; set; }

        [Required(ErrorMessage = "Моля въведете заявката си.")]
        [MaxLength(200)]
        public string Description { get; set; } = null!;
    }
}
