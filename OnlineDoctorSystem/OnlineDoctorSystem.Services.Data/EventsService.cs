using Microsoft.EntityFrameworkCore;
using OnlineDoctorSystem.Data;
using OnlineDoctorSystem.Data.Models;
using OnlineDoctorSystem.Services.Data.Interfaces;

namespace OnlineDoctorSystem.Services.Data
{
    public class EventsService : IEventsService
    {
        private readonly OnlineDoctorDbContext context;
        private readonly IDoctorsService doctorsService;
        private readonly IPatientsService patientsService;

        public EventsService(OnlineDoctorDbContext context, IDoctorsService doctorsService, IPatientsService patientsService)
        {
            this.context = context;
            this.doctorsService = doctorsService;
            this.patientsService = patientsService;
        }

        public async Task ChangeEventColor(int eventId, string color)
        {
            var @event = this.context.CalendarEvents.First(e => e.Id == eventId);
            @event.Color = color;
            await this.context.SaveChangesAsync();
        }

        public async Task DeleteEventByIdAsync(int id)
        {
            var consultation = this.context.Consultations
                .Include(c => c.CalendarEvent)
                .ToList()
                .First(c => c.CalendarEvent.Id == id);

            consultation.IsActive = false;
            consultation.IsDeleted = true;
            consultation.CalendarEvent.IsDeleted = true;

            await this.context.SaveChangesAsync();
        }

        public async Task MoveEvent(int eventId, DateTime startTime, DateTime endTime)
        {
            var consultation = this.context.Consultations
              .Include(x => x.CalendarEvent)
              .First(x => x.CalendarEvent.Id == eventId);

            consultation.Date = startTime.Date;
            consultation.StartTime = startTime.TimeOfDay;
            consultation.EndTime = endTime.TimeOfDay;

            consultation.CalendarEvent.Start = startTime;
            consultation.CalendarEvent.End = endTime;

            await this.context.SaveChangesAsync();
        }

        public List<CalendarEvent> GetDoctorsEvents(string userId)
        {
            var doctor = this.doctorsService.GetDoctorByUserIdAsync(userId).GetAwaiter().GetResult();

            var events = this.context.Consultations
                .Include(c => c.CalendarEvent)
                .Where(c => c.DoctorId == doctor.Id && c.IsActive && c.IsConfirmed == true)
                .Select(c => c.CalendarEvent)
                .ToList();

            return events;
        }

        public List<CalendarEvent> GetPatientsEvents(string userId)
        {
            var patient = this.patientsService.GetPatientByUserIdAsync(userId).GetAwaiter().GetResult();

            var events = this.context.Consultations
                .Include(c => c.CalendarEvent)
                .Where(c => c.PatientId == patient.Id && c.IsActive && c.IsConfirmed == true)
                .Select(c => c.CalendarEvent)
                .ToList();

            return events;
        }
    }
}
