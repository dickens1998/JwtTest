using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtTest.Controllers
{
    [Route("api/[controller]")]
    // ApiController属性: 框架自动注入参数验证、model类型推断等功能
    [ApiController]
    // Authorize属性: 给控制器的所有动作加上权限验证,会跳过标注AllowAnonymous的方法
    [Authorize]
    public class ValuesController : ControllerBase
    {
        [HttpGet("no/auth")]
        [AllowAnonymous]
        public string Get()
        {
            return "这个方法不需要权限验证";
        }

        [HttpGet("auth")]
        public string Get2()
        {
            return "这个方法需要权限验证";
        }
    }
}
