using Dapper;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Application.DTO;
using StudentManagement.Application.Interfaces.Repositories;
using StudentManagement.Domain.Model;
using StudentManagement.Infrastructure.Context;
using StudentManagement.Shared.Common.Pagination;
using System.Data;
using System.Linq.Expressions;

namespace StudentManagement.Infrastructure.Repository.Implementations
{
    public class StudentRepository : IStudentRepository
    {
        private readonly AppDbContext _context;
        private readonly DapperContext _dapperContext;

        public StudentRepository(AppDbContext context, DapperContext dapperContext)
        {
            _context = context;
            _dapperContext = dapperContext;
        }

  
        private Expression<Func<Student, StudentResponseDto>> ToResponseDto()
        {
            return s => new StudentResponseDto
            {
                Id = s.StudentID,
                Name = s.FullName,
                StudentCode = s.StudentCode,
                Gender = s.Gender,
                Email = s.Email,
                ClassID = s.ClassID,
                Age = s.BirthDate.HasValue ? DateTime.Today.Year - s.BirthDate.Value.Year : 0
            };
        }
         
        public async Task<List<StudentResponseDto>> GetAllAsync(string? keyword)
        {
            var query = _context.Students.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(s => s.FullName.Contains(keyword));
            }

            return await query
                .OrderBy(s => s.StudentID)
                .Select(ToResponseDto())
                .ToListAsync();
        }

        public async Task<Student?> GetByIdAsync(int id)
        {
            return await _context.Students
                .FirstOrDefaultAsync(s => s.StudentID == id);
        }

        public async Task AddAsync(Student student)
        {
            await _context.Students.AddAsync(student);
        }

        public void Remove(Student student)
        {
            _context.Students.Remove(student);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<int> CountAsync(string? keyword)
        {
            var query = _context.Students.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(s => s.FullName.Contains(keyword));
            }

            return await query.CountAsync();
        }

        public async Task<List<StudentResponseDto>> GetPageAsync(string? keyword, int skipCount, int pageSize)
        {
            var query = _context.Students.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(s => s.FullName.Contains(keyword));
            }

            return await query
                .OrderBy(s => s.StudentID)
                .Skip(skipCount)
                .Take(pageSize)
                .Select(ToResponseDto())
                .ToListAsync();
        }

        public async Task<List<StudentWithClassDto>> GetAllWithClassAsync()
        {
            return await _context.Students 
                .AsNoTracking()
                .Select(s => new StudentWithClassDto
                {
                    StudentID = s.StudentID,
                    StudentCode = s.StudentCode,
                    FullName = s.FullName,
                    ClassName = s.Class.ClassName
                })
                .ToListAsync();
        }

        public async Task<List<GradeDetailDto>> GetGradesDetailAsync()
        {
            return await _context.StudentGrades
                .AsNoTracking()
                .Select(g => new GradeDetailDto
                {
                    StudentCode = g.Student.StudentCode,
                    StudentFullName = g.Student.FullName,
                    ClassName = g.Student.Class.ClassName,
                    SubjectName = g.Subject.SubjectName,
                    Mark = g.Mark
                })
                .ToListAsync();
        }

        public async Task<PagedResult<StudentPagingDto>> GetStudentsPagingByDapperAsync(int pageIndex, int pageSize)
        {
            using var connection = _dapperContext.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("PageIndex", pageIndex, DbType.Int32);
            parameters.Add("PageSize", pageSize, DbType.Int32);
            parameters.Add("TotalRecords", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var items = await connection.QueryAsync<StudentPagingDto>(
                "sp_GetStudentsPaging",
                parameters,
                commandType: CommandType.StoredProcedure);

            int totalRecords = parameters.Get<int>("TotalRecords");

            return new PagedResult<StudentPagingDto>
            {
                Items = items.ToList(),
                PageNumber = pageIndex,
                PageSize = pageSize,
                TotalItems = totalRecords
            };
        }
    }
}