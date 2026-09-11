using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ivanovGymBackendNetCore.Application.DTOs
{
    public class CreateConsultationRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        
    }
}