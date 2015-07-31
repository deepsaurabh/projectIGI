using Enterprise.Repository;
using Microsoft.AspNet.Identity.EntityFramework;
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
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            using (AuthRepository _repo = new AuthRepository())
            {
                IdentityUser user = await _repo.FindUser(context.UserName, context.Password);
                if (user == null)
                {
                    context.SetError("invalid_grant", "The username or password is incorrect.");
                    return;
                }
            }
            //context.OwinContext.Response.Headers.Add("Role", new[] { userRole });

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            //identity.AddClaim(new Claim("user_name", context.UserName));

            //foreach (var role in userRoles)
            //{
            //    identity.AddClaim(new Claim(ClaimTypes.Role, role.ToLower()));
            //}
            context.Validated(identity);
        }
    }
}