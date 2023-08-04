using Microsoft.EntityFrameworkCore;
using OnlineDoctorSystem.Data;
using OnlineDoctorSystem.Data.Models;
using OnlineDoctorSystem.Services.Data.Interfaces;

namespace OnlineDoctorSystem.Services.Data
{
	internal class PatientsService : IPatientsService
	{
		private readonly OnlineDoctorDbContext context;

		public PatientsService(OnlineDoctorDbContext context)
		{
			this.context = context;
		}

		public async Task<Patient> GetPatientByIdAsync(string id) => await this.context.Patients.FirstAsync(d => d.Id.ToString() == id);

		public async Task<Patient> GetPatientByUserIdAsync(string id) => await this.context.Patients.FirstAsync(d => d.PatientUserId == id);
	}
}
