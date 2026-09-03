using MediatR;
using StudentManagement.Application.DTO;
using StudentManagement.Application.Interfaces.Repositories;

namespace StudentManagement.Application.Features.Students.Queries.GetGradesDetail;

public class GetGradesDetailQueryHandler : IRequestHandler<GetGradesDetailQuery, List<GradeDetailDto>>
{
    private readonly IStudentRepository _studentRepository;

    public GetGradesDetailQueryHandler(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public async Task<List<GradeDetailDto>> Handle(GetGradesDetailQuery request, CancellationToken cancellationToken)
    {
        return await _studentRepository.GetGradesDetailAsync();
    }
}