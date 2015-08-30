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
        private IRepository<AttachedDocument, Int64> _courseDocumentRepository;
        private IRepository<ToolkitDocument, Int64> _toolkitDocumentRepository;        
        private IRepository<UserPurchaseHistory, Int64> _userPurchaseHistoryRepository;
        private IRepository<CourseAttachmentMapping, Int64> _courseAttachmentRepository;
        private IRepository<ToolkitAttachmentMapping, Int64> _toolkitAttachmentRepository;
        private IRepository<Cart, Int64> _cartRepository;
  
        #endregion

        #region "RepositoryProperties"

        //Prepare the property for user profile repository

        public IRepository<Cart, Int64> CartRepository
        {
            get
            {
                if (this._cartRepository == null)
                {
                    this._cartRepository = new GenericRepository<Cart, Int64>(UserContext);
                }
                return _cartRepository;
            }
        }
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

        public IRepository<CourseAttachmentMapping, Int64> CourseAttachmentRepository
        {
            get
            {
                if (this._courseAttachmentRepository == null)
                {
                    this._courseAttachmentRepository = new GenericRepository<CourseAttachmentMapping, Int64>(UserContext);
                }
                return _courseAttachmentRepository;
            }
        }

        public IRepository<ToolkitAttachmentMapping, Int64> ToolkitAttachmentRepository
        {
            get
            {
                if (this._toolkitAttachmentRepository == null)
                {
                    this._toolkitAttachmentRepository = new GenericRepository<ToolkitAttachmentMapping, Int64>(UserContext);
                }
                return _toolkitAttachmentRepository;
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

        public IRepository<AttachedDocument, Int64> AttachedDocumentRepository
        {
            get
            {
                if (this._courseDocumentRepository == null)
                {
                    this._courseDocumentRepository = new GenericRepository<AttachedDocument, Int64>(UserContext);
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

        public IRepository<UserPurchaseHistory, Int64> UserPurchaseHistoryRepository
        {
            get
            {
                if (this._userPurchaseHistoryRepository == null)
                {
                    this._userPurchaseHistoryRepository = new GenericRepository<UserPurchaseHistory, Int64>(UserContext);
                }
                return _userPurchaseHistoryRepository;
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
