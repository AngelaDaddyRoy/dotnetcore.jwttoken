using DotnetCore.JwtToken.Models;
using DotnetCore.JwtToken.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DotnetCore.JwtToken.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IUserFakeRepository _userFakeRepository;
        public TokenController(ITokenService tokenService,IUserFakeRepository userFakeRepository)
        {
            _tokenService = tokenService;
            _userFakeRepository = userFakeRepository;
        }

        [HttpPost("refresh")]
        public ActionResult<IEnumerable<string>> RefreshToken(TokenDTO tokenDTO)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(tokenDTO.Token);
            var username = principal.Identity.Name;
            //真实项目中使用数据库查询用户是否存在
            var user = _userFakeRepository.GetUserByUsername(username);
            if (user == null || user.RefreshToken != tokenDTO.RefreshToken)
                return BadRequest();
            var newJwtToken = _tokenService.GenerateAccessToken(principal.Claims);
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            //真实项目中将新产生的refreshtoken保存在数据库中
            //user.RefreshToken = newRefreshToken;
            //await _usersDb.SaveChangesAsync();
            return new ObjectResult(new
            {
                token = newJwtToken,
                refreshToken = newRefreshToken
            });
        }

        [HttpPost("revoke"), Authorize]
        public IActionResult Revoke()
        {
            var username = User.Identity.Name;

            var user = _userFakeRepository.GetUserByUsername(username);
            if (user == null) return BadRequest();

            user.RefreshToken = null;

            _userFakeRepository.UpdateUser(user);

            return NoContent();
        }


    }
}
