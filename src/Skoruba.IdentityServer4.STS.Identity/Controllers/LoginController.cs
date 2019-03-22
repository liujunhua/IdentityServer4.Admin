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
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict["client_id"] = model.ClientId;
            dict["client_secret"] = model.ClientSecret;
            dict["grant_type"] = model.GrantType;
            dict["username"] = model.UserName;
            dict["password"] = model.Password;
            var tokenUri = Configuration["IdentityService:TokenUri"];
            using (HttpClient http = HttpClientFactory.Create()) //; new HttpClient())
            using (var content = new FormUrlEncodedContent(dict))
            {
                var msg = await http.PostAsync(tokenUri, content);
                if (!msg.IsSuccessStatusCode)
                {
                    return StatusCode(Convert.ToInt32(msg.StatusCode));
                }

                string result = await msg.Content.ReadAsStringAsync();
                try
                {
                    var apiResult = await UserService.SetSessionInfo(new SearchDto() { Value = model.UserName });

                    HttpContext.Session.Set("UserInfo", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(apiResult.Data)));
                }
                catch (Exception ex)
                {
                    var temp = ex;
                    throw ex;
                }
                //var user = await UserManager.FindByNameAsync(model.UserName);

                return Content(result, "application/json");
            }
        }
    }
}