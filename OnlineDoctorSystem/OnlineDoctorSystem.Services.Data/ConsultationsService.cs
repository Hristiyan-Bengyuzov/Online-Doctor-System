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
            if (model.StartTime > model.EndTime)
            {
                return false;
            }
            else if (model.StartTime == model.EndTime)
            {
                return false;
            }
            else if (model.Date <= DateTime.Now)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> AddAsync(AddConsultationFormModel model)
        {
            if (!IsTimeCorrect(model)) return false;

            var doctor = await this.doctorsService.GetDoctorByIdAsync(model.DoctorId);
            var patient = await this.patientsService.GetPatientByIdAsync(model.PatientId);

            var consultation = new Consultation
            {
                Date = model.Date,
                Description = model.Description,
                StartTime = model.StartTime,
                EndTime = model.EndTime,
                PatientId = patient.Id,
                Patient = patient,
                DoctorId = doctor.Id,
                Doctor = doctor,
                IsActive = true,
                IsConfirmed = null
            };

            doctor.Consultations.Add(consultation);

            await this.context.Consultations.AddAsync(consultation);
            await this.context.SaveChangesAsync();

            return true;
        }
    }
}
