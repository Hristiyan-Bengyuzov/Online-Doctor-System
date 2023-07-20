namespace OnlineDoctorSystem.Data.Models
{
    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public virtual Doctor Doctor { get; set; }

        public virtual Patient Patient { get; set; }
    }
}