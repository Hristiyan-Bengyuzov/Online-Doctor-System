using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineDoctorSystem.Data.Models;

namespace OnlineDoctorSystem.Data.Configurations
{
    public class ConsultationEntityConfiguration : IEntityTypeConfiguration<Consultation>
    {

        public void Configure(EntityTypeBuilder<Consultation> builder)
        {
            builder.HasOne(c => c.Patient)
                   .WithMany(p => p.Consultations)
                   .HasForeignKey(c => c.PatientId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Doctor)
                   .WithMany(d => d.Consultations)
                   .HasForeignKey(c => c.DoctorId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
