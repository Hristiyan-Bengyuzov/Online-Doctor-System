using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineDoctorSystem.Data.Models;
using System.Reflection;

namespace OnlineDoctorSystem.Data
{
    public class OnlineDoctorDbContext : IdentityDbContext<ApplicationUser>
    {
        public OnlineDoctorDbContext(DbContextOptions<OnlineDoctorDbContext> options)
            : base(options)
        {
        }

        public DbSet<Prescription> Prescriptions { get; set; } = null!;

        public DbSet<Town> Towns { get; set; } = null!;

        public DbSet<Doctor> Doctors { get; set; } = null!;

        public DbSet<Patient> Patients { get; set; } = null!;

        public DbSet<Consultation> Consultations { get; set; } = null!;

        public DbSet<Review> Reviews { get; set; } = null!;

        public DbSet<Specialty> Specialties { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            Assembly configAssembly = Assembly.GetAssembly(typeof(OnlineDoctorDbContext)) ??
                                      Assembly.GetExecutingAssembly();
            builder.ApplyConfigurationsFromAssembly(configAssembly);

            base.OnModelCreating(builder);
        }
    }
}