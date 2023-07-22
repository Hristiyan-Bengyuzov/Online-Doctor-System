namespace OnlineDoctorSystem.Web.ViewModels.Doctors
{
	public class DoctorDetailsViewModel
	{
		public string Id { get; set; } = null!;
		public string Name { get; set; } = null!;
		public string Specialty { get; set; } 
		public string Town { get; set; } 
		public string ImageUrl { get; set; } = null!;
		public string SmallInfo { get; set; } = null!;
		public string Education { get; set; } = null!;
		public string Qualifications { get; set; } = null!;
		public string Biography { get; set; } = null!;
	}
}
