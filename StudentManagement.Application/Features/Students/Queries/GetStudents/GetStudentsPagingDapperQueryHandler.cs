using MediatR;
using StudentManagement.Application.DTO;
using StudentManagement.Application.Interfaces.Repositories;
using StudentManagement.Shared.Common.Pagination;

namespace StudentManagement.Application.Features.Students.Queries.GetStudentsPagingDapper;

public class GetStudentsPagingDapperQueryHandler : IRequestHandler<GetStudentsPagingDapperQuery, PagedResult<StudentPagingDto>>
{
    private readonly IStudentRepository _studentRepository;

    public GetStudentsPagingDapperQueryHandler(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public async Task<PagedResult<StudentPagingDto>> Handle(GetStudentsPagingDapperQuery request, CancellationToken cancellationToken)
    {
        return await _studentRepository.GetStudentsPagingByDapperAsync(request.PageIndex, request.PageSize);
    }
}