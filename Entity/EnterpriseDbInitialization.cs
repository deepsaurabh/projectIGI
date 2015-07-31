using Core;
using System.Data.Entity.Infrastructure;

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
