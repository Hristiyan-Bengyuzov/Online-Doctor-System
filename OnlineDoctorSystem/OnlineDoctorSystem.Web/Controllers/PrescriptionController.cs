﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineDoctorSystem.Common;
using OnlineDoctorSystem.Services.Data.Interfaces;
using OnlineDoctorSystem.Web.Infrastructure.Extensions;
using OnlineDoctorSystem.Web.ViewModels.Prescriptions;
using System.Security.Claims;

namespace OnlineDoctorSystem.Web.Controllers
{
	public class PrescriptionController : Controller
	{
		private readonly IDoctorsService doctorsService;
		private readonly IPrescriptionsService prescriptionsService;
		private readonly IPatientsService patientsService;

		public PrescriptionController(IDoctorsService doctorsService, IPrescriptionsService prescriptionsService, IPatientsService patientsService)
		{
			this.doctorsService = doctorsService;
			this.prescriptionsService = prescriptionsService;
			this.patientsService = patientsService;
		}

		public async Task<IActionResult> Add(string patientId)
		{
			var model = new AddPrescriptionFormModel()
			{
				PatientId = patientId,
				Doctor = await this.doctorsService.GetDoctorByUserIdAsync(User.GetId()!)
			};
			return this.View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Add(AddPrescriptionFormModel model)
		{
			model.Doctor = await this.doctorsService.GetDoctorByUserIdAsync(User.GetId()!);

			if (!ModelState.IsValid)
			{
				return this.View();
			}

			await this.prescriptionsService.AddPrescriptionAsync(model);
			return this.RedirectToAction(nameof(SuccessfullyAdded), model);
		}


		[Authorize(Roles = GlobalConstants.PatientRole)]
		public async Task<IActionResult> GetPatientsPrescriptions()
		{
			var patient = await this.patientsService.GetPatientByUserIdAsync(this.User.GetId()!);
			var model = this.prescriptionsService.GetPatientsPrescriptions(patient.Id.ToString());

			return this.View(model);
		}


		public IActionResult SuccessfullyAdded(AddPrescriptionFormModel model)
		{
			return this.View(new SuccessfullyAddedPrescription { PatientName = this.patientsService.GetPatientByIdAsync(model.PatientId).Result.Name });
		}

		public IActionResult Index()
		{
			return View();
		}
	}
}
