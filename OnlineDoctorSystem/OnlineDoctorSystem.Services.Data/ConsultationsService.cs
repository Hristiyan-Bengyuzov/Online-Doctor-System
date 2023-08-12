﻿using Ganss.Xss;
using Microsoft.EntityFrameworkCore;
using OnlineDoctorSystem.Data;
using OnlineDoctorSystem.Data.Models;
using OnlineDoctorSystem.Services.Data.Interfaces;
using OnlineDoctorSystem.Web.ViewModels.Consultations;

namespace OnlineDoctorSystem.Services.Data
{
	public class ConsultationsService : IConsultationsService
	{
		private readonly OnlineDoctorDbContext context;
		private readonly IDoctorsService doctorsService;
		private readonly IPatientsService patientsService;

		public ConsultationsService(OnlineDoctorDbContext context, IDoctorsService doctorsService, IPatientsService patientsService)
		{
			this.context = context;
			this.doctorsService = doctorsService;
			this.patientsService = patientsService;
		}
		private bool IsTimeCorrect(AddConsultationFormModel model)
		{
			if (model.StartTime >= model.EndTime)
			{
				return false;
			}
			else if (model.Date < DateTime.Now)
			{
				return false;
			}

			return true;
		}

		public async Task<bool> AddAsync(AddConsultationFormModel model)
		{
			if (!IsTimeCorrect(model)) return false;

			var htmlSanitizer = new HtmlSanitizer();

			var doctor = await this.doctorsService.GetDoctorByIdAsync(model.DoctorId);
			var patient = await this.patientsService.GetPatientByIdAsync(model.PatientId);

			var consultation = new Consultation
			{
				Date = model.Date,
				Description = htmlSanitizer.Sanitize(model.Description),
				StartTime = model.StartTime,
				EndTime = model.EndTime,
				PatientId = patient.Id,
				Patient = patient,
				DoctorId = doctor.Id,
				Doctor = doctor,
				IsActive = true,
				IsConfirmed = null
			};

			var calendarEvent = new CalendarEvent()
			{
				Color = "yellow",
				Start = consultation.Date + consultation.StartTime,
				End = consultation.Date + consultation.EndTime,
				Text = $"{consultation.StartTime}",
			};
			consultation.CalendarEvent = calendarEvent;

			doctor.Consultations.Add(consultation);

			await this.context.Consultations.AddAsync(consultation);
			await this.context.SaveChangesAsync();

			return true;
		}

		public async Task<Consultation> GetConsultationByIdAsync(string id) => await this.context.Consultations.FirstAsync(c => c.Id == Guid.Parse(id));

		public async Task Decline(string id)
		{
			var consultation = this.GetConsultationByIdAsync(id).Result;

			consultation.IsConfirmed = false;
			await this.context.SaveChangesAsync();
		}

		public async Task Approve(string id)
		{
			var consultation = this.GetConsultationByIdAsync(id).Result;

			consultation.IsConfirmed = true;
			await this.context.SaveChangesAsync();
		}

		public async Task<IEnumerable<Consultation>> GetUnconfirmedConsultations(string doctorId)
		{
			var doctor = await this.doctorsService.GetDoctorByUserIdAsync(doctorId);
			var consultations = await this.context.Consultations.Where(x => x.IsConfirmed == null && x.DoctorId == doctor.Id).ToListAsync();
			foreach (var consultation in consultations)
			{
				consultation.Patient = await this.patientsService.GetPatientByIdAsync(consultation.PatientId.ToString());
			}

			return consultations;
		}

		public async Task UpdateConsultationsWhenCompleted()
		{
			var pastConsultations = await this.context.Consultations
				.Where(x => x.Date < DateTime.Today || (x.Date == DateTime.Today && x.EndTime <= DateTime.Now.TimeOfDay))
				.ToListAsync();

			foreach (var consultation in pastConsultations)
			{
				consultation.IsActive = false;
			}

			await this.context.SaveChangesAsync();
		}

		public async Task<IEnumerable<ConsultationViewModel>> GetDoctorsConsultationsAsync(string doctorId)
		{
			var consultations = await this.context.Consultations
				.Where(c => c.DoctorId == Guid.Parse(doctorId))
				.Include(c => c.CalendarEvent)
				.Select(c => new ConsultationViewModel
				{
					Id = c.Id,
					DoctorName = c.Doctor.Name,
					PatientName = c.Patient.Name,
					PatientId = c.PatientId.ToString(),
					DoctorId = doctorId,
					IsActive = c.IsActive,
					IsDeleted = c.IsDeleted,
					IsConfirmed = c.IsConfirmed,
					StartTime = c.StartTime,
					EndTime = c.EndTime,
					Date = c.Date,
					Description = c.Description,
					EventId = c.CalendarEvent.Id,
				})
				.ToListAsync();

			return consultations;
		}

		public async Task<IEnumerable<ConsultationViewModel>> GetPatientsConsultationsAsync(string patientId)
		{
			var consultations = await this.context.Consultations
			   .Where(c => c.PatientId == Guid.Parse(patientId))
			   .Include(c => c.CalendarEvent)
			   .Select(c => new ConsultationViewModel
			   {
				   Id = c.Id,
				   DoctorName = c.Doctor.Name,
				   PatientName = c.Patient.Name,
				   PatientId = patientId,
				   DoctorId = c.DoctorId.ToString(),
				   IsActive = c.IsActive,
				   IsDeleted = c.IsDeleted,
				   IsConfirmed = c.IsConfirmed,
				   StartTime = c.StartTime,
				   EndTime = c.EndTime,
				   Date = c.Date,
				   Description = c.Description,
				   EventId = c.CalendarEvent.Id,
			   })
			   .ToListAsync();

			return consultations;
		}
	}
}
