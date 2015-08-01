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

    public class CourseConfiguration : BaseEntityConfiguration<Course>
    {
        public CourseConfiguration() {
            this.HasMany(g => g.CourseDocument)
                    .WithRequired()
                    .HasForeignKey(ga => ga.CourseId);

        }

    }

    public class ToolkitConfiguration : BaseEntityConfiguration<Toolkit>
    {
        public ToolkitConfiguration() {
            this.HasMany(g => g.ToolkitDocument)
                        .WithRequired()
                        .HasForeignKey(ga => ga.ToolkitId);
        }

    }

    public class CourseDocumentConfiguration : BaseEntityConfiguration<CourseDocument>
    {
        public CourseDocumentConfiguration() { }

    }

    public class ToolkitDocumentConfiguration : BaseEntityConfiguration<ToolkitDocument>
    {
        public ToolkitDocumentConfiguration() { }

    }

}
