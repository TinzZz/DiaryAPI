using Diary.Application.Resources;
using Diary.Domain.Dto;
using Diary.Domain.Entity;
using Diary.Domain.Interfaces.Repositories;
using Diary.Domain.Interfaces.Services;
using Diary.Domain.Result;
using Diary.Domain.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Diary.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly IBaseRepository<User> _userRepository;
        private readonly string _jwtKey;
        private readonly string _issuer;
        private readonly string _audience;

        public TokenService(IOptions<JwtSettings> options, IBaseRepository<User> userRepository)
        {
            _userRepository = userRepository;
            _jwtKey = options.Value.JwtKey;
            _issuer = options.Value.Issuer;
            _audience = options.Value.Audience;
        }

        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var secutiryToken = new JwtSecurityToken(_issuer, _audience, claims, null, DateTime.UtcNow.AddMinutes(10), credentials);
            var token = new JwtSecurityTokenHandler().WriteToken(secutiryToken);
            return token;
        }

        public string GenerateRefreshToken()
        {
            var randomNumbers = new byte[32];
            using var randomNumbergenarator = RandomNumberGenerator.Create();
            randomNumbergenarator.GetBytes(randomNumbers);
            return Convert.ToBase64String(randomNumbers);
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken)
        {
            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateAudience= true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey)),
                ValidateLifetime = true,
                ValidAudience = _audience,
                ValidIssuer = _issuer
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var claimsPrinciple = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out var securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg
                .Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException(ErrorMessage.InvalidToken);
            return claimsPrinciple;
        }

        public async Task<BaseResult<TokenDTO>> RefreshToken(TokenDTO dto)
        {
            var accessToken = dto.AccessToken;
            var refreshToken = dto.RefreshToken;
            var claimsPrinciple = GetPrincipalFromExpiredToken(accessToken);
            var userName = claimsPrinciple.Identity?.Name;
            var user = await _userRepository.GetAll().Include(s => s.UserToken).FirstOrDefaultAsync(s => s.Login == userName);
            if (user == null || user.UserToken.RefreshToken != refreshToken ||
                user.UserToken.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return new BaseResult<TokenDTO>()
                {
                    ErrorMessage = ErrorMessage.InvalidClientRequest
                };
            }
            var newAccessToken = GenerateAccessToken(claimsPrinciple.Claims);
            var newRefreshToken = GenerateRefreshToken();
            user.UserToken.RefreshToken = newRefreshToken;
            await _userRepository.UpdateAsync(user);
            return new BaseResult<TokenDTO>()
            {
                Data = new TokenDTO()
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken
                }
            };
        }
    }
}
