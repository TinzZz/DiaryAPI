using Asp.Versioning;
using Diary.Domain.Dto.Report;
using Diary.Domain.Entity;
using Diary.Domain.Interfaces.Services;
using Diary.Domain.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Diary.Api.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportsService;
        public ReportController(IReportService reportsService)
        {
            _reportsService = reportsService;
        }

        /// <summary>
        /// Удаление отчёта по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ///<remarks>
        ///Request for report delete
        ///
        ///     Delete
        ///         {
        ///             "Id": 1
        ///         }
        ///         
        ///</remarks> 
        ///<response code="200">Отчёт удален</response>
        ///<response code="400">Отчёт не удален</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<ReportDTO>>> DeleteReport(long id)
        {
            var response = await _reportsService.DeleteReportAsync(id);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }


        /// <summary>
        /// Получение отчётов по id пользователя
        /// </summary>
        /// <param userId="id"></param>
        /// <returns></returns>
        ///<remarks>
        ///Request for getting report by user id
        ///
        ///     GET
        ///         {
        ///             "Id": 1
        ///         }
        ///         
        ///</remarks> 
        ///<response code="200">Отчёты получены</response>
        ///<response code="400">Отчёты не получены</response>
        [HttpGet("reports/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<ReportDTO>>> GetUserReports(long userId)
        {
            var response = await _reportsService.GetReportsAsync(userId);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        /// <summary>
        /// Получение отчёта по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ///<remarks>
        ///Request for getting report by id
        ///
        ///     GET
        ///         {
        ///             "Id": 1
        ///         }
        ///         
        ///</remarks> 
        ///<response code="200">Отчёт получен</response>
        ///<response code="400">Отчёт не получен</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<ReportDTO>>> GetReport(long id)
        {
            var response = await _reportsService.GetReportByIdAsync(id);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }


        /// <summary>
        /// Создание отчёта
        /// </summary>
        /// <param object="dto"></param>
        /// <returns></returns>
        ///<remarks>
        ///Request for report creating
        ///
        ///     POST
        ///         {
        ///             "name": "Report #1"
        ///             "description": "Test report",
        ///             "userId": 1
        ///         }
        ///         
        ///</remarks> 
        ///<response code="200">Отчёт создан</response>
        ///<response code="400">Отчёт не создан</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<ReportDTO>>> CreateReport([FromBody]CreateReportDTO dto)
        {
            var response = await _reportsService.CreateReportAsync(dto);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        /// <summary>
        /// Обновление отчёта
        /// </summary>
        /// <param object="dto"></param>
        /// <returns></returns>
        ///<remarks>
        ///Request for report updating
        ///
        ///     PUT
        ///         {
        ///             "Id": 1
        ///             "name": "Report #1"
        ///             "description": "Test report",
        ///         }
        ///         
        ///</remarks> 
        ///<response code="200">Отчёт создан</response>
        ///<response code="400">Отчёт не создан</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<ReportDTO>>> UpdateReport([FromBody]UpdateReportDTO dto)
        {
            var response = await _reportsService.UpdateReportAsync(dto);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
