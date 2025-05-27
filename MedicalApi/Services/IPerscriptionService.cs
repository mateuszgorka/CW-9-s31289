
using MedicalApi.dtos;
using MedicalApi.Dtos;


namespace MedicalApi.Services;

public interface IPrescriptionService
{
    Task AddPrescriptionAsync(AddPrescriptionRequest request);
    Task<PatientDetailsResponse> GetPatientDetailsAsync(int patientId);
}
