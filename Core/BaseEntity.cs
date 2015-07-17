using System;

namespace Core
{
    public class BaseEntity : IBaseEntity
    {
        public virtual Int64 Id { get; set; }
        public virtual bool IsDeleted { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual DateTime UpdatedDate { get; set; }
    }

    public enum SortOrder
    {
        NONE = 0,
        ASC = 1,
        DESC = 2
    }
}
