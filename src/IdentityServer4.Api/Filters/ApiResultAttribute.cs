using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace IdentityServer4.Api
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiResultAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var data = new ApiResult();
                data.IsSuccess = false;
                foreach (var item in context.ModelState.Values)
                {
                    foreach (var error in item.Errors)
                    {
                        data.ErrorMessage += $"{error.ErrorMessage}\r\n";
                    }
                }
                context.Result = SetApiResult(data);
            }

            List<string> list = new List<string>() { "Login", "GetVerificationCode", "Health" };
            if (list.IndexOf(context.RouteData.Values["action"].ToString()) == -1 && list.IndexOf(context.RouteData.Values["controller"].ToString()) == -1)
            {
                //if (SuperCodeContext.Current.User == null || SuperCodeContext.Current.User.Id == 0)
                //{
                //    var data = new ApiResult()
                //    {
                //        StatusCode = HttpStatusCode.Unauthorized,
                //        IsSuccess = false,
                //        ErrorMessage = "用户未登陆"
                //    };
                //    context.Result = SetApiResult(data);
                //    return;
                //}
            }
            base.OnActionExecuting(context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var result = new ApiResult();
            if (context.Exception == null)
            {
                switch (context.Result.GetType().Name)
                {
                    case "ContentResult":
                        result.StatusCode = (HttpStatusCode)context.HttpContext.Response.StatusCode;
                        result.IsSuccess = true;
                        result.Data = JsonConvert.DeserializeObject(((ContentResult)context.Result).Content);
                        break;
                    case "ObjectResult":
                        result.StatusCode = (HttpStatusCode)context.HttpContext.Response.StatusCode;
                        result.IsSuccess = true;
                        result.Data = ((ObjectResult)context.Result).Value;
                        break;
                    default:
                        return;
                }
            }
            else
            {
                #region 发生异常时的响应信息
                result.IsSuccess = false;
                result.StatusCode = HttpStatusCode.ExpectationFailed;
                result.ErrorMessage = context.Exception.Message;
                #endregion
            }
            context.Result = SetApiResult(result);
            //Todo 异常处理
            context.Exception = null;
        }

        // 设置 api action 响应内容
        private ContentResult SetApiResult(object data)
        {
            #region JSON 序列化自定义的响应结果
            var settings = new JsonSerializerSettings();
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            settings.Formatting = Formatting.Indented;
            settings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            var json = JsonConvert.SerializeObject(data, settings);
            var contentResult = new ContentResult
            {
                Content = json
            };
            #endregion
            return contentResult;
        }
    }
}
