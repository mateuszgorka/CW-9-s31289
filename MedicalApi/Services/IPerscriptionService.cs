using MedicalApi.dtos;

namespace MedicalApi.Services;

public interface IPrescriptionService
{
    Task AddPrescriptionAsync(AddPrescriptionRequest request);
    Task<PatientDetailsResponse> GetPatientDetailsAsync(int patientId);
}
