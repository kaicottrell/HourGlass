using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
	public interface IGenericRepository<T> where T : class
	{
		#region IGenericRepository Get Methods
		//Get a single object by its key ID
		T GetById(int id);

		//used to get (SELECT/WHERE)
		T Get(Expression<Func<T, bool>> predicate, bool asNoTracking = false, string includes = null);

		//same as above
		Task<T> GetAsync(Expression<Func<T, bool>> predicate, bool asNoTracking = false, string includes = null);

		// Get enumerable list of objects that is queryable with ORDER BY and JOINs
		IEnumerable<T> GetAll(
			Expression<Func<T, bool>> filter = null,
			Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
			string includeProperties = null
			);
		#endregion

		#region IGenericRepository Get FirstOrDefault Method
		// Get first record from multiple records
		T GetFirstOrDefault(
			Expression<Func<T, bool>> filter = null,
			string includeProperties = null
			);
		#endregion

		#region IGenericRepository IEnumerable List Methods
		//returns an enumerable list of results
		IEnumerable<T> List();

		//Returns an enumerable list of results, expression is the query, optional order By and option string includes for joins.
		IEnumerable<T> List(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> orderBy = null, string includes = null);

		//Returns an enumerable list of results, expression is the query, optional order By and option string includes for joins.
		IEnumerable<T> ListByDate(Expression<Func<T, bool>> predicate, Expression<Func<T, DateTime>> orderBy = null, string includes = null);

		//same as above
		Task<IEnumerable<T>> ListAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> orderBy = null, string includes = null);
		#endregion

		#region IGenericRepository Add Method
		//Add (Insert) a new record instance
		void Add(T entity);
		#endregion

		#region IGenericRepository Delete Methods
		//Delete (Remove) a single record instance
		void Delete(T entity);

		//Delete (Remove) multiple record instances
		void Delete(IEnumerable<T> entities);
		#endregion

		#region IGenericRepository Update Method
		//Update all changes in an object
		void Update(T entity);
		#endregion
	}
}
