using Ganss.Xss;
using Microsoft.EntityFrameworkCore;
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
			var htmlSanitizer = new HtmlSanitizer();

			var contactSubmission = new ContactSubmission
			{
				Name = model.Name!,
				Email = model.Email!,
				Title = htmlSanitizer.Sanitize(model.Title),
				Content = htmlSanitizer.Sanitize(model.Content),
			};

			await this.context.ContactSubmissions.AddAsync(contactSubmission);
			await this.context.SaveChangesAsync();
		}

		public async Task<IEnumerable<ContactSubmissionViewModel>> GetContactSubmissionsAsync()
		{
			var contactSubmissions = await this.context.ContactSubmissions
				.Select(c => new ContactSubmissionViewModel
				{
					Name = c.Name,
					Email = c.Email,
					Title = c.Title,
					Content = c.Content,
				})
				.ToListAsync();

			return contactSubmissions;
		}
	}
}
