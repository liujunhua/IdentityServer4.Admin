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
    public class UserController : Controller
    {
        private readonly IIdentityService<AdminDbContext, UserDto<int>, int, RoleDto<int>, int, int, int, UserIdentity, UserIdentityRole, int, UserIdentityUserClaim, UserIdentityUserRole, UserIdentityUserLogin, UserIdentityRoleClaim, UserIdentityUserToken> _identityService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identityService"></param>
        public UserController(IIdentityService<AdminDbContext, UserDto<int>, int, RoleDto<int>, int, int, int, UserIdentity, UserIdentityRole, int, UserIdentityUserClaim, UserIdentityUserRole, UserIdentityUserLogin, UserIdentityRoleClaim, UserIdentityUserToken> identityService)
        {
            _identityService = identityService;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userPassword"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> ChangePassword(UserChangePasswordDto<int> userPassword)
        {
            var identityResult = await _identityService.UserChangePasswordAsync(userPassword);
            if (!identityResult.Errors.Any())
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 用户保存
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> Save(UserDto<int> userDto)
        {
            int userId;
            if (EqualityComparer<int>.Default.Equals(userDto.Id, default))
            {
                var userData = await _identityService.CreateUserAsync(userDto);
                userId = userData.userId;
            }
            else
            {
                var userData = await _identityService.UpdateUserAsync(userDto);
                userId = userData.userId;
            }
            return userId;
        }

        /// <summary>
        /// 增加角色
        /// </summary>
        /// <param name="userRole"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> CreateUserRole(UserRolesDto<RoleDto<int>, int, int> userRole)
        {
            await _identityService.CreateUserRoleAsync(userRole);
            return true;
        }

    }
}