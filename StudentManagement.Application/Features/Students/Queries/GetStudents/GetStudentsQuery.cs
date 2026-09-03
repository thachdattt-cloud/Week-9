using MediatR;
using StudentManagement.Application.DTO;
using StudentManagement.Shared;
using StudentManagement.Shared.Common.Pagination;

namespace StudentManagement.Application.Features.Students.Queries.GetStudents;

public class GetStudentsQuery : IRequest<PagedResult<StudentResponseDto>>
{
    public string? Keyword { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}