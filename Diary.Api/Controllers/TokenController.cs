using Diary.Application.Services;
using Diary.Domain.Dto;
using Diary.Domain.Interfaces.Services;
using Diary.Domain.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Diary.Api.Controllers
{
    [ApiController]
    public class TokenController : Controller
    {
        private readonly ITokenService _tokenService;
        public TokenController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost]
        [Route("refresh")]
        public async Task<ActionResult<BaseResult<TokenDTO>>> RefreshToken([FromBody]TokenDTO tokenDTO)
        {
            var response = await _tokenService.RefreshToken(tokenDTO);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
