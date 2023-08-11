namespace OnlineDoctorSystem.Web.ViewModels.Doctors
{
	public class AllDoctorsQueryModel
	{
		public string? Specialty { get; set; }
		public string? Name { get; set; }
		public string? Town { get; set; }

		public IEnumerable<string> Specialties { get; set; } = new HashSet<string>();
		public IEnumerable<string> Towns { get; set; } = new HashSet<string>();

		public int CurrentPage { get; set; } = 1;
		public int DoctorsPerPage { get; set; } = 3;
		public int TotalDoctors { get; set; }

        public double Latitude { get; set; }
		public double Longitude { get; set; }

        public IEnumerable<AllDoctorsViewModel> Doctors { get; set; } = new HashSet<AllDoctorsViewModel>();
	}
}
