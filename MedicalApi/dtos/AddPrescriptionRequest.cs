using MedicalApi.dtos;

namespace MedicalApi.Dtos;

public class AddPrescriptionRequest
{
    public PatientDto Patient { get; set; } = default!;
    public int IdDoctor { get; set; } = default!;
    public DateTime Date { get; set; } = default!;
    public DateTime DueDate { get; set; } = default!;
    public List<PrescriptionMedicamentDto> Medicaments { get; set; } = default!;
    
}