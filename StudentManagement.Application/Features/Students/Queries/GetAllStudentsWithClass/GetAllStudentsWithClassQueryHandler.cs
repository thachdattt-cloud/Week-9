using MediatR;
using StudentManagement.Application.DTO;
using StudentManagement.Application.Interfaces.Repositories;

namespace StudentManagement.Application.Features.Students.Queries.GetAllStudentsWithClass;

public class GetAllStudentsWithClassQueryHandler : IRequestHandler<GetAllStudentsWithClassQuery, List<StudentWithClassDto>>
{
    private readonly IStudentRepository _studentRepository;

    public GetAllStudentsWithClassQueryHandler(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public async Task<List<StudentWithClassDto>> Handle(GetAllStudentsWithClassQuery request, CancellationToken cancellationToken)
    {
        return await _studentRepository.GetAllWithClassAsync();
    }
}