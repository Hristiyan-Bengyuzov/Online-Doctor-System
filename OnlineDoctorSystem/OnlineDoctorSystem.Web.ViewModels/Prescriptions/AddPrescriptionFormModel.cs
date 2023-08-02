using OnlineDoctorSystem.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace OnlineDoctorSystem.Web.ViewModels.Prescriptions
{
	public class AddPrescriptionFormModel
	{
		public string? DoctorId { get; set; }

		public virtual Doctor? Doctor { get; set; }

		public string PatientId { get; set; }

		public virtual Patient? Patient { get; set; }

		[Required(ErrorMessage = "Въведете име на медикамента")]
		public string MedicamentName { get; set; }

		[Required(ErrorMessage = "Въведете инструкции")]
		public string Instructions { get; set; }
	}
}