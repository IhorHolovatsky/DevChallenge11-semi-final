using NewsMonitoringSystem.UTIL;
using NewsMonitoringSystem.UTIL.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace NewsMonitoringSystem.API.Filters
{
    /// <summary>
    /// Validates Admin token in request header for secured actions
    /// </summary>
    [AttributeUsage(AttributeTargets.Class |
                    AttributeTargets.Method)]
    public class ValidateAdminTokenAttribute : ActionFilterAttribute
    {
        private static string _adminToken;
        protected static string AdminToken
        {
            get
            {
                return _adminToken ?? (_adminToken = ConfigUtils.GetAppSetting<string>(ConfigUtils.AppSettingKeys.AdminToken));
            }
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            IEnumerable<string> adminTokens = Enumerable.Empty<string>();
            actionContext.Request.Headers.TryGetValues(Constants.ADMIN_TOKEN_HEADER_NAME, out adminTokens);

            if ((adminTokens == null 
                 || !adminTokens.Any(a => a.Equals(AdminToken, StringComparison.OrdinalIgnoreCase)))
                && !actionContext.Request.GetQueryNameValuePairs().Any(q => q.Value.Equals(AdminToken, StringComparison.OrdinalIgnoreCase)))
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Invalid Token!");
            }

            base.OnActionExecuting(actionContext);
        }
    }
}
