using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Dtos.Identity;
using Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Services.Interfaces;
using Skoruba.IdentityServer4.Admin.EntityFramework.DbContexts;
using Skoruba.IdentityServer4.Admin.EntityFramework.Identity.Entities.Identity;

namespace IdentityServer4.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("api/identityadmin/[controller]/[action]")]
    public class RoleController : Controller
    {

        private readonly IIdentityService<AdminDbContext, UserDto<int>, int, RoleDto<int>, int, int, int, UserIdentity, UserIdentityRole, int, UserIdentityUserClaim, UserIdentityUserRole, UserIdentityUserLogin, UserIdentityRoleClaim, UserIdentityUserToken> _identityService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identityService"></param>
        public RoleController(IIdentityService<AdminDbContext, UserDto<int>, int, RoleDto<int>, int, int, int, UserIdentity, UserIdentityRole, int, UserIdentityUserClaim, UserIdentityUserRole, UserIdentityUserLogin, UserIdentityRoleClaim, UserIdentityUserToken> identityService)
        {
            _identityService = identityService;
        }

    }
}