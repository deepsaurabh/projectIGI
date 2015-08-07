using Enterprise.Repository;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace API.Security
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {


        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        //http://localhost:51473/token
        //grant_type=password&username=admin1&password=1234561
        //application/x-www-form-urlencoded
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            IdentityUser user = null;
            using (AuthRepository _repo = new AuthRepository())
            {
                var username = context.UserName;
                if (context.UserName.Contains("@"))
                {
                    var userForEmail = await _repo.FindByEmailAsync(context.UserName);
                    if (userForEmail != null)
                    {
                        username = userForEmail.UserName;
                    }
                }
                user = await _repo.FindUser(username, context.Password);

                if (user == null)
                {
                    context.SetError("invalid_grant", "The username or password is incorrect.");
                    return;
                }

                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim("user_name", context.UserName));

                var roleCollection = string.Empty;
                foreach (var role in user.Roles)
                {
                    var userRole = _repo.FindRole(role.RoleId);
                    roleCollection = string.IsNullOrEmpty(roleCollection) ? userRole.Name  : roleCollection + '-' + userRole.Name;
                    identity.AddClaim(new Claim(ClaimTypes.Role, userRole.Name));
                }

                AuthenticationProperties properties = CreateProperties(user.UserName, roleCollection);
                AuthenticationTicket ticket = new AuthenticationTicket(identity, properties);

                context.Validated(ticket);
            }
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(string userName, string Roles)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
        {
            { "userName", userName },
            {"roles",Roles}
        };
            return new AuthenticationProperties(data);
        }
    }
}