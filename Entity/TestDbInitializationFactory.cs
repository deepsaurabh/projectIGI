using Data.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class TestDbInitializationFactory : IDbContextFactory<UserContext>
    {
        public UserContext Create()
        {
            return new UserContext("Random");
        }
    }
}
