using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiClient;
using WebApiClient.Attributes;

namespace Skoruba.IdentityServer4.STS.Identity
{
    public interface IUserService : IHttpApi
    {
        [JsonReturn]
        [HttpPost("/api/core/User/GetUser")]
        ITask<ApiResult> SetSessionInfo([JsonContent]SearchDto userName);

        [JsonReturn]
        [HttpPost("/api/core/User/Login")]
        ITask<ApiResult> Login([JsonContent]LoginUser loginUser);
    }

    [Serializable]
    public class LoginUser
    {
        /// <summary>
        /// 登录名
        /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string VerificationCode { get; set; }
    }

    [Serializable]
    public class SearchDto : ISearchDto<string>
    {
        public string Value { get; set; }
    }

    public interface ISearchDto<T>
    {
        T Value { get; set; }
    }

    [Serializable]
    public class SearchDto<T> : ISearchDto<T>
    {
        public T Value { get; set; }

        public SearchDto()
        {

        }

        public SearchDto(T value)
        {
            Value = value;
        }
    }
}
