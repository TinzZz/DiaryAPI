using Diary.Domain.Dto;
using Diary.Domain.Dto.User;
using Diary.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diary.Domain.Interfaces.Services
{
    /// <summary>
    /// Сервис авторизации/регистрации
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<BaseResult<UserDTO>> Register(RegisterUserDTO dto);
        
        /// <summary>
        /// авторизация пользователя
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<BaseResult<TokenDTO>> Login(LoginUserDTO dto);
    }
}
