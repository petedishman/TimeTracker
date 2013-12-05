using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace TimeTracker.Core.Schema.Services
{
	public interface IRepository<T>
		where T : class
	{
		IQueryable<T> FindAll();
		IQueryable<T> FindWhere(Expression<Func<T, bool>> predicate);
		T Find(params object[] keyValues);

		void Add(T newEntity);
		void Remove(T entity);

		void Attach(T entity, bool alreadyModified = false);
	}
}
