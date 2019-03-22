using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Skoruba.IdentityServer4.STS.Identity
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiResult
    {
        /// <summary>
        /// 响应状态码
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// 业务返回数据
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 异常消息
        /// </summary>
        public string ErrorMessage { get; set; } = string.Empty;

        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool IsSuccess { get; set; }
    }
}
