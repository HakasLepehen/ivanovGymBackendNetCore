using ivanovGymBackendNetCore.Application.DTOs;

namespace ivanovGymBackendNetCore.Application.Interfaces;

public interface IMemberService
{
    Task<IEnumerable<MemberDto>> GetAllMembersAsync();
    Task<MemberDto?> GetMemberByIdAsync(Guid id);
    Task<MemberDto> CreateMemberAsync(CreateMemberDto dto);
    Task<MemberDto?> UpdateMemberAsync(Guid id, UpdateMemberDto dto);
    Task DeleteMemberAsync(Guid id);
}
