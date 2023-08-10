using OnlineDoctorSystem.Data.Common.Models;

namespace OnlineDoctorSystem.Data.Models
{
    public class ContactSubmission : BaseDeletableModel<int>
    {
        public string Name { get; set; }

        public string Title { get; set; }

        public string Email { get; set; }

        public string Content { get; set; }
    }
}
