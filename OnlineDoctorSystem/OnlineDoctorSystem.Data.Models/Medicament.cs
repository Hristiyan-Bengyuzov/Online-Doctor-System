using OnlineDoctorSystem.Data.Common.Models;

namespace OnlineDoctorSystem.Data.Models
{
    public class Medicament : BaseDeletableModel<Guid>
    {
        public string Name { get; set; }

        public string Instructions { get; set; }
    }
}
