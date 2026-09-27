using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ivanovGymBackendNetCore.Domain.Entities;

namespace ivanovGymBackendNetCore.Domain.Interfaces
{
    public interface IConsultationRequestsRepository
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
        /// <summary>
        /// Очистить запросов с сайта по которым не было обратной связи
        /// </summary>
        /// <param name="resetIdentity">Сбросить ли счётчик идентификаторов (RESTART IDENTITY)</param>
        /// <param name="cancellationToken">Токен отмены</param>
        Task<List<ConsultationRequest>> RemoveUnansweredRequestsFromDB(bool resetIdentity = true, CancellationToken cancellationToken = default);
    }
}