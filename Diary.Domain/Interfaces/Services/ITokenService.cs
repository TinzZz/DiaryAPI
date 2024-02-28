using Diary.Domain.Dto;
using Diary.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Diary.Domain.Interfaces.Services
{
    public interface ITokenService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        Task<BaseResult<TokenDTO>> RefreshToken(TokenDTO dto);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken);

    }
}
