namespace MedicalApi.dtos;

    
public class MedicamentDto
{
    public int IdMedicament { get; set; } = default!;
    public string Name { get; set; } = default!;
    public int Dose { get; set; } = default!;
    public string Description { get; set; } = default!;
}