using System;
namespace Core
{
    /// <summary>
    /// Unit of work interface
    /// Created By : Ashish Kumar
    /// Created On : 07/04/2014
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        void SaveChange();
    }
}
