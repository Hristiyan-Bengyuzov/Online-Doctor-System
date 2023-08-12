using Ganss.Xss;
using Microsoft.EntityFrameworkCore;
using OnlineDoctorSystem.Data;
using OnlineDoctorSystem.Data.Models;
using OnlineDoctorSystem.Services.Data.Interfaces;
using OnlineDoctorSystem.Web.ViewModels.Prescriptions;

namespace OnlineDoctorSystem.Services.Data
{
	public class PrescriptionsService : IPrescriptionsService
	{
		private readonly OnlineDoctorDbContext context;

		public PrescriptionsService(OnlineDoctorDbContext context)
		{
			this.context = context;
		}

		public async Task AddPrescriptionAsync(AddPrescriptionFormModel model)
		{
			var htmlSanitizer = new HtmlSanitizer();

			var prescription = new Prescription
			{
				Doctor = model.Doctor!,
				DoctorId = model.Doctor!.Id,
				Patient = model.Patient!,
				PatientId = Guid.Parse(model.PatientId!),
				MedicamentName = htmlSanitizer.Sanitize(model.MedicamentName!),
				Instructions = htmlSanitizer.Sanitize(model.Instructions!),
			};

			await this.context.Prescriptions.AddAsync(prescription);
			await this.context.SaveChangesAsync();
		}

		public IEnumerable<PrescriptionViewModel> GetPatientsPrescriptions(string id)
		{
			var prescriptions = this.context.Patients
				.Include(p => p.Prescriptions)
				.ThenInclude(p => p.Doctor)
				.First(d => d.Id == Guid.Parse(id))
				.Prescriptions
				.Select(p => new PrescriptionViewModel
				{
					DoctorName = p.Doctor.Name,
					MedicamentName = p.MedicamentName,
					Instructions = p.Instructions,
				});

			return prescriptions;
		}
	}
}
