using OnlineDoctorSystem.Data;
using OnlineDoctorSystem.Data.Models;
using OnlineDoctorSystem.Services.Data;
using OnlineDoctorSystem.Services.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace OnlineDoctorSystem.Tests.Services
{
	[TestFixture]
	public class EventsServiceTests
	{
		private OnlineDoctorDbContext context;
		private IEventsService eventsService;
		private IDoctorsService doctorsService;
		private IPatientsService patientsService;
		private ITownsService townsService;
		private ISpecialtiesService specialtiesService;

		[SetUp]
		public void SetUp()
		{
			var options = new DbContextOptionsBuilder<OnlineDoctorDbContext>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
				.Options;


			this.context = new OnlineDoctorDbContext(options);
			this.townsService = new TownsService(context);
			this.specialtiesService = new SpecialtiesService(context);
			this.doctorsService = new DoctorsService(context, townsService, specialtiesService);
			this.patientsService = new PatientsService(context);
			this.eventsService = new EventsService(context, doctorsService, patientsService);

			SeedData();
		}

		private void SeedData()
		{
			var doctorUser = new ApplicationUser { Id = "doctorUserId", UserName = Guid.NewGuid().ToString() };
			context.Users.Add(doctorUser);

			var doctor = new Doctor
			{
				DoctorUserId = doctorUser.Id,
				User = doctorUser,
				Biography = "Biography",
				Education = "Education",
				Name = "Georgi",
				Phone = "123456789",
				Qualifications = "Qualifications",
				SmallInfo = "Small info"
			};
			context.Doctors.Add(doctor);

			var patientUser = new ApplicationUser { Id = "patientUserId", UserName = Guid.NewGuid().ToString() };
			context.Users.Add(patientUser);

			var patient = new Patient
			{
				PatientUserId = patientUser.Id,
				User = patientUser,
				Name = "Ivan",
				Phone = "987654321"
			};
			context.Patients.Add(patient);

			var consultation = new Consultation
			{
				DoctorId = doctor.Id,
				PatientId = patient.Id,
				IsActive = true,
				IsConfirmed = true,
				Description = "Boli me glavata",
				CalendarEvent = new CalendarEvent
				{
					Id = 1,
					Color = "yellow",
					Text = "Boli me glavata"
				}
			};
			context.Consultations.Add(consultation);

			context.SaveChanges();
		}

		[TearDown]


		[Test]
		public void ChangeEventColor_ShouldChangeColor()
		{
			var eventId = 1;
			var color = "NewColor";

			eventsService.ChangeEventColor(eventId, color).Wait();

			var updatedEvent = context.CalendarEvents.FirstOrDefault(e => e.Id == eventId);
			Assert.IsNotNull(updatedEvent);
			Assert.AreEqual(color, updatedEvent.Color);
		}

		[Test]
		public void DeleteEventByIdAsync_ShouldDeleteEventAndConsultation()
		{
			var eventId = 1;

			eventsService.DeleteEventByIdAsync(eventId).Wait();

			var deletedConsultation = context.Consultations
				.Include(c => c.CalendarEvent)
				.FirstOrDefault(c => c.CalendarEvent.Id == eventId);

			Assert.IsNotNull(deletedConsultation);
			Assert.IsFalse(deletedConsultation.IsActive);
			Assert.IsTrue(deletedConsultation.IsDeleted);
			Assert.IsTrue(deletedConsultation.CalendarEvent.IsDeleted);
		}

		[Test]
		public void MoveEvent_ShouldUpdateEventTimes()
		{
			var eventId = 1;
			var newStartTime = DateTime.Now.Date.AddHours(14);
			var newEndTime = DateTime.Now.Date.AddHours(16);

			eventsService.MoveEvent(eventId, newStartTime, newEndTime).Wait();

			var movedConsultation = context.Consultations
				.Include(c => c.CalendarEvent)
				.FirstOrDefault(c => c.CalendarEvent.Id == eventId);

			Assert.IsNotNull(movedConsultation);
			Assert.AreEqual(newStartTime.Date, movedConsultation.Date);
			Assert.AreEqual(newStartTime.TimeOfDay, movedConsultation.StartTime);
			Assert.AreEqual(newEndTime.TimeOfDay, movedConsultation.EndTime);
			Assert.AreEqual(newStartTime, movedConsultation.CalendarEvent.Start);
			Assert.AreEqual(newEndTime, movedConsultation.CalendarEvent.End);
		}

		[Test]
		public void GetDoctorsEvents_ShouldReturnCorrectEvents()
		{
			var doctorUserId = "doctorUserId";

			var events = eventsService.GetDoctorsEvents(doctorUserId);

			Assert.AreEqual(1, events.Count);
		}

		[Test]
		public void GetPatientsEvents_ShouldReturnCorrectEvents()
		{
			var patientUserId = "patientUserId";

			var events = eventsService.GetPatientsEvents(patientUserId);

			Assert.AreEqual(1, events.Count);
		}
	}
}