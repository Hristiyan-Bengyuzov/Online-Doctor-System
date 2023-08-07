using OnlineDoctorSystem.Data.Models;

namespace OnlineDoctorSystem.Services.Data.Interfaces
{
	public interface IEventsService
	{
		Task DeleteEventByIdAsync(int id);

		List<CalendarEvent> GetDoctorsEvents(string userId);

		List<CalendarEvent> GetPatientsEvents(string userId);

		Task MoveEvent(int eventId, DateTime startTime, DateTime endTime);

		Task ChangeEventColor(int eventId, string color);
	}
}
