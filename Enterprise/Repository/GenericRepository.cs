﻿using System;
using System.Collections.Generic;
using System.Linq;

using System.Data.Entity;
using Entity;
using Core;

namespace Enterprise.Repository
{
   public class GenericRepository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : BaseEntity
    {
       //Set the variables here
        internal DataContext Context;
        internal DbSet<TEntity> DbSet;

       //Prepare the environment here
        public GenericRepository(DataContext context)
        {
            this.Context = context;
            this.DbSet = context.Set<TEntity>();
        }

       /// <summary>
       /// TO get all the records
       /// </summary>
       /// <param name="filter">Get the filter expression</param>
       /// <param name="orderBy">Get the order by parameters</param>
       /// <param name="includeProperties">Get the include properties</param>
       /// <returns>return the list of records</returns>
        public IEnumerable<TEntity> Get(System.Linq.Expressions.Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, 
                                                                IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "")
        {
            IQueryable<TEntity> query = DbSet;
            if (filter != null)
            {
                query = query.Where(filter).Where(m => !m.IsDeleted);
            }
            
            foreach (var includeProperty in includeProperties.Split
                (new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }


            return query.ToList();
        }

       /// <summary>
       /// Get the entity by id
       /// </summary>
       /// <param name="id"></param>
       /// <returns></returns>
        public TEntity GetById(TKey id)
        {
            return DbSet.Find(id);
        }

       /// <summary>
       /// Insert the record
       /// </summary>
       /// <param name="entityToAdd">Get the entity to insert</param>
        public void Insert(TEntity entityToAdd)
        {
            entityToAdd.CreatedDate = DateTime.UtcNow;
            entityToAdd.UpdatedDate = DateTime.UtcNow;
            DbSet.Add(entityToAdd);
        }

        /// <summary>
        /// Update the record
        /// Created By : Ashish Kumar
        /// </summary>
        /// <param name="entityToUpdate">Get the entity to update</param>
        public void Update(TEntity entityToUpdate)
        {
            entityToUpdate.UpdatedDate = DateTime.UtcNow;
            DbSet.Attach(entityToUpdate);
            Context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        /// <summary>
        /// Delete the entity
        /// Created By : Ashish Kumar
        /// </summary>
        /// <param name="id">Get the id to delete</param>
        public void Delete(TKey id)
        {
            TEntity entityToDelete = DbSet.Find(id);           
            if (Context.Entry(entityToDelete).State == EntityState.Detached)
            {
                DbSet.Attach(entityToDelete);
            }
            
            DbSet.Remove(entityToDelete);
        }

        public void SoftDelete(TKey id)
        {
            TEntity entityToDelete = DbSet.Find(id);
            entityToDelete.IsDeleted = true;
        }
    }
}
