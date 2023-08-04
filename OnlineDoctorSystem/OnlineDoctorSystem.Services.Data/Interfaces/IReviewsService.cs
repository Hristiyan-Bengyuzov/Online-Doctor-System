using OnlineDoctorSystem.Web.ViewModels.Reviews;

namespace OnlineDoctorSystem.Services.Data.Interfaces
{
	public interface IReviewsService
	{
		Task AddReviewAsync(AddReviewFormModel model);
	}
}
