using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;

namespace Core
{
  public abstract class BaseDataContext : DbContext
    {

        public BaseDataContext(string connectionString) : base(connectionString) { }

        public BaseDataContext(IConnectionRetriever connectionRetriever) : base(connectionRetriever.GetConnectionStringName()) { }

        public abstract string GetSchemaName();

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Properties().Where(p => p.Name.Equals("Id")).Configure(p => p.IsKey().HasColumnName(TableColumnNameHelper.GetColumnNamePrependedByEntityName(p.ClrPropertyInfo)));

            modelBuilder.Properties().Where(p => p.Name.Equals("RowVersion")).Configure(p => p.IsRowVersion().HasColumnName("RowVersion"));

            modelBuilder.Types().Configure(t => t.ToTable(TableColumnNameHelper.GetTableName(t.ClrType)));
            modelBuilder.HasDefaultSchema(GetSchemaName());
            AddConfigurations(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        private void AddConfigurations(DbModelBuilder modelBuilder)
        {
            var entityConfigurationTypes = FindTypesToRegister();
            entityConfigurationTypes.ToList().ForEach(entityConfigurationType =>
            {
                dynamic configurationInstance = Activator.CreateInstance(entityConfigurationType);
                modelBuilder.Configurations.Add(configurationInstance);
            });

        }

        private IEnumerable<Type> FindTypesToRegister()
        {
            //TODO: More robust mechanism to discover the types
            var assembly = GetType().Assembly;
            var typesToRegister = assembly.GetTypes()
            .Where(type => type.Namespace != null && type.Namespace.Equals(GetType().Namespace))
            .Where(type => type.BaseType.IsGenericType &&
                           (type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>)
                            || type.BaseType.GetGenericTypeDefinition() == typeof(BaseEntityConfiguration<>)));
            return typesToRegister;
        }


    }
}
