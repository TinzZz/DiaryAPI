using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diary.Domain.Dto.User
{
    public record RegisterUserDTO(string Login, string Password, string PasswordConfirm)
    {
    }
}
