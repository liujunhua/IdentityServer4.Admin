using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skoruba.IdentityServer4.STS.Identity.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class LoginRequestParam
    {

        /// <summary>
        /// 
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// 
        /// </summary>

        public string ClientSecret { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string GrantType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string VerificationCode { get; set; }

    }
}
