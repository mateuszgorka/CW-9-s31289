using MedicalApi.dtos;
using Microsoft.EntityFrameworkCore;
using MedicalApi.Models;

namespace MedicalApi.Data;

public class MedicalContext : DbContext
{
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }

    public MedicalContext(DbContextOptions<MedicalContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

      // ->>> klucze
        modelBuilder.Entity<Doctor>().HasKey(d => d.IdDoctor);
        modelBuilder.Entity<Patient>().HasKey(p => p.IdPatient);
        modelBuilder.Entity<Medicament>().HasKey(m => m.IdMedicament);
        modelBuilder.Entity<Prescription>().HasKey(p => p.IdPrescription);
        modelBuilder.Entity<PrescriptionMedicament>()
            .HasKey(pm => new { pm.IdPrescription, pm.IdMedicament });

      // ->>> seed
        modelBuilder.Entity<Doctor>().HasData(new Doctor
        {
            IdDoctor = 1,
            FirstName = "Adam",
            LastName = "Lekarz",
            Email = "adam@szpital.pl"
        });

        modelBuilder.Entity<Medicament>().HasData(new Medicament
        {
            IdMedicament = 1,
            Name = "Paracetamol",
            Description = "Lek przeciwbólowy",
            Type = "Tabletka"
        });

        modelBuilder.Entity<Patient>().HasData(new Patient
        {
            IdPatient = 1,
            FirstName = "Anna",
            LastName = "Nowak",
            Birthdate = new DateTime(1985, 4, 15)
        });

        modelBuilder.Ignore<DoctorDto>();
        modelBuilder.Ignore<MedicamentDto>();
        modelBuilder.Ignore<PrescriptionDto>();

    }
}
