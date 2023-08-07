using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineDoctorSystem.Common;
using OnlineDoctorSystem.Data.Models;
using OnlineDoctorSystem.Services.Data.Interfaces;
using OnlineDoctorSystem.Web.Infrastructure.Extensions;
using OnlineDoctorSystem.Web.ViewModels.Events;

namespace OnlineDoctorSystem.Web.Controllers
{
	[Authorize]
	[Produces("application/json")]
	[Route("api/[controller]")]
	public class EventsController : Controller
	{
		private readonly IEventsService eventsService;

		public EventsController(IEventsService eventsService)
		{
			this.eventsService = eventsService;
		}

		[HttpGet]
		public ActionResult<IEnumerable<CalendarEvent>> GetEvents()
		{
			var userId = this.User.GetId()!;

			if (this.User.IsInRole(GlobalConstants.DoctorRole))
			{
				return eventsService.GetDoctorsEvents(userId);
			}
			else if (this.User.IsInRole(GlobalConstants.PatientRole))
			{
				return this.eventsService.GetPatientsEvents(userId);
			}

			return this.NoContent();
		}

		[HttpDelete("{id}")]
		[Authorize(Roles = GlobalConstants.DoctorRole)]
		public async Task<IActionResult> DeleteEvent([FromRoute] int id)
		{
			if (!ModelState.IsValid)
			{
				return this.BadRequest(this.ModelState);
			}

			await this.eventsService.DeleteEventByIdAsync(id);

			return this.Ok();
		}

		[HttpPut("{id}/move")]
		[Authorize(Roles = GlobalConstants.DoctorRole)]
		public async Task<IActionResult> MoveEvent([FromRoute] int id, [FromBody] EventMoveParams param)
		{
			if (!this.ModelState.IsValid)
			{
				return this.BadRequest(this.ModelState);
			}

			await this.eventsService.MoveEvent(id, param.Start, param.End);

			return this.Ok();
		}

		[HttpPut("{id}/color")]
		public async Task<IActionResult> SetEventColor([FromRoute] int id, [FromBody] EventColorParams param)
		{
			if (!this.ModelState.IsValid)
			{
				return this.BadRequest(this.ModelState);
			}

			await this.eventsService.ChangeEventColor(id, param.Color);

			return this.Ok();
		}
	}
}
