using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineDoctorSystem.Web.ViewModels.Doctors
{
    public class AllDoctorsQueryModel
    {
        public string? Specialty { get; set; }
        public string? Name { get; set; }
        public string? Town { get; set; }

        public IEnumerable<string> Specialties { get; set; } = new HashSet<string>();
        public IEnumerable<string> Towns { get; set; } = new HashSet<string>();

        public int CurrentPage { get; set; } = 1;
        public int DoctorsPerPage { get; set; } = 3;
        public int TotalDoctors { get; set; }

        public IEnumerable<AllDoctorsViewModel> Doctors { get; set; } = new HashSet<AllDoctorsViewModel>();
    }
}
