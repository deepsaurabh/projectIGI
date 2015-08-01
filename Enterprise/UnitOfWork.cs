using Core;
using Enterprise.Repository;
using Entity;
using Entity.POCO;
using System;

namespace Enterprise
{
    /// <summary>
    /// Set the unit of work
    /// Created By : Ashish Kumar
    /// Created On : 07/04/2014
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        //Prepare the variables here
        internal readonly DataContext UserContext = new DataContext(new ConnectionRetriever().GetConnectionStringName());

        private bool _disposed;

        #region "RepositoryLocalVariables"
        
        //prepare the repositoy label variables here
        private IRepository<User, Int64> _userRepository;
        private IRepository<Course, Int64> _courseRepository;
        private IRepository<Toolkit, Int64> _toolkitRepository;
        private IRepository<CourseDocument, Int64> _courseDocumentRepository;
        private IRepository<ToolkitDocument, Int64> _toolkitDocumentRepository;
  
        #endregion

        #region "RepositoryProperties"

        //Prepare the property for user profile repository
        public IRepository<User, Int64> UserRepository
        {
            get
            {
                if (this._userRepository == null)
                {
                    this._userRepository = new GenericRepository<User, Int64>(UserContext);
                }
                return _userRepository;
            }
        }

        public IRepository<Course, Int64> CourseRepository
        {
            get
            {
                if (this._courseRepository == null)
                {
                    this._courseRepository = new GenericRepository<Course, Int64>(UserContext);
                }
                return _courseRepository;
            }
        }

        public IRepository<Toolkit, Int64> ToolkitRepository
        {
            get
            {
                if (this._toolkitRepository == null)
                {
                    this._toolkitRepository = new GenericRepository<Toolkit, Int64>(UserContext);
                }
                return _toolkitRepository;
            }
        }

        public IRepository<CourseDocument, Int64> CourseDocumentRepository
        {
            get
            {
                if (this._courseDocumentRepository == null)
                {
                    this._courseDocumentRepository = new GenericRepository<CourseDocument, Int64>(UserContext);
                }
                return _courseDocumentRepository;
            }
        }

        public IRepository<ToolkitDocument, Int64> ToolkitDocumentRepository
        {
            get
            {
                if (this._toolkitDocumentRepository == null)
                {
                    this._toolkitDocumentRepository = new GenericRepository<ToolkitDocument, Int64>(UserContext);
                }
                return _toolkitDocumentRepository;
            }
        }

        #endregion

        #region "ContextSave"

        /// <summary>
        /// Save the changes here
        /// </summary>
        public void SaveChange()
        {
            UserContext.SaveChanges();
        }
        #endregion

        #region "CleanUpCode"

        //To Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

       //To Dispose
        public virtual void Dispose(bool disposing)
        {
            if (!_disposed & disposing)
                    UserContext.Dispose();

            _disposed = true;
        }

       ~UnitOfWork()
       {
           Dispose(false);
       }

        #endregion

    }
}
