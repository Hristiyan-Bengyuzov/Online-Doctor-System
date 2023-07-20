using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineDoctorSystem.Data.Models;

namespace OnlineDoctorSystem.Data.Configurations
{
    public class DoctorEntityConfiguration : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.HasOne(d => d.Specialty)
                  .WithMany(s => s.Doctors)
                  .HasForeignKey(d => d.SpecialtyId)
                  .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.Town)
                   .WithMany(t => t.Doctors)
                   .HasForeignKey(d => d.TownId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.User)
                   .WithOne(u => u.Doctor)
                   .HasForeignKey<Doctor>(d => d.DoctorUserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
