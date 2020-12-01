using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IConfiguration _configuration { get; set; }

        // 控制器注入Configuration依赖, 方便获取appsettings.json的SecurityKey
        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("login")]
        public ActionResult Login(string userName, string passWord)
        {
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(passWord))
            {

                // token 中的claims用于存储自定义信息,如登录之后的用户Id等
                var claims = new[]
                {
                    new Claim("UserId", userName),
                };

                var authentication = _configuration.GetSection("Authentication");


                // 获取SecurityKey
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authentication["SecurityKey"]));


                var token = new JwtSecurityToken(
                    issuer: authentication["Issure"],        // 发布者
                    audience: authentication["Audience"],    // 接收者
                    notBefore: DateTime.Now,                 // token签发时间
                    expires: DateTime.Now.AddMinutes(30),    // token过期时间
                    claims: claims,                          // 该token内存存储的自定义字段信息
                    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256) // 用于签发token的密钥算法
                );
                // 返回成功信息, 并且写出token
                return Ok(new { code = 200, message = "登陆成功", data = new JwtSecurityTokenHandler().WriteToken(token) });
            }

            return BadRequest(new { code = 400, message = "登陆失败, 用户名或密码错误" });
        }
    }
}
