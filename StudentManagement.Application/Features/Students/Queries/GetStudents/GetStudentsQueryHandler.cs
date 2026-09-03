using MediatR;
using StudentManagement.Application.DTO;
using StudentManagement.Application.Features.Students.Queries.GetStudents;
using StudentManagement.Application.Interfaces.Repositories;
using StudentManagement.Shared.Common.Pagination;

public class GetStudentsQueryHandler : IRequestHandler<GetStudentsQuery, PagedResult<StudentResponseDto>>
{
    private readonly IStudentRepository _studentRepository;

    public GetStudentsQueryHandler(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public async Task<PagedResult<StudentResponseDto>> Handle(GetStudentsQuery request, CancellationToken cancellationToken)
    {
        int skipCount = (request.PageNumber - 1) * request.PageSize;

        var items = await _studentRepository.GetPageAsync(request.Keyword, skipCount, request.PageSize);
        var totalItems = await _studentRepository.CountAsync(request.Keyword);

        return new PagedResult<StudentResponseDto>
        {
            Items = items,
            TotalItems = totalItems,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}