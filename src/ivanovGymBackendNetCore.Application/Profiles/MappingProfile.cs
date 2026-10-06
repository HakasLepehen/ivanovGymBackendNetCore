using AutoMapper;
using ivanovGymBackendNetCore.Application.DTOs;
using ivanovGymBackendNetCore.Domain.Entities;

namespace ivanovGymBackendNetCore.Application.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateClientDto, Client>();
        CreateMap<Client, ClientDto>();
        CreateMap<ClientDto, Client>();
        CreateMap<ExerciseDto, Exercise>();
        CreateMap<Exercise, ExerciseDto>();
        CreateMap<CreateExerciseDto, Exercise>();
        CreateMap<Training, TrainingDto>();
        CreateMap<CreateTrainingDto, Training>();
        CreateMap<TrainingDto, Training>();
        CreateMap<TrainingExercise, TrainingExerciseDto>();
        // CreatedAt формируется на стороне БД, поэтому при маппинге из DTO
        // в сущность свойство намеренно игнорируется
        CreateMap<TrainingExerciseDto, TrainingExercise>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
        CreateMap<CreateConsultationRequestDto, ConsultationRequest>();
        CreateMap<ConsultationRequest, CreateConsultationRequestDto>();
        CreateMap<ConsultationRequestDto, ConsultationRequest>();
        CreateMap<ConsultationRequest, ConsultationRequestDto>();
    }
}
