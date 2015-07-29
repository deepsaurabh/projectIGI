using Core;
using Data.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class EnterpriseDbInitialization : IDbContextFactory<DataContext>
    {
        public DataContext Create()
        {
            return new DataContext(new ConnectionRetriever().GetConnectionStringName());
        }
    }
}
