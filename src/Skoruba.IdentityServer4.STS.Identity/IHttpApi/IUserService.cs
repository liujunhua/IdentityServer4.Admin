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
