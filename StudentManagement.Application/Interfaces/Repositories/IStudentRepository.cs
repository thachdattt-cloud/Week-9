using StudentManagement.Application.DTO;

using StudentManagement.Domain.Model;
using StudentManagement.Shared.Common.Pagination;

namespace StudentManagement.Application.Interfaces.Repositories
{
    public interface IStudentRepository
    {
        Task<List<StudentResponseDto>> GetAllAsync(string? keyword);
        Task<Student?> GetByIdAsync(int id);
        Task AddAsync(Student student);
        void Remove(Student student);
        Task SaveChangesAsync();

        Task<int> CountAsync(string? keyword);
        Task<List<StudentResponseDto>> GetPageAsync(string? keyword, int skipCount, int pageSize);
        Task<List<StudentWithClassDto>> GetAllWithClassAsync();
        Task<List<GradeDetailDto>> GetGradesDetailAsync();
        Task<PagedResult<StudentPagingDto>> GetStudentsPagingByDapperAsync(int pageIndex, int pageSize);
    }
}