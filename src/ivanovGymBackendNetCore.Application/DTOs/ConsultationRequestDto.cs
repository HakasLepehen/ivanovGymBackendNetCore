using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ivanovGymBackendNetCore.Application.DTOs
{
    public class ConsultationRequestDto
    {
        public int? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public bool IsCalled { get; set; } = false;
    }
}