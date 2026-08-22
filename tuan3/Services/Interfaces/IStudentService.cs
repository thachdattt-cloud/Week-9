// Services/Interfaces/IStudentService.cs
using tuan3.Common.Pagination;
using tuan3.DTO;

namespace tuan3.Services.Interfaces
{
    public interface IStudentService
    {
        Task<List<StudentResponseDto>> GetAllAsync(string? keyword);
        Task<List<StudentWithClassDto>> GetAllWithClassAsync();
        Task<StudentResponseDto> GetByIdAsync(int id);
        Task<StudentResponseDto> CreateAsync(CreateStudentDto dto);
        Task<StudentResponseDto> UpdateAsync(int id, UpdateStudentDto dto);
        Task DeleteAsync(int id);
        Task<PagedResult<StudentResponseDto>> GetPageAsync(PaginationQuery query);
        Task<List<GradeDetailDto>> GetGradesDetailAsync();
    }
}