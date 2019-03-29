using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Skoruba.IdentityServer4.Admin.EntityFramework.Identity.Entities.Identity;

namespace Skoruba.IdentityServer4.STS.Identity.Controllers
{
    /// <summary>
    /// 登录服务
    /// </summary>
    [Produces("application/json")]
    [Route("Identity/Login")]
    public class LoginController : Controller
    {
        private IConfiguration Configuration { get; }

        private readonly UserManager<UserIdentity> UserManager;

        private readonly IUserService UserService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_configuration"></param>
        public LoginController(IConfiguration _configuration, UserManager<UserIdentity> _userManager, IUserService _userService)
        {
            Configuration = _configuration;
            UserManager = _userManager;
            UserService = _userService;
        }

        /// <summary>
        /// 登录接口
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> RequestToken([FromBody]LoginRequestParam model)
        {
            //byte[] verificationCode;
            //if (!HttpContext.Session.TryGetValue("VerificationCode", out verificationCode))
            //{
            //    throw new Exception("验证码已失效");
            //}
            //if (!Encoding.UTF8.GetString(verificationCode).ToLower().Equals(model.VerificationCode.ToLower()))
            //{
            //    throw new Exception("验证码不正确");
            //}

            var apiResult = await UserService.Login(new LoginUser() { LoginName = model.UserName, Password = model.Password, VerificationCode = "" });
            var result = JsonConvert.SerializeObject(new LoginResult() { access_token = apiResult.Data.ToString() });

            return Content(result, "application/json");
        }
    }

    public class LoginResult
    {
        public string access_token { get; set; }

        public string token_type { get; set; } = "Bearer";

        public int expires_in { get; set; } = 3600;

    }
}