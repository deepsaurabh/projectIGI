using Microsoft.AspNet.Identity.EntityFramework;

namespace Entity
{
    public class AuthContext : IdentityDbContext<IdentityUser>
    {
        public AuthContext()
            : base("IGI")
        {

        }
    }
}