using OnlineDoctorSystem.Web.ViewModels.Contacts;

namespace OnlineDoctorSystem.Services.Data.Interfaces
{
	public interface IContactSubmissionsService
	{
		Task AddAsync(AddContactSubmissionFormModel model);
	}
}
