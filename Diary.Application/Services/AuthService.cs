using AutoMapper;
using Diary.Application.Resources;
using Diary.Domain.Dto;
using Diary.Domain.Dto.Report;
using Diary.Domain.Dto.User;
using Diary.Domain.Entity;
using Diary.Domain.Enum;
using Diary.Domain.Interfaces.Repositories;
using Diary.Domain.Interfaces.Services;
using Diary.Domain.Result;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Diary.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<UserToken> _userTokenRepository;
        private readonly ITokenService _tokenService;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public AuthService(IBaseRepository<User> userRepository, IBaseRepository<UserToken> userTokenRepository, ITokenService tokenService, ILogger logger, IMapper mapper)
        {
            _userRepository = userRepository;
            _userTokenRepository = userTokenRepository;
            _logger = logger;
            _mapper = mapper;   
            _tokenService = tokenService;
        }

        private string HashPassword(string password)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes).ToLower();
        }

        private bool IsVerifiedPassword(string userPasswordDbHash, string userPassword)
        {
            var hash = HashPassword(userPassword);
            return hash == userPasswordDbHash;
        }



        public async Task<BaseResult<TokenDTO>> Login(LoginUserDTO dto)
        {
            try
            {
                var user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Login == dto.Login);
                if (user == null)
                {
                    return new BaseResult<TokenDTO>()
                    {
                        ErrorMessage = ErrorMessage.UserNotFound,
                        ErrorCode = (int)ErrorCodes.UserNotFound
                    };
                }
                if (!IsVerifiedPassword(user.Password, dto.Password))
                {
                    return new BaseResult<TokenDTO>
                    {
                        ErrorMessage = ErrorMessage.PasswordIsWrong,
                        ErrorCode = (int)ErrorCodes.PasswordIsWrong
                    };
                }
                var userToken = await _userTokenRepository.GetAll().FirstOrDefaultAsync(s => s.UserId == user.Id);
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, user.Login),
                    new Claim(ClaimTypes.Role, "User")
                };
                var accessToken = _tokenService.GenerateAccessToken(claims);
                var refreshToken = _tokenService.GenerateRefreshToken();
                {
                    if (userToken == null)
                    {
                        userToken = new UserToken()
                        {
                            UserId = user.Id,
                            RefreshToken = refreshToken,
                            RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7)
                        };
                        await _userTokenRepository.CreateAsync(userToken);
                    }
                    else
                    {
                        userToken.RefreshToken = refreshToken;
                        userToken.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
                        await _userTokenRepository.UpdateAsync(userToken);
                    }

                    return new BaseResult<TokenDTO>()
                    {
                        Data = new TokenDTO()
                        {
                            AccessToken = accessToken,
                            RefreshToken = refreshToken
                        }

                    };
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return new BaseResult<TokenDTO>()
                {
                    ErrorMessage = ErrorMessage.InternalServerError,
                    ErrorCode = (int)ErrorCodes.InternalServerError
                };
            }
        }
        



        public async Task<BaseResult<UserDTO>> Register(RegisterUserDTO dto)
        {
            if (dto.Password != dto.PasswordConfirm)
            {
                return new BaseResult<UserDTO>()
                {
                    ErrorMessage = ErrorMessage.PasswordNotEqualsPasswordConfirm,
                    ErrorCode = (int)ErrorCodes.PasswordNotEqualsPasswordConfirm
                };
            }
            try
            {
                var user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Login == dto.Login);
                if (user != null)
                {
                    return new BaseResult<UserDTO>()
                    {
                        ErrorMessage = ErrorMessage.UserAlreadyExists,
                        ErrorCode = (int)ErrorCodes.UserAlreadyExists
                    };
                }
                var hashUserPassword = HashPassword(dto.Password);
                user = new User()
                {
                    Login = dto.Login,
                    Password = hashUserPassword
                };
                await _userRepository.CreateAsync(user);
                return new BaseResult<UserDTO>()
                {
                    Data = _mapper.Map<UserDTO>(user)
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return new BaseResult<UserDTO>()
                {
                    ErrorMessage = ErrorMessage.InternalServerError,
                    ErrorCode = (int)ErrorCodes.InternalServerError
                };
            }
        }
    }
}
