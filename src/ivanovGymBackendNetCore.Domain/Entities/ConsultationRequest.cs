using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ivanovGymBackendNetCore.Domain.Entities
{
    public class ConsultationRequest
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool IsCalled { get; set; } = false;
    }
}