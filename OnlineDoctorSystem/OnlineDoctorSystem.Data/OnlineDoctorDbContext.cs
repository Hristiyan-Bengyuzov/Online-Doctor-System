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

        public DbSet<Prescription> Prescriptions { get; set; }

        public DbSet<Town> Towns { get; set; }

        public DbSet<Doctor> Doctors { get; set; }

        public DbSet<Patient> Patients { get; set; }

        public DbSet<Consultation> Consultations { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<Specialty> Specialties { get; set; }
    }
}