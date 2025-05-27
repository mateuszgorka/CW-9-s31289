namespace MedicalApi.dtos;

public class PrescriptionMedicamentDto
{
    public int IdMedicament { get; set; } = default!;
    public int Dose { get; set; } = default!;
    public string Description { get; set; } = default!;
}