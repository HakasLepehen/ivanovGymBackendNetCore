using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ivanovGymBackendNetCore.Domain.Entities;

namespace ivanovGymBackendNetCore.Domain.Interfaces
{
    public interface IConsultationRequestRepository
    {
        Task<List<ConsultationRequest>> GetAllAsync();
        /// <summary>
        /// Поставить метку обработки звонка
        /// </summary>
        /// <param name="id">Идентификатор запроса с сайта</param>
        /// <returns></returns>
        Task CompleteAsync(int id);
        Task CreateRequestAsync(ConsultationRequest model);
        Task DeleteAsync(int id);
    }
}