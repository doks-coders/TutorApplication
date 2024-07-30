using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TutorApplication.Infrastructure.Data;
using TutorApplication.Infrastructure.Repositories.Interfaces;
using TutorApplication.SharedModels.Entities;
using TutorApplication.SharedModels.Models;
using TutorApplication.SharedModels.Requests;
using TutorApplication.SharedModels.Responses;

namespace TutorApplication.Infrastructure.Repositories
{
	public class CourseStudentRepository : BaseRepository<CourseStudent>, ICourseStudentRepository
	{
		private readonly ApplicationDbContext _context;
		public CourseStudentRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task<IEnumerable<CourseStudent>> GetOneCourseStudentForStudent(Guid studentId)
		{
			return await _context.CourseStudents.Where(u => u.StudentId == studentId)
				.Include(u => u.Course)
				.ThenInclude(u => u.Students)
				.Include(u => u.Course)
				.ThenInclude(u => u.Tutor)
				.ToListAsync();
		}

		public async Task<IEnumerable<CourseStudent>> GetOneCourseStudentForTutor(Guid tutorId)
		{
			return await _context.CourseStudents.Where(u => u.TutorId == tutorId)
				.Include(u => u.Course)
				.ThenInclude(u => u.Students)
				.Include(u => u.Course)
				.ThenInclude(u => u.Tutor)
				.ToListAsync();
		}

		public async Task<PaginationResponse> GetTutorsPaginationItems(PaginationRequest request, Expression<Func<CourseStudent, bool>> query, string? includeProperties = null)
		{
			try
			{
				var totalNumber = await _dbSet.Where(query).Include(u=>u.Tutor).GroupBy(u=>u.TutorId).Select(e => e.First()).CountAsync();

				var limit = request.PageLimit;
				var page = request.PageNumber;
				var skipValue = limit * (page - 1);


				var totalPages = Math.Ceiling(totalNumber / (decimal)limit);
				var q = _dbSet.AsQueryable();
				q = IncludeProperties(q, includeProperties);

				var pagedValues = q.Where(query).Include(u => u.Tutor).Select(e=>e.Tutor).GroupBy(u => u.Id).Select(e => e.First())
					.Skip(skipValue).Take(limit).ToList();

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


		public async Task<PaginationResponse> GetStudentsPaginationItems(PaginationRequest request, Expression<Func<CourseStudent, bool>> query, string? includeProperties = null)
		{
			try
			{
				var totalNumber = await _dbSet.Where(query).Include(u => u.Student).GroupBy(u => u.StudentId).Select(e => e.First()).CountAsync();

				var limit = request.PageLimit;
				var page = request.PageNumber;
				var skipValue = limit * (page - 1);


				var totalPages = Math.Ceiling(totalNumber / (decimal)limit);
				var q = _dbSet.AsQueryable();
				q = IncludeProperties(q, includeProperties);

				var pagedValues = q.Where(query).Include(u => u.Student).Select(e => e.Student).GroupBy(u => u.Id).Select(e => e.First())
					.Skip(skipValue).Take(limit).ToList();

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



		public async Task<PaginationResponse> GetCoursePaginationItems(PaginationRequest request, Expression<Func<CourseStudent, bool>> query, string? includeProperties = null)
		{
			try
			{
				var totalNumber = await _dbSet.Where(query).Include(u => u.Course).GroupBy(u => u.CourseId).Select(e => e.First()).CountAsync();

				var limit = request.PageLimit;
				var page = request.PageNumber;
				var skipValue = limit * (page - 1);


				var totalPages = Math.Ceiling(totalNumber / (decimal)limit);
				var q = _dbSet.AsQueryable();
				q = IncludeProperties(q, includeProperties);

				var pagedValues = q.Where(query).Include(u => u.Course).ThenInclude(u=>u.Tutor)
					.Include(u => u.Course).ThenInclude(u => u.Students)
					.Select(e => e.Course).GroupBy(u => u.Id).Select(e => e.First())
					.Skip(skipValue).Take(limit).ToList();

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


	}
}
