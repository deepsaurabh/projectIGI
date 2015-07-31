
using Data.Enum;
using System;
using System.Web.Http.Controllers;

namespace API.Security
{
    /// <summary>
    /// This attribute is used to Authorize the user
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class AuthorizeEPRoleAttribute : System.Web.Http.AuthorizeAttribute
    {
        private AppRole[] _roles;
        public AuthorizeEPRoleAttribute(params AppRole[] roles)
        {
            _roles = roles;
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            //get the custom user and check the role if exist
            var user = actionContext.RequestContext.Principal;

            foreach (var role in _roles)
            {
                if (user.IsInRole(role.ToString()))
                {
                    return true;
                }
            }
            return false;
        }
    }

}