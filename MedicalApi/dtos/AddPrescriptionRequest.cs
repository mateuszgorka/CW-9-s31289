using MedicalApi.Models;

namespace MedicalApi.dtos;

public class AddPrescriptionRequest
{
    public Patient Patient { get; set; }
    public int IdDoctor { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public List<PrescriptionMedicamentDto> Medicaments { get; set; }

    public class PrescriptionMedicamentDto
    {
        public int IdMedicament { get; set; }
        public int Dose { get; set; }
        public string Description { get; set; }
    }
}
