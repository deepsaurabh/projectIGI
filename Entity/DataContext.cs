using Core;
using Entity;
using Entity.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;

namespace Data.Entity
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
