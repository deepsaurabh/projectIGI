using Core;
using System.Data.Entity.ModelConfiguration;
public abstract class BaseEntityConfiguration<T> : EntityTypeConfiguration<T> where T : BaseEntity
{

}