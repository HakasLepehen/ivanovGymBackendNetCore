using ivanovGymBackendNetCore.Application.DTOs;

namespace ivanovGymBackendNetCore.Application.Interfaces
{
    public interface IConsultationRequestsService
    {
        Task<List<ConsultationRequestDto>> GetRequests();
        Task CreateRequest(CreateConsultationRequestDto dto);
        Task<List<ConsultationRequestDto>> RemoveUnansweredRequests(CancellationToken cancellationToken = default);
        Task CompleteRequest(int id);
    }
}