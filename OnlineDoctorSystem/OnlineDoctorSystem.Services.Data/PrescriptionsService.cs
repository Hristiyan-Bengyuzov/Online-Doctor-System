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
			var prescription = new Prescription
			{
				Doctor = model.Doctor!,
				DoctorId = model.Doctor!.Id,
				Patient = model.Patient!,
				PatientId = Guid.Parse(model.PatientId),
				MedicamentName = model.MedicamentName,
				Instructions = model.Instructions,
			};

			await this.context.Prescriptions.AddAsync(prescription);
			await this.context.SaveChangesAsync();
		}
	}
}
