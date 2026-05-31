using AutoMapper;
using ivanovGymBackendNetCore.Application.DTOs;
using ivanovGymBackendNetCore.Domain.Entities;

namespace ivanovGymBackendNetCore.Application.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Member, MemberDto>();
        CreateMap<CreateMemberDto, Member>();
        CreateMap<UpdateMemberDto, Member>();
        CreateMap<CreateClientDto, Client>();
        CreateMap<Client, ClientDto>();
        CreateMap<ClientDto, Client>();
        CreateMap<ExerciseDto, Exercise>();
    }
}
