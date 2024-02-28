using Diary.Domain.Entity;
using Diary.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diary.Domain.Interfaces.Validations
{
    public interface IReportValidator : IBaseValidator<Report>
    {
        /// <summary>
        /// Проверяется наличие отчёта, если отчёт с переданным названием есть в БД, то создать такой же нельзя
        /// Проверяется пользователь, если userId не найден, то такого пользователя нет
        /// </summary>
        /// <param name="report"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        BaseResult CreateValidator(Report report, User user);
    }
}
