using Core;
using Entity.POCO;
using System;
namespace Enterprise
{
    /// <summary>
    /// Unit of work interface
    /// Created By : Ashish Kumar
    /// Created On : 07/04/2014
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        void SaveChange();

        IRepository<User, Int64> UserRepository { get; }
        IRepository<Course, Int64> CourseRepository { get; }
        IRepository<Toolkit, Int64> ToolkitRepository { get; }
        IRepository<ToolkitDocument, Int64> ToolkitDocumentRepository { get; }
        IRepository<AttachedDocument, Int64> AttachedDocumentRepository { get; }
        IRepository<CourseAttachmentMapping, Int64> CourseAttachmentRepository { get; }
        IRepository<ToolkitAttachmentMapping, Int64> ToolkitAttachmentRepository { get; }
        IRepository<UserPurchaseHistory, Int64> UserPurchaseHistoryRepository { get; }
        IRepository<Cart, Int64> CartRepository { get; }
    }
}

