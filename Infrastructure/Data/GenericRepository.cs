using Microsoft.EntityFrameworkCore;
using ApplicationCore.Models;
using ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
	public class GenericRepository<T> : IGenericRepository<T> where T : class
	{
		#region GenericRepository Constructor
		private readonly ApplicationDbContext _dbContext;
		internal DbSet<T> dbSet;

		public GenericRepository(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		#endregion

		#region Add Methods
		public void Add(T entity)
		{
			_dbContext.Set<T>().Add(entity);
			_dbContext.SaveChanges();
		}
		#endregion

		#region Delete Methods
		public void Delete(T entity)
		{
			_dbContext.Set<T>().Remove(entity);
			_dbContext.SaveChanges();
		}

		public void Delete(IEnumerable<T> entities)
		{
			_dbContext.Set<T>().RemoveRange(entities);
			_dbContext.SaveChanges();
		}
		#endregion

		#region Get Methods
		public virtual T Get(Expression<Func<T, bool>> predicate, bool asNoTracking = false, string includes = null)
		{
			if (includes == null)//no tables to join, single object
			{
				if (asNoTracking)//read only copy for display
				{
					return _dbContext.Set<T>()
						.AsNoTracking()
						.Where(predicate)
						.FirstOrDefault();
				}
				else //needs to be tracked, as tracking is on
				{
					return _dbContext.Set<T>()
						.Where(predicate)
						.FirstOrDefault();
				}
			}
			else //this has includes (other objects or tables)
			{
				IQueryable<T> queryable = _dbContext.Set<T>();
				foreach (var includeProperty in includes.Split(new char[]
					{','}, StringSplitOptions.RemoveEmptyEntries))

				{
					queryable = queryable.Include(includeProperty);
				}
				if (asNoTracking)
				{
					return queryable
							.AsNoTracking()
							.Where(predicate)
							.FirstOrDefault();
				}
				else
				{
					return queryable
						   .Where(predicate)
						   .FirstOrDefault();
				}
			}
		}

		public virtual async Task<T> GetAsync(Expression<Func<T, bool>> predicate, bool asNoTracking = false, string includes = null)
		{
			if (includes == null)//no tables to join, single object
			{
				if (asNoTracking)//read only copy for display
				{
					return await _dbContext.Set<T>()
						.AsNoTracking()
						.Where(predicate)
						.FirstOrDefaultAsync();
				}
				else //needs to be tracked, as tracking is on
				{
					return await _dbContext.Set<T>()
						.Where(predicate)
						.FirstOrDefaultAsync();
				}
			}
			else //this has includes (other objects or tables)
			{
				IQueryable<T> queryable = _dbContext.Set<T>();
				foreach (var includeProperty in includes.Split(new char[]
					{','}, StringSplitOptions.RemoveEmptyEntries))

				{
					queryable = queryable.Include(includeProperty);
				}
				if (asNoTracking)
				{
					return await queryable
							.AsNoTracking()
							.Where(predicate)
							.FirstOrDefaultAsync();
				}
				else
				{
					return await queryable
						   .Where(predicate)
						   .FirstOrDefaultAsync();
				}
			}
		}

		public T GetById(int id)
		{
			return _dbContext.Set<T>().Find(id);
		}


		public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = null)
		{
			IQueryable<T> query = dbSet;
			if (filter != null)
			{
				query = query.Where(filter);
			}
			// Include properties will be comma-separated
			if (includeProperties != null)
			{
				foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
				{
					query = query.Include(includeProperty);
				}
			}
			if (orderBy != null)
			{
				return orderBy(query).ToList();
			}
			return query.ToList();
		}

		public T GetFirstOrDefault(Expression<Func<T, bool>> filter = null, string includeProperties = null)
		{
			IQueryable<T> query = dbSet;
			if (filter != null)
			{
				query = query.Where(filter);
			}
			// Include properties will be comma separated
			if (includeProperties != null)
			{
				foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
				{
					query = query.Include(includeProperty);
				}
			}
			return query.FirstOrDefault();
		}
		#endregion

		#region IEnumerable List Methods
		public virtual IEnumerable<T> List()
		{
			return _dbContext.Set<T>().ToList().AsEnumerable();
		}

		public virtual IEnumerable<T> List(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> orderBy = null, string includes = null)
		{
			IQueryable<T> queryable = _dbContext.Set<T>();
			if (predicate != null && includes == null) //doesnt have a where
			{
				return _dbContext.Set<T>()
					.Where(predicate)
					.ToList()
					.AsEnumerable();
			}
			else if (includes != null) //contains joins
			{
				foreach (var includeProperty in includes.Split(new char[]
					{','}, StringSplitOptions.RemoveEmptyEntries))
				{
					queryable = queryable.Include(includeProperty);
				}
			}
			if (predicate == null)
			{
				if (orderBy == null)
				{
					return queryable.ToList().AsEnumerable();
				}
				else
				{
					return queryable.OrderBy(orderBy).ToList().AsEnumerable();
				}
			}
			else
			{
				if (orderBy == null)
				{
					return queryable.Where(predicate).ToList().AsEnumerable();
				}
				else
				{
					return queryable.Where(predicate).OrderBy(orderBy).ToList().AsEnumerable();
				}
			}
		}

		public virtual IEnumerable<T> ListByDate(Expression<Func<T, bool>> predicate, Expression<Func<T, DateTime>> orderBy = null, string includes = null)
		{
			IQueryable<T> queryable = _dbContext.Set<T>();
			if (predicate != null && includes == null) //doesnt have a where
			{
				return _dbContext.Set<T>()
					.Where(predicate)
					.ToList()
					.AsEnumerable();
			}
			else if (includes != null) //contains joins
			{
				foreach (var includeProperty in includes.Split(new char[]
					{','}, StringSplitOptions.RemoveEmptyEntries))
				{
					queryable = queryable.Include(includeProperty);
				}
			}
			if (predicate == null)
			{
				if (orderBy == null)
				{
					return queryable.ToList().AsEnumerable();
				}
				else
				{
					return queryable.OrderBy(orderBy).ToList().AsEnumerable();
				}
			}
			else
			{
				if (orderBy == null)
				{
					return queryable.Where(predicate).ToList().AsEnumerable();
				}
				else
				{
					return queryable.Where(predicate).OrderBy(orderBy).ToList().AsEnumerable();
				}
			}
		}

		public virtual async Task<IEnumerable<T>> ListAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> orderBy = null, string includes = null)
		{
			IQueryable<T> queryable = _dbContext.Set<T>();
			if (predicate != null && includes == null) //doesnt have a where
			{
				return await _dbContext.Set<T>()
					.Where(predicate)
					.ToListAsync();
			}
			else if (includes != null) //contains joins
			{
				foreach (var includeProperty in includes.Split(new char[]
					{','}, StringSplitOptions.RemoveEmptyEntries))
				{
					queryable = queryable.Include(includeProperty);
				}
			}
			if (predicate == null)
			{
				if (orderBy == null)
				{
					return await queryable.ToListAsync();
				}
				else
				{
					return await queryable.OrderBy(orderBy).ToListAsync();
				}
			}
			else
			{
				if (orderBy == null)
				{
					return await queryable.Where(predicate).ToListAsync();
				}
				else
				{
					return await queryable.Where(predicate).OrderBy(orderBy).ToListAsync();
				}
			}
		}
		#endregion

		#region Update Methods
		public void Update(T entity)
		{
			_dbContext.Entry(entity).State = EntityState.Modified;
			_dbContext.SaveChanges();
		}
		#endregion
	}
}
