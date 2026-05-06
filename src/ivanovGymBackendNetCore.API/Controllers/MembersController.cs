using ivanovGymBackendNetCore.Application.DTOs;
using ivanovGymBackendNetCore.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ivanovGymBackendNetCore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MembersController : ControllerBase
{
    private readonly IMemberService _memberService;

    public MembersController(IMemberService memberService)
    {
        _memberService = memberService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetAllMembers()
    {
        var members = await _memberService.GetAllMembersAsync();
        return Ok(members);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MemberDto>> GetMember(Guid id)
    {
        var member = await _memberService.GetMemberByIdAsync(id);

        if (member == null)
            return NotFound();

        return Ok(member);
    }

    [HttpPost]
    public async Task<ActionResult<MemberDto>> CreateMember([FromBody] CreateMemberDto dto)
    {
        var member = await _memberService.CreateMemberAsync(dto);
        return CreatedAtAction(nameof(GetMember), new { id = member.Id }, member);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<MemberDto>> UpdateMember(Guid id, [FromBody] UpdateMemberDto dto)
    {
        var member = await _memberService.UpdateMemberAsync(id, dto);

        if (member == null)
            return NotFound();

        return Ok(member);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMember(Guid id)
    {
        await _memberService.DeleteMemberAsync(id);
        return NoContent();
    }
}
