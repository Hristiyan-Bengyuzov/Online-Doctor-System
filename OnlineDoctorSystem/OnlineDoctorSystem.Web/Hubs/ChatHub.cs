using Microsoft.AspNetCore.SignalR;
using OnlineDoctorSystem.Services.Data.Interfaces;
using OnlineDoctorSystem.Web.Infrastructure.Extensions;
using OnlineDoctorSystem.Web.ViewModels.Chat;

namespace OnlineDoctorSystem.Web.Hubs
{
	public class ChatHub : Hub
	{
		private readonly IDoctorsService doctorsService;
		private readonly IPatientsService patientsService;

		public ChatHub(
			IDoctorsService doctorsService,
			IPatientsService patientsService)
		{
			this.doctorsService = doctorsService;
			this.patientsService = patientsService;
		}

		public async Task Send(string message)
		{
			var userId = this.Context.User.GetId();

			if (this.Context.User.IsDoctor())
			{
				var doctor = await this.doctorsService.GetDoctorByUserIdAsync(userId);
				await this.Clients.All.SendAsync(
					"NewMessage",
					new Message()
					{
						CreatedOn = DateTime.Now.ToShortDateString(),
						Text = message,
						ImageUrl = doctor.ImageUrl,
						User = $"{doctor.Name} (Доктор)",
						IsDoctor = true,
						IsAdmin = false,
						IsPatient = false,
					});
			}
			else if (this.Context.User.IsPatient())
			{
				var patient = await this.patientsService.GetPatientByUserIdAsync(userId);
				await this.Clients.All.SendAsync(
					"NewMessage",
					new Message()
					{
						CreatedOn = DateTime.Now.ToShortDateString(),
						Text = message,
						ImageUrl = patient.ImageUrl,
						User = $"{patient.Name} (Пациент)",
						IsDoctor = false,
						IsAdmin = false,
						IsPatient = true,
					});
			}
			else if (this.Context.User.IsAdmin())
			{
				await this.Clients.All.SendAsync(
					"NewMessage",
					new Message()
					{
						CreatedOn = DateTime.Now.ToShortDateString(),
						Text = message,
						ImageUrl = @"https://res.cloudinary.com/du3ohgfpc/image/upload/v1606322301/jrtza0zytvwqeqihpg1m.png",
						User = "Admin",
						IsDoctor = false,
						IsAdmin = true,
						IsPatient= false,
					});
			}
		}
	}
}
