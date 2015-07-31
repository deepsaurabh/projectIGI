using Core;
using Entity.POCO;

namespace Entity
{
    /// <summary>
    /// Context class. This work as repository for all the classes
    /// Created By : Ashish Kumar
    /// Created On : 07/04/2014
    /// </summary>
    public class DataContext : BaseDataContext
    {
        public DataContext(string connectionName)
            : base(connectionName) { }
        public DataContext(IConnectionRetriever connectionRetriever) : this(connectionRetriever.GetConnectionStringName()) { }

        public override string GetSchemaName()
        {
            return "Enterprise";
        }
    }

    public class UserConfiguration : BaseEntityConfiguration<User>
    {
        public UserConfiguration() { }

    }

}
