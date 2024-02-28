using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diary.Domain.Enum
{
    public enum ErrorCodes
    {
        // 0 - 10 - Report
        // 11 - 20 - User

        ReportsNotFound = 0,
        ReportNotFound,
        ReportAlreadyExists,

        InternalServerError = 10,
        UserNotFound,
        UserAlreadyExists,

        PasswordNotEqualsPasswordConfirm = 21,
        PasswordIsWrong

    }
}
