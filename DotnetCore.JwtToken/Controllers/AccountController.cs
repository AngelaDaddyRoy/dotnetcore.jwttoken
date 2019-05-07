using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DotnetCore.JwtToken.Models;
using DotnetCore.JwtToken.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotnetCore.JwtToken.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IUserFakeRepository _userFakeRepository;

        public AccountController(ITokenService tokenService , IUserFakeRepository userFakeRepository)
        {
            _tokenService = tokenService;
            _userFakeRepository = userFakeRepository;
        }
        [HttpPost("signin")]
        public  IActionResult  SignIn(User user)
        {
            var targetUser = _userFakeRepository.GetUserByUsername(user.Username);

            if (targetUser is null|| targetUser.Password!=user.Password)
            {
                return BadRequest();
            }
             

            var usersClaims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, targetUser.Id.ToString())
            };

            var jwtToken = _tokenService.GenerateAccessToken(usersClaims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            targetUser.RefreshToken = refreshToken;

            _userFakeRepository.UpdateUser(targetUser);

            //await _usersDb.SaveChangesAsync();

            return new ObjectResult(new
            {
                token = jwtToken,
                refreshToken = refreshToken
            });
        }
        [HttpPost("signup")]
        public  IActionResult Signup(User user)
        {
            if (string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Password))
            {
                return BadRequest();
            }
            var targetUser = _userFakeRepository.GetUserByUsername(user.Username);

            if (targetUser != null )
            {
                return StatusCode(409);
            }

            var newuser = new User
            {
                Username = user.Username,
                Password = user.Password
            };

            _userFakeRepository.AddUser(newuser);

            //await _usersDb.SaveChangesAsync();

            return Ok(newuser);
        }


    }
}