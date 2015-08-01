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
        IRepository<CourseDocument, Int64> CourseDocumentRepository { get; }
    }
}
