using MediatR;
using StudentManagement.Application.DTO;
using StudentManagement.Application.Interfaces.Repositories;

namespace StudentManagement.Application.Features.Students.Queries.GetAllStudents;

public class GetAllStudentsQueryHandler : IRequestHandler<GetAllStudentsQuery, List<StudentResponseDto>>
{
    private readonly IStudentRepository _studentRepository;

    public GetAllStudentsQueryHandler(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public async Task<List<StudentResponseDto>> Handle(GetAllStudentsQuery request, CancellationToken cancellationToken)
    {
        return await _studentRepository.GetAllAsync(request.Keyword);
    }
}