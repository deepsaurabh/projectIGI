using Core;
using Data.Entity;
using Entity;
using Entity.Entity;
using Interfaces.IBusiness;
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
        private IRepository<User, int> _testokenRepository;
  
        #endregion

        #region "RepositoryProperties"

        //Prepare the property for user profile repository
        public IRepository<User, int> UserProfileRepository
        {
            get 
            {
                if (this._testokenRepository == null)
                {
                    this._testokenRepository = new GenericRepository<User, int>(UserContext);
                }
                return _testokenRepository;
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
