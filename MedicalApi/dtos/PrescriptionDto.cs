namespace MedicalApi.dtos;

public class PrescriptionDto
{
    public int IdPrescription { get; set; } = default!;
    public DateTime Date { get; set; } = default!;
    public DateTime DueDate { get; set; } = default!;
    public DoctorDto Doctor { get; set; } = default!;
    public List<MedicamentDto> Medicaments { get; set; } = default!;
}
