using Diary.Domain.Dto.Report;
using Diary.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diary.Domain.Interfaces.Services
{
    // Service for work with report domain part

    public interface IReportService
    {
        /// <summary>
        /// Получение всех отчётов пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<CollectionResult<ReportDTO>> GetReportsAsync(long userId);
        /// <summary>
        /// Получение отчёта по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResult<ReportDTO>> GetReportByIdAsync(long id);
        /// <summary>
        /// Создает отчёт с базовыми параметрами
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<BaseResult<ReportDTO>> CreateReportAsync(CreateReportDTO dto);
        /// <summary>
        /// Удаление отчёта по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResult<ReportDTO>> DeleteReportAsync(long id);
        /// <summary>
        /// Обновление отчёта
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<BaseResult<ReportDTO>> UpdateReportAsync(UpdateReportDTO dto);
    }
}
