using OnlineDoctorSystem.Web.ViewModels.Prescriptions;

namespace OnlineDoctorSystem.Services.Data.Interfaces
{
	public interface IPrescriptionsService
	{
		Task AddPrescriptionAsync(AddPrescriptionFormModel model);
	}
}
