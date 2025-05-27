using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalApi.dtos;

public class PatientDetailsResponse
{
    public int IdPatient { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public DateTime Birthdate { get; set; } = default!;

    public List<PrescriptionDto> Prescriptions { get; set; } = default!;
    
}
