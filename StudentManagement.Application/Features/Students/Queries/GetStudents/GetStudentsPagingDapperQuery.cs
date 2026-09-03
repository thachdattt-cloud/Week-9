using MediatR;
using StudentManagement.Application.DTO;
using StudentManagement.Shared.Common.Pagination;

namespace StudentManagement.Application.Features.Students.Queries.GetStudentsPagingDapper;

public class GetStudentsPagingDapperQuery : IRequest<PagedResult<StudentPagingDto>>
{
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}