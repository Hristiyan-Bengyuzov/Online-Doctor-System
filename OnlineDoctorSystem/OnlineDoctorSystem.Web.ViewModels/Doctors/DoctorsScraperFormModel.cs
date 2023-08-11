using System.ComponentModel.DataAnnotations;

namespace OnlineDoctorSystem.Web.ViewModels.Doctors
{
	public class DoctorsScraperFormModel
	{
		[Range(1, 40)]
		public int DoctorsCount { get; set; }

		[Required]
		[Range(1, int.MaxValue)]
		public int TownId { get; set; }
	}
}
