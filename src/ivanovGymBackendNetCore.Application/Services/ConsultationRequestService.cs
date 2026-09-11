using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ivanovGymBackendNetCore.Application.DTOs;
using ivanovGymBackendNetCore.Application.Interfaces;
using ivanovGymBackendNetCore.Domain.Entities;
using ivanovGymBackendNetCore.Domain.Interfaces;

namespace ivanovGymBackendNetCore.Application.Services
{
    public class ConsultationRequestService : IConsultationRequestService
    {
        private readonly IConsultationRequestRepository _repository;
        private readonly IMapper _mapper;

        public ConsultationRequestService(IConsultationRequestRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<List<ConsultationRequestDto>> GetRequests()
        {
            var requests = await _repository.GetAllAsync();
            return _mapper.Map<List<ConsultationRequestDto>>(requests);
        }

        public async Task CreateRequest(CreateConsultationRequestDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Имя не может быть пустым", nameof(dto.Name));

            if (string.IsNullOrWhiteSpace(dto.Phone))
                throw new ArgumentException("Телефон не может быть пустым", nameof(dto.Phone));

            try
            {
                ConsultationRequest creatingModel = _mapper.Map<ConsultationRequest>(dto);
                await _repository.CreateRequestAsync(creatingModel);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Ошибка при создании запроса на консультацию", ex);
            }
        }
    }
}