using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineDoctorSystem.Data.Models;

namespace OnlineDoctorSystem.Data
{
	public class OnlineDoctorDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
	{
		public OnlineDoctorDbContext(DbContextOptions<OnlineDoctorDbContext> options)
			: base(options)
		{
		}
	}
}