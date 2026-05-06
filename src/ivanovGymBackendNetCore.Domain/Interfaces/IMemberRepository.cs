using ivanovGymBackendNetCore.Domain.Entities;

namespace ivanovGymBackendNetCore.Domain.Interfaces;

public interface IMemberRepository
{
    Task<IEnumerable<Member>> GetAllAsync();
    Task<Member?> GetByIdAsync(Guid id);
    Task<Member> CreateAsync(Member member);
    Task<Member> UpdateAsync(Member member);
    Task DeleteAsync(Guid id);
}
