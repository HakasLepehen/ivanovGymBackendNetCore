using ivanovGymBackendNetCore.Application.DTOs;

namespace ivanovGymBackendNetCore.Application.Interfaces
{
    public interface IConsultationRequestService
    {
        Task<List<ConsultationRequestDto>> GetRequests();
        Task<int> CreateRequest(CreateConsultationRequestDto dto);
    }
}