using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalApi.dtos;


[NotMapped]
public class DoctorDto
{
    public int IdDoctor { get; set; } = default!;
    public string FirstName { get; set; } = default!;
}
