using MedicalApi.Data;
using MedicalApi.dtos;
using MedicalApi.Dtos;
using MedicalApi.Models;
using Microsoft.EntityFrameworkCore;

namespace MedicalApi.Services;

public class PrescriptionService : IPrescriptionService
{
    private readonly MedicalContext _context;

    public PrescriptionService(MedicalContext context)
    {
        _context = context;
    }

    public async Task AddPrescriptionAsync(AddPrescriptionRequest request)
    {
        if (request.Medicaments.Count > 10)
            throw new ArgumentException("Maksymalnie 10 lekow");

        if (request.DueDate < request.Date)
            throw new ArgumentException("Po terminie waznosci");

        foreach (var med in request.Medicaments)
        {
            var exists = await _context.Medicaments.AnyAsync(m => m.IdMedicament == med.IdMedicament);
            if (!exists)
                throw new ArgumentException($"Lek {med.IdMedicament} nie znaleziony");
        }

        // -> czy pacijent istnieje [?]
        var patient = await _context.Patients.FirstOrDefaultAsync(p =>
            p.FirstName == request.Patient.FirstName &&
            p.LastName == request.Patient.LastName &&
            p.Birthdate.Date == request.Patient.Birthdate.Date);

        if (patient == null)
        {
            patient = new Patient
            {
                FirstName = request.Patient.FirstName,
                LastName = request.Patient.LastName,
                Birthdate = request.Patient.Birthdate
            };

            await _context.Patients.AddAsync(patient);
            await _context.SaveChangesAsync();
        }

        var prescription = new Prescription
        {
            Date = request.Date,
            DueDate = request.DueDate,
            IdDoctor = request.IdDoctor,
            IdPatient = patient.IdPatient,
            PrescriptionMedicaments = request.Medicaments.Select(m => new PrescriptionMedicament
            {
                IdMedicament = m.IdMedicament,
                Dose = m.Dose,
                Description = m.Description
            }).ToList()
        };

        await _context.Prescriptions.AddAsync(prescription);
        await _context.SaveChangesAsync();
    }

    public async Task<PatientDetailsResponse> GetPatientDetailsAsync(int patientId)
    {
        var patient = await _context.Patients
            .Include(p => p.Prescriptions)
                .ThenInclude(r => r.Doctor)
            .Include(p => p.Prescriptions)
                .ThenInclude(r => r.PrescriptionMedicaments)
                    .ThenInclude(pm => pm.Medicament)
            .FirstOrDefaultAsync(p => p.IdPatient == patientId);

        if (patient == null) return null;

        return new PatientDetailsResponse
        {
            IdPatient = patient.IdPatient,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            Birthdate = patient.Birthdate,
            Prescriptions = patient.Prescriptions
                .OrderBy(p => p.DueDate)
                .Select(p => new PrescriptionDto
                {
                    IdPrescription = p.IdPrescription,
                    Date = p.Date,
                    DueDate = p.DueDate,
                    Doctor = new DoctorDto
                    {
                        IdDoctor = p.Doctor.IdDoctor,
                        FirstName = p.Doctor.FirstName
                    },
                    Medicaments = p.PrescriptionMedicaments.Select(pm => new MedicamentDto
                    {
                        IdMedicament = pm.Medicament.IdMedicament,
                        Name = pm.Medicament.Name,
                        Dose = pm.Dose,
                        Description = pm.Description
                    }).ToList()
                }).ToList()
        };
    }
}
