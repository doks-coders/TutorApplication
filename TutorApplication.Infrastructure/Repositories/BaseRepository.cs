using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TutorApplication.Infrastructure.Data;
using TutorApplication.Infrastructure.Repositories.Interfaces;
using TutorApplication.SharedModels.Models;
using TutorApplication.SharedModels.Requests;
using TutorApplication.SharedModels.Responses;

namespace TutorApplication.Infrastructure.Repositories
{
	public class BaseRepository<T> : IBaseRepository<T> where T : class
	{
		internal readonly DbSet<T> _dbSet;

		public BaseRepository(ApplicationDbContext context)
		{
			_dbSet = context.Set<T>();
		}

		public async Task<T> GetItem(Expression<Func<T, bool>> query, string? includeProperties = null)
		{
			var dbSetQueryable = _dbSet.AsQueryable();
			dbSetQueryable = IncludeProperties(dbSetQueryable, includeProperties);
			return await dbSetQueryable.Where(query).FirstOrDefaultAsync();

		}
		public async Task<IEnumerable<T>> GetItems(Expression<Func<T, bool>> query, string? includeProperties = null)
		{
			var dbSetQueryable = _dbSet.AsQueryable();
			dbSetQueryable = IncludeProperties(dbSetQueryable, includeProperties);
			return await dbSetQueryable.Where(query).ToListAsync(); ;
		}




		public async Task<PaginationResponse> GetPaginationItems(PaginationRequest request, Expression<Func<T, bool>> query, string? includeProperties = null)
		{
			try
			{
				var totalNumber = await _dbSet.Where(query).CountAsync();

				var limit = request.PageLimit;
				var page = request.PageNumber;
				var skipValue = limit * (page - 1);


				var totalPages = Math.Ceiling(totalNumber / (decimal)limit);
				var q = _dbSet.AsQueryable();
				q = IncludeProperties(q, includeProperties);

				var pagedValues = q.Where(query).Skip(skipValue).Take(limit).ToList();

				return new PaginationResponse()
				{
					Items = pagedValues,
					PageNumber = page,
					TotalPages = (int)totalPages,
					TotalItems = totalNumber
				};

			}
			catch (Exception ex)
			{
				throw new CustomException(ex.Message);
			}

		}

		public async Task<bool> AddItem(T entity)
		{
			var res = await _dbSet.AddAsync(entity);
			return res.State.HasFlag(EntityState.Added);
		}

		public async Task AddItems(IEnumerable<T> entities)
		{
			await _dbSet.AddRangeAsync(entities);
		}

		public Task<bool> DeleteItem(T entity)
		{
			var res = _dbSet.Remove(entity);
			return Task.FromResult(res.State.HasFlag(EntityState.Deleted));
		}

		public Task DeleteItems(IEnumerable<T> entities)
		{
			_dbSet.RemoveRange(entities);
			return Task.CompletedTask;

		}





		internal IQueryable<T> IncludeProperties(IQueryable<T> dbSetQueryable, string includeProperties)
		{
			if (includeProperties != null)
			{
				var properties = includeProperties.Split(",", StringSplitOptions.RemoveEmptyEntries);
				foreach (var property in properties)
				{
					dbSetQueryable = dbSetQueryable.Include(property);
				}
			}
			return dbSetQueryable;
		}
	}
}
