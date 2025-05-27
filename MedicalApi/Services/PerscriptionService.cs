using MedicalApi.data;
using MedicalApi.dtos;
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
            throw new ArgumentException("Maximum 10 medicaments allowed");

        if (request.DueDate < request.Date)
            throw new ArgumentException("DueDate must be after or equal to Date");

        foreach (var med in request.Medicaments)
        {
            var exists = await _context.Medicaments.AnyAsync(m => m.IdMedicament == med.IdMedicament);
            if (!exists) throw new ArgumentException($"Medicament {med.IdMedicament} not found");
        }

        var patient = await _context.Patients
            .FirstOrDefaultAsync(p => p.IdPatient == request.Patient.IdPatient)
            ?? request.Patient;

        var prescription = new Prescription
        {
            Date = request.Date,
            DueDate = request.DueDate,
            IdDoctor = request.IdDoctor,
            Patient = patient,
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
                .Select(p => new PatientDetailsResponse.PrescriptionDto
                {
                    IdPrescription = p.IdPrescription,
                    Date = p.Date,
                    DueDate = p.DueDate,
                    Doctor = new PatientDetailsResponse.DoctorDto
                    {
                        IdDoctor = p.Doctor.IdDoctor,
                        FirstName = p.Doctor.FirstName
                    },
                    Medicaments = p.PrescriptionMedicaments.Select(pm => new PatientDetailsResponse.MedicamentDto
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
