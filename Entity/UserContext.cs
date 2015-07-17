using Core;
using Entity;
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
    public class UserContext : BaseDataContext
    {
        public UserContext(string connectionName)
            : base(connectionName) { }
        public UserContext(IConnectionRetriever connectionRetriever) : this(connectionRetriever.GetConnectionStringName()) { }


        public class TestConfiguration : BaseEntityConfiguration<TestClass>
        {
            public TestConfiguration() { }
                
        }

        public override string GetSchemaName()
        {
            return "Test";
        }
    }

}
