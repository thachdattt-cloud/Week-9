using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using tuan3.Context;
using tuan3.DTO;
using tuan3.Model;
using tuan3.Repository.Interfaces;

namespace tuan3.Repository.Implementations
{
    public class StudentRepository : IStudentRepository
    {
        private readonly AppDbContext _context;

        public StudentRepository(AppDbContext context)
        {
            _context = context;
        }

        // Projection dat truc tiep trong query -> EF Core dich thanh SQL
        // chi lay dung cot can dung, khong keo full entity ve RAM.
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
            var grades = await _context.StudentGrades
                .AsNoTracking()
                .Include(g => g.Student)
                    .ThenInclude(s => s.Class)
                .Include(g => g.Subject)
                .ToListAsync();

            var result = new List<GradeDetailDto>();
            foreach (var g in grades)
            {
                var dto = new GradeDetailDto();
                dto.StudentCode = g.Student.StudentCode;
                dto.StudentFullName = g.Student.FullName;
                dto.ClassName = g.Student.Class.ClassName;
                dto.SubjectName = g.Subject.SubjectName;
                dto.Mark = g.Mark;
                result.Add(dto);
            }

            return result;
        }
    }
}