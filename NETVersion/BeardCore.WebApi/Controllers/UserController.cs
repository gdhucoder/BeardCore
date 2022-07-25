// Copyright (c) HuGuodong 2022. All Rights Reserved.

using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace BeardCore.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly AppSettings _appSettings;
        public UserController(ILogger<UserController> logger, IOptions<AppSettings> appSettings)
        {
            _logger = logger;
        }

        /// <summary>
        /// 获取用户Token
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        public String AccquireToken(AuthenticateRequest req)
        {
            return generateJwtToken(req);
        }

        [Helpers.Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok("校验成功");
        }

        private string generateJwtToken(AuthenticateRequest user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            // var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var key = Encoding.ASCII.GetBytes("adsfasdfasdfasdfasdfasdfasdfsadf123") ;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("username", user.UserId )}),
                Expires = DateTime.UtcNow.AddSeconds(60),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public class AuthenticateRequest
        {
            [Required]
            public string UserId { get; set; }

            [Required]
            public string Password { get; set; }
        }

        public class AuthenticateResponse
        {

            public string Token { get; set; }


            public AuthenticateResponse(string token)
            {
                Token = token;
            }
        }

        public class AppSettings
        {
            public string Secret { get; set; }
        }

    }
}
