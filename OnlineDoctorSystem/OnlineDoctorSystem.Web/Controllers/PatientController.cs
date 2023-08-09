using Microsoft.AspNetCore.Mvc;

namespace OnlineDoctorSystem.Web.Controllers
{
    public class PatientController : Controller
    {
        public IActionResult ThankYou()
        {
            return this.View();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
