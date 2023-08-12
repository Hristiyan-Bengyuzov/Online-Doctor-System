using OnlineDoctorSystem.Data.Models;
using OnlineDoctorSystem.Services.Data.Models.Doctors;
using OnlineDoctorSystem.Web.ViewModels.Doctors;
using OnlineDoctorSystem.Web.ViewModels.Reviews;

namespace OnlineDoctorSystem.Services.Data.Interfaces
{
	public interface IDoctorsService
	{
		Task AddDoctorToDbAsync(string userId, Doctor doctor);
		AllDoctorsFilteredAndPagedServiceModel All(AllDoctorsQueryModel queryModel);
		Task<DoctorDetailsViewModel> GetDoctorDetailsAsync(string id);
		Task<string> GetDoctorNameByIdAsync(string id);
		Task<Doctor> GetDoctorByUserIdAsync(string id);
		Task<Doctor> GetDoctorByIdAsync(string id);
		Task<IEnumerable<ReviewViewModel>> GetDoctorReviewsAsync(string id);
		Task<IEnumerable<Doctor>> GetUnconfirmedDoctorsAsync();
		Task ApproveDoctorAsync(string id);
		Task DeclineDoctorAsync(string id);
		void UpdateDoctorDistance(double latitude, double longitude);
	}
}
