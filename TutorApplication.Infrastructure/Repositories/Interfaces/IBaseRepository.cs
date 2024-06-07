using System.Linq.Expressions;
using TutorApplication.SharedModels.Requests;
using TutorApplication.SharedModels.Responses;

namespace TutorApplication.Infrastructure.Repositories.Interfaces
{
	public interface IBaseRepository<T> where T : class
	{
		Task<T> GetItem(Expression<Func<T, bool>> query, string? includeProperties = null);
		Task<IEnumerable<T>> GetItems(Expression<Func<T, bool>> query, string? includeProperties = null);
		Task<PaginationResponse> GetPaginationItems(PaginationRequest request, Expression<Func<T, bool>> query, string? includeProperties = null);
		Task<bool> AddItem(T entity);
		Task AddItems(IEnumerable<T> entities);
	}
}
