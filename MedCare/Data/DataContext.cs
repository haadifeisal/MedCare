using MedCare.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace MedCare.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<DoctorPatient>()
                .HasKey(p => new { p.DoctorId, p.PatientId });

            modelBuilder.Entity<DoctorPatient>()
                .HasOne(dp => dp.Doctor)
                .WithMany(d => d.Patients)
                .HasForeignKey(dp => dp.DoctorId);

            modelBuilder.Entity<DoctorPatient>()
                .HasOne(dp => dp.Patient)
                .WithMany(d => d.Doctors)
                .HasForeignKey(dp => dp.PatientId);
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<DoctorPatient> DoctorPatients { get; set; }
        public DbSet<Record> Records { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}
