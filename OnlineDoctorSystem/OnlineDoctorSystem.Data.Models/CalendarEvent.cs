using OnlineDoctorSystem.Data.Common.Models;

namespace OnlineDoctorSystem.Data.Models
{
	public class CalendarEvent : BaseDeletableModel<int>
	{
		public DateTime Start { get; set; }

		public DateTime End { get; set; }

		public string Text { get; set; }

		public string Color { get; set; }
	}
}