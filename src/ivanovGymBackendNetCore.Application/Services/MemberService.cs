using AutoMapper;
using ivanovGymBackendNetCore.Application.DTOs;
using ivanovGymBackendNetCore.Application.Interfaces;
using ivanovGymBackendNetCore.Domain.Entities;
using ivanovGymBackendNetCore.Domain.Interfaces;

namespace ivanovGymBackendNetCore.Application.Services;

public class MemberService : IMemberService
{
    private readonly IMemberRepository _memberRepository;
    private readonly IMapper _mapper;

    public MemberService(IMemberRepository memberRepository, IMapper mapper)
    {
        _memberRepository = memberRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<MemberDto>> GetAllMembersAsync()
    {
        var members = await _memberRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<MemberDto>>(members);
    }

    public async Task<MemberDto?> GetMemberByIdAsync(Guid id)
    {
        var member = await _memberRepository.GetByIdAsync(id);
        return member != null ? _mapper.Map<MemberDto>(member) : null;
    }

    public async Task<MemberDto> CreateMemberAsync(CreateMemberDto dto)
    {
        var member = _mapper.Map<Member>(dto);
        member.Id = Guid.NewGuid();
        member.CreatedAt = DateTime.UtcNow;
        member.IsActive = true;

        var createdMember = await _memberRepository.CreateAsync(member);
        return _mapper.Map<MemberDto>(createdMember);
    }

    public async Task<MemberDto?> UpdateMemberAsync(Guid id, UpdateMemberDto dto)
    {
        var member = await _memberRepository.GetByIdAsync(id);
        if (member == null) return null;

        _mapper.Map(dto, member);
        member.UpdatedAt = DateTime.UtcNow;

        var updatedMember = await _memberRepository.UpdateAsync(member);
        return _mapper.Map<MemberDto>(updatedMember);
    }

    public async Task DeleteMemberAsync(Guid id)
    {
        await _memberRepository.DeleteAsync(id);
    }
}
