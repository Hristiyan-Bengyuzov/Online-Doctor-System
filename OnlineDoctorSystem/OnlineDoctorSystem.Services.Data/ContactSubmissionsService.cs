using OnlineDoctorSystem.Data;
using OnlineDoctorSystem.Data.Models;
using OnlineDoctorSystem.Services.Data.Interfaces;
using OnlineDoctorSystem.Web.ViewModels.Contacts;

namespace OnlineDoctorSystem.Services.Data
{
	public class ContactSubmissionsService : IContactSubmissionsService
	{
		private readonly OnlineDoctorDbContext context;

		public ContactSubmissionsService(OnlineDoctorDbContext context)
		{
			this.context = context;
		}

		public async Task AddAsync(AddContactSubmissionFormModel model)
		{
			var contactSubmission = new ContactSubmission
			{
				Name = model.Name!,
				Email = model.Email!,
				Title = model.Title,
				Content = model.Content,
			};

			await this.context.ContactSubmissions.AddAsync(contactSubmission);
			await this.context.SaveChangesAsync();
		}
	}
}
