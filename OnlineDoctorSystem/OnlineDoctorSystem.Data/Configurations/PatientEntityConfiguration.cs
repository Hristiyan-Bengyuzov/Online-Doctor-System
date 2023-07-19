using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineDoctorSystem.Data.Models;

namespace OnlineDoctorSystem.Data.Configurations
{
    public class PatientEntityConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.HasOne(p => p.Town)
                  .WithMany(t => t.Patients)
                  .HasForeignKey(p => p.TownId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
