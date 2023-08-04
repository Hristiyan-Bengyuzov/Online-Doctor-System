using System.ComponentModel.DataAnnotations;

namespace OnlineDoctorSystem.Web.ViewModels.Reviews
{
	public class AddReviewFormModel
	{
		public string? DoctorName { get; set; } 

		public string DoctorId { get; set; } = null!;

		[Range(1, 5, ErrorMessage = "Трябва да дадете поне една звезда")]
		public double Rating { get; set; }

		[Required(ErrorMessage = "Трябва да кажете мнението си")]
		public string ReviewText { get; set; } = null!;
	}
}