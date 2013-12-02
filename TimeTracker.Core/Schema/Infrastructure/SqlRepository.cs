using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Linq.Expressions;
using TimeTracker.Core.Schema.Services;

namespace TimeTracker.Core.Schema.Infrastructure
{
	public class SqlRepository<T> : IRepository<T> where T : class
	{
		protected DbSet<T> _dbSet;
		protected DbContext _dbContext;

		public SqlRepository(DbSet<T> dbSet, DbContext dbContext)
		{
			_dbSet = dbSet;
			_dbContext = dbContext;
		}

		public IQueryable<T> FindAll()
		{
			return _dbSet;
		}

		public IQueryable<T> FindWhere(Expression<Func<T, bool>> predicate)
		{
			return _dbSet.Where(predicate);
		}

		public T Find(params object[] keyValues)
		{
			return _dbSet.Find(keyValues);
		}

		public void Add(T newEntity)
		{
			_dbSet.Add(newEntity);
		}

		public void Remove(T entity)
		{
			_dbSet.Remove(entity);
		}

		public void Attach(T entity, bool alreadyModified = false)
		{
			_dbSet.Attach(entity);

			if (alreadyModified)
			{
				_dbContext.Entry(entity).State = System.Data.Entity.EntityState.Modified;
			}
		}
	}
}
