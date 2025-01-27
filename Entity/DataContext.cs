﻿using Core;
using Entity.POCO;
using MySql.Data.Entity;
using System.Data.Entity;
using System.Data.Entity.Migrations.History;

namespace Entity
{
    /// <summary>
    /// Context class. This work as repository for all the classes
    /// Created By : Ashish Kumar
    /// Created On : 07/04/2014
    /// </summary>

    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class DataContext : BaseDataContext
    {
        public DataContext(string connectionName)
            : base(connectionName) { }
        public DataContext(IConnectionRetriever connectionRetriever) : this(connectionRetriever.GetConnectionStringName()) { }

        public override string GetSchemaName()
        {
            return "";
        }      
        
    }

    public class UserConfiguration : BaseEntityConfiguration<User>
    {
        public UserConfiguration() { }

    }

    public class CourseConfiguration : BaseEntityConfiguration<Course>
    {
        public CourseConfiguration()
        {

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

    public class ToolkitAttachmentMappingConfiguration : BaseEntityConfiguration<ToolkitAttachmentMapping>
    {
        public ToolkitAttachmentMappingConfiguration()
        {
            this.HasRequired(ch => ch.toolkit).WithMany().HasForeignKey(ch
                => ch.toolkitId);

            this.HasRequired(ch => ch.toolkitDocument).WithMany().HasForeignKey(ch
                => ch.toolkitDocumentID);
        }

    }

    public class ToolkitConfiguration : BaseEntityConfiguration<Toolkit>
    {
        public ToolkitConfiguration()
        {

        }

    }

    public class CourseDocumentConfiguration : BaseEntityConfiguration<AttachedDocument>
    {
        public CourseDocumentConfiguration()
        {
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

    public class CartConfiguration : BaseEntityConfiguration<Cart>
    {
        public CartConfiguration() { }

    }

    public class CheckOutAddressConfiguration : BaseEntityConfiguration<CheckOutAddress>
    {
        public CheckOutAddressConfiguration() { }

    }

}
