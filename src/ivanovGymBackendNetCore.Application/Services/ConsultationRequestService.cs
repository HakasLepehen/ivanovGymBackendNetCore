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

        public async Task<int> CreateRequest(CreateConsultationRequestDto dto)
        {
            var model = _mapper.Map<ConsultationRequest>(dto);
            ConsultationRequest result = await _repository.CreateRequestAsync(model);

            return result.Id;
        }
    }
}