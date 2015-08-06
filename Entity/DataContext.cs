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
            
        }

    }

    public class CourseAttachmentMappingConfiguration : BaseEntityConfiguration<CourseAttachmentMapping>
    {
        public CourseAttachmentMappingConfiguration()
        {
            this.HasRequired(ch => ch.course).WithMany().HasForeignKey(ch
                => ch.courseId);

            this.HasRequired(ch => ch.courseDocument).WithMany().HasForeignKey(ch
                => ch.courseDocumentID);
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
        public CourseDocumentConfiguration() {
        }

    }

    public class ToolkitDocumentConfiguration : BaseEntityConfiguration<ToolkitDocument>
    {
        public ToolkitDocumentConfiguration() { }

    }

    public class UserPurchaseHistoryConfiguration : BaseEntityConfiguration<UserPurchaseHistory>
    {
        public UserPurchaseHistoryConfiguration() { }

    }

}
